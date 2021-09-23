using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// Responsebility: View of the graph; Create and remove nodes
/// </summary>
public class GraphViewElement : GraphView
{
    #region Members
    private readonly Vector2 nodeSize = new Vector2(150, 200);

    private int connectedCounter = 0;

    private UnityEvent onSelectionNoedsChange = new UnityEvent();
    #endregion

    #region Functions
    #region Init
    public GraphViewElement()
    {
        //Add style
        styleSheets.Add(Resources.Load<StyleSheet>("Graph"));

        //Add the ability to set zoom
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        //Add selection and drag ability
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //Add grid background
        var gridBackground = new GridBackground();
        Insert(0, gridBackground);
        gridBackground.StretchToParentSize();

        //Create entry node
        CreateNode(GraphNodeType.ENTRY_NODE, new Vector2(100, 200), new NodeAdditionalData()); ;

        CreateMiniMap();
        unserializeAndPaste += OnDuplicateClicked;
        graphViewChanged = GraphChanged;
    }

    private void AddStyle(NodeView node)
    {
        switch (node.Type)
        {
            case GraphNodeType.ENTRY_NODE:
                node.styleSheets.Add(Resources.Load<StyleSheet>("EntryNode"));
                break;
            case GraphNodeType.TYPE_1:
                node.styleSheets.Add(Resources.Load<StyleSheet>("NodeType1"));
                break;
            case GraphNodeType.TYPE_2:
                node.styleSheets.Add(Resources.Load<StyleSheet>("NodeType2"));
                break;
            case GraphNodeType.TYPE_3:
                node.styleSheets.Add(Resources.Load<StyleSheet>("NodeType3"));
                break;
            default:
                break;
        }
    }
    #endregion

    #region Actions
    public void RegisterToOnNoedsSelectionChange(UnityAction unityAction)
    {
        onSelectionNoedsChange.AddListener(unityAction);
    }

    private IEnumerator UpdateNodesTitles()
    {
        if (nodes.ToList().Count != 0)
        {
            yield return null; //Changes are update in the end of the frame.
            nodes.ForEach(x => x.title = ((NodeView)x).Type == GraphNodeType.ENTRY_NODE ? "Entry Point" : "not connected (" + ((NodeView)x).Type.ToString().ToLower().Replace('_', '-') + ")");
            connectedCounter = 0;
            NodeView firstNode = (NodeView)nodes.ToList()[0];
            MarkAsConnected(firstNode);
        }
    }

    private void MarkAsConnected(NodeView node)
    {
        if (node.Type != GraphNodeType.ENTRY_NODE)
        {
            connectedCounter++;
            node.title = node.Type == GraphNodeType.ENTRY_NODE ? "Start" : "connected  (" + node.Type.ToString().ToLower().Replace('_', '-') + "),  N-" + connectedCounter;
        }

        List<NodeView> connectedNode = new List<NodeView>();
        foreach (Edge edge in edges.ToList())
            if (((NodeView)edge.output.node).GUID == node.GUID)
                connectedNode.Add((NodeView)edge.input.node);

        for (int i = 0; i < connectedNode.Count; i++)
            MarkAsConnected(connectedNode[i]);
    }

    private void CreateMiniMap()
    {
        MiniMap miniMap = new MiniMap() { anchored = true };
        miniMap.SetPosition(new Rect(10, 30, 200, 140));
        Add(miniMap);
    }

    private Port GeneratePort(NodeView node, Direction portDiraction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDiraction, capacity, typeof(float));
    }

    public NodeView CreateNode(GraphNodeType nodeType, Vector2 position, NodeAdditionalData additionalData, string guid = "")
    {
        string GUID = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;
        NodeView node = new NodeView(GUID, nodeType, additionalData);
        node.RegisterToOnSelectChange(OnSelectionNoedsChange);

        //Add input ports
        switch (nodeType)
        {
            case GraphNodeType.ENTRY_NODE:
                AddOutputPort(node, 0);
                node.capabilities &= ~Capabilities.Movable;
                node.capabilities &= ~Capabilities.Deletable;
                node.capabilities &= ~Capabilities.Selectable;
                break;
            case GraphNodeType.TYPE_1:
                AddInputPort(node);
                AddOutputPort(node, 0);
                break;
            case GraphNodeType.TYPE_2:
                AddInputPort(node);
                AddOutputPort(node, 0);
                break;
            case GraphNodeType.TYPE_3:
                AddInputPort(node);
                AddOutputPort(node, 0);
                AddOutputPort(node, 1);
                break;
            default:
                break;
        }

        //Refresh
        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(position, nodeSize));

        //Add design
        AddStyle(node);

        //Add element
        AddElement(node);

        //Update all titles
        EditorCoroutineUtility.StartCoroutine(UpdateNodesTitles(), this);

        return node;
    }

    private void AddInputPort(NodeView node)
    {
        Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);
    }

    private void AddOutputPort(NodeView node, int portNum)
    {
        Port outputPort = GeneratePort(node, Direction.Output);
        outputPort.portName = "Output-" + portNum;
        node.outputContainer.Add(outputPort);
    }

    private IEnumerator NotifySelectionNoedsChange()
    {
        //selections is update only in the end of the frame
        yield return null;
        onSelectionNoedsChange.Invoke();
    }

    public void ClearGraph()
    {
        foreach (NodeView node in nodes.ToList().Cast<NodeView>().ToList())
        {
            edges.ToList().Where(x => x.input.node == node).ToList()
                .ForEach(edge => RemoveElement(edge));
            RemoveElement(node);
        }
    }

    public void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        Edge tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        Add(tempEdge);
    }
    #endregion

    #region Handle Events
    private GraphViewChange GraphChanged(GraphViewChange graphViewChange)
    {
        EditorCoroutineUtility.StartCoroutine(UpdateNodesTitles(), this);
        return graphViewChange;
    }

    private void OnDuplicateClicked(string operationName, string data)
    {
        List<ISelectable> toAddToSelection = new List<ISelectable>();
        List<ISelectable> toRemoveFromSelection = new List<ISelectable>();

        //Duplicate Nodes
        foreach (ISelectable selectable in selection)
        {
            if (selectable.GetType() == typeof(NodeView))
            {
                NodeView node = (NodeView)selectable;
                if (node.Type != GraphNodeType.ENTRY_NODE)
                    toAddToSelection.Add(CreateNode(node.Type, node.GetPosition().position - new Vector2(50, 50), new NodeAdditionalData(node.NodeAdditionalData), node.GUID + "-Duplicate"));
            }
            toRemoveFromSelection.Add(selectable);
        }

        //Duplicate Edges
        foreach (ISelectable selectable in selection)
        {
            if (selectable.GetType() == typeof(Edge))
            {
                Edge edge = (Edge)selectable;
                NodeView baseNode = (NodeView)edge.output.node;
                if (baseNode.Type == GraphNodeType.ENTRY_NODE)
                    continue;
                NodeView targetNode = (NodeView)edge.input.node;
                if (selection.Contains(baseNode) && selection.Contains(targetNode))
                {
                    Node newBaseNode = nodes.ToList().First(x => ((NodeView)x).GUID == baseNode.GUID + "-Duplicate");
                    Node newTargetNode = nodes.ToList().First(x => ((NodeView)x).GUID == targetNode.GUID + "-Duplicate");
                    int portIndex = int.Parse(edge.output.portName.Substring(edge.output.portName.IndexOf('-') + 1));
                    LinkNodesTogether((Port)newBaseNode.outputContainer[portIndex], (Port)newTargetNode.inputContainer[0]);
                    toAddToSelection.Add(edge);
                }
                toRemoveFromSelection.Add(selectable);
            }
        }

        toAddToSelection.ForEach(action: x => AddToSelection(x));
        toRemoveFromSelection.ForEach(action: x => RemoveFromSelection(x));
    }

    private void OnSelectionNoedsChange() => EditorCoroutineUtility.StartCoroutine(NotifySelectionNoedsChange(), this);
    #endregion

    #region Override
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (!(port.portName == "Input" && startPort.portName == "Input") &&
                    !(port.portName.StartsWith("Output") && startPort.portName.StartsWith("Output")) &&
                        startPort != port && startPort.node != port.node)
            {
                //Exclusive node connection option
                switch (((NodeView)startPort.node).Type)
                {
                    case GraphNodeType.ENTRY_NODE:
                        compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.TYPE_1:
                        if (((NodeView)port.node).Type == GraphNodeType.TYPE_2)
                            compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.TYPE_2:
                        if (((NodeView)port.node).Type == GraphNodeType.TYPE_3)
                            compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.TYPE_3:
                        if (((NodeView)port.node).Type == GraphNodeType.TYPE_1)
                            compatiblePorts.Add(port);
                        break;
                    default:
                        break;
                }
            }
        });

        return compatiblePorts;
    }
    #endregion
    #endregion
}
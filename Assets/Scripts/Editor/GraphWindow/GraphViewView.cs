using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Responsebility: View of the graph; Create and remove nodes
/// </summary>
public class GraphViewView : GraphView
{
    private readonly Vector2 nodeSize = new Vector2(150, 200);

    public GraphViewView()
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
        CreateNode(GraphNodeType.ENTRY_NODE, new Vector2(100, 200));

        CreateMiniMap();
        unserializeAndPaste += OnDuplicateClicked;
    }

    private void OnDuplicateClicked(string operationName, string data)
    {
        List<ISelectable> toAddToSelection = new List<ISelectable>();
        List<ISelectable> toRemoveFromSelection = new List<ISelectable>();
        foreach (ISelectable selectable in selection)
        {
            if (selectable.GetType() == typeof(NodeView))
            {
                NodeView node = (NodeView)selectable;
                if (node.type != GraphNodeType.ENTRY_NODE)
                    toAddToSelection.Add(CreateNode(node.type, node.GetPosition().position - new Vector2(50, 50), node.GUID + "-Duplicate"));
            }
            toRemoveFromSelection.Add(selectable);
        }
        foreach (ISelectable selectable in selection)
        {
            if (selectable.GetType() == typeof(Edge))
            {
                Edge edge = (Edge)selectable;
                NodeView baseNode = (NodeView)edge.output.node;
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

    private void CreateMiniMap()
    {
        MiniMap miniMap = new MiniMap() { anchored = true };
        miniMap.SetPosition(new Rect(10, 30, 200, 140));
        Add(miniMap);
    }

    private Port GeneratePort(NodeView node, Direction portDiraction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDiraction, capacity, typeof(float)); // Arbitrary type
    }

    public NodeView CreateNode(GraphNodeType nodeType, Vector2 position, string guid = "")
    {
        NodeView node = new NodeView
        {
            title = nodeType == GraphNodeType.ENTRY_NODE? "Start" : "not connected  (" + nodeType.ToString().ToLower().Replace('_','-') +")", //TODO need to be automatic
            GUID = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid,
            type = nodeType
        };

        if (nodeType != GraphNodeType.ENTRY_NODE)
        {
            Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);
        }
        else
        {
            node.capabilities &= ~Capabilities.Movable;
            node.capabilities &= ~Capabilities.Deletable;
        }

        Port outputPort = GeneratePort(node, Direction.Output);
        outputPort.portName = "Output-0";
        node.outputContainer.Add(outputPort);

        if(nodeType == GraphNodeType.TYPE_3)
        {
            Port secondOutputPort = GeneratePort(node, Direction.Output);
            secondOutputPort.portName = "Output-1";
            node.outputContainer.Add(secondOutputPort);
        }

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(position, nodeSize));
        AddStyle(node);

        AddElement(node);

        return node;
    }

    private void AddStyle(NodeView node)
    {
        switch (node.type)
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

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (!(port.portName == "Input" && startPort.portName == "Input") &&
                    !(port.portName.StartsWith("Output") && startPort.portName.StartsWith("Output")) &&
                        startPort != port && startPort.node != port.node)
            {
                //Exclusive node connection option //TODO by config
                switch (((NodeView)startPort.node).type)
                {
                    case GraphNodeType.ENTRY_NODE:
                        compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.TYPE_1:
                        if(((NodeView)port.node).type == GraphNodeType.TYPE_2)
                            compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.TYPE_2:
                        if (((NodeView)port.node).type == GraphNodeType.TYPE_3)
                            compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.TYPE_3:
                        if (((NodeView)port.node).type == GraphNodeType.TYPE_1)
                            compatiblePorts.Add(port);
                        break;
                    default:
                        break;
                }
            }
        });

        return compatiblePorts;
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
}

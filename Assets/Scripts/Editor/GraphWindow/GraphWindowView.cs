using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphWindowView : EditorWindow, IGraphWindowView
{
    #region Members
    private IGraphWindowController mController;

    private GraphViewElement graph;

    private CreateNodeButtonClickEvent createNodeButtonClickEvent = new CreateNodeButtonClickEvent();

    private UnityEvent onSelectionChangeEvent = new UnityEvent();
    private UnityEvent onDisableEvent = new UnityEvent();
    #endregion

    #region Functions

    #region Init
    [MenuItem("Graph/Graph Window")]
    public static void Open()
    {
        GetWindow<GraphWindowView>().titleContent = new GUIContent("Graph");
    }

    public void OnEnable()
    {
        mController = SingleManager.Get<IGraphWindowController>();
        mController.SetView(this);
        graph.RegisterToOnNoedsSelectionChange(OnNoedsSelectionChange);
    }

    public void ConsturctGraph()
    {
        graph = new GraphViewElement
        {
            name = "Graph"
        };

        graph.StretchToParentSize();
        rootVisualElement.Add(graph);
        GenerateToolbar();
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        //Open inspector button
        Button inspectorWindowOpenButton = new Button(() => { GetWindow<GraphInspectorWindowView>().titleContent = new GUIContent("Inspector Window"); });
        inspectorWindowOpenButton.text = "Open Inspector Window";
        toolbar.Add(inspectorWindowOpenButton);

        //Create node 1 button
        Button nodeCreateButton1 = new Button(() => createNodeButtonClickEvent.Invoke(GraphNodeType.TYPE_1));
        nodeCreateButton1.text = "Create Node Type 1";
        toolbar.Add(nodeCreateButton1);

        //Create node 2 button
        Button nodeCreateButton2 = new Button(() => createNodeButtonClickEvent.Invoke(GraphNodeType.TYPE_2));
        nodeCreateButton2.text = "Create Node Type 2";
        toolbar.Add(nodeCreateButton2);

        //Create node 3 button
        Button nodeCreateButton3 = new Button(() => createNodeButtonClickEvent.Invoke(GraphNodeType.TYPE_3));
        nodeCreateButton3.text = "Create Node Type 3";
        toolbar.Add(nodeCreateButton3);

        //Open delete all button
        Button DeleteAllButton = new Button(() => { ClearGraph(); });
        DeleteAllButton.text = "Delete all";
        toolbar.Add(DeleteAllButton);

        //Add the toolbar to the editor window
        rootVisualElement.Add(toolbar);
    }
    #endregion

    #region Handle Events
    private void OnNoedsSelectionChange() => onSelectionChangeEvent.Invoke();

    public void OnDisable()
    {
        onDisableEvent.Invoke();
        rootVisualElement.Remove(graph);
        GetWindow<GraphInspectorWindowView>().Close();
    }
    #endregion

    #region Actions
    public void CreateNode(GraphNodeType nodeType, Vector2 position, NodeAdditionalData additionalData)
    {
        graph.CreateNode(nodeType, position, additionalData);
    }

    public void LoadGraphData(GraphData graphData)
    {
        if (graphData != null)
        {
            ClearGraph();
            foreach (GraphNodeData nodeData in graphData.Nodes)
                graph.CreateNode(nodeData.Type, nodeData.Position, nodeData.AdditionalData, nodeData.GUID);

            foreach (GraphNodeLinkData link in graphData.Links)
            {
                Node baseNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.BaseNodeGuid);
                Node targetNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.TargetNodeGuid);
                int portIndex = int.Parse(link.PortName.Substring(link.PortName.IndexOf('-') + 1));
                graph.LinkNodesTogether((Port)baseNode.outputContainer[portIndex], (Port)targetNode.inputContainer[0]);
            }
        }
        else
            Debug.Log("Loading default graph");
    }

    public void ClearGraph() => graph.ClearGraph();

    public void RegisterToOnCreateNodeClickEvent(UnityAction<GraphNodeType> action)
    {
        createNodeButtonClickEvent.AddListener(action);
    }

    public void RegisterToOnNodesSelectionChange(UnityAction action)
    {
        onSelectionChangeEvent.AddListener(action);
    }

    public void RegisterToOnDisable(UnityAction action)
    {
        onDisableEvent.AddListener(action);
    }

    public void InjectAdditionalDataToSelectionNodes(NodeAdditionalData nodeAdditionalData)
    {
        foreach (ISelectable selectable in graph.selection)
        {
            if (selectable.GetType() == typeof(NodeView))
            {
                NodeView node = (NodeView)selectable;
                node.NodeAdditionalData = new NodeAdditionalData(nodeAdditionalData);
            }
        }
    }
    #endregion

    #region GetData
    /// <summary>
    /// Get all Nodes and connections except Entry Node 
    /// </summary>
    /// <returns></returns>
    public GraphData GetGraphData()
    {
        GraphData graphData = new GraphData();
        foreach (NodeView node in graph.nodes.ToList().Cast<NodeView>().ToList())
        {
            graphData.Nodes.Add(new GraphNodeData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
                Type = node.Type,
                AdditionalData = node.NodeAdditionalData
            });
        }
        foreach (Edge edge in graph.edges.ToList())
        {
            graphData.Links.Add(new GraphNodeLinkData()
            {
                BaseNodeGuid = ((NodeView)edge.output.node).GUID,
                TargetNodeGuid = ((NodeView)edge.input.node).GUID,
                PortName = edge.output.portName
            });
        }
        return graphData;
    }

    public List<GraphNodeData> GetNodesSelectionList()
    {
        List<GraphNodeData> nodesSelection = new List<GraphNodeData>();
        foreach (ISelectable selectable in graph.selection)
        {
            if (selectable.GetType() == typeof(NodeView))
            {
                NodeView node = (NodeView)selectable;
                if (node.Type != GraphNodeType.ENTRY_NODE)
                    nodesSelection.Add(new GraphNodeData()
                    {
                        AdditionalData = node.NodeAdditionalData,
                        GUID = node.GUID,
                        Position = node.GetPosition().position,
                        Type = node.Type
                    });
            }
        }
        return nodesSelection;
    }
    #endregion
    #endregion
}

public class CreateNodeButtonClickEvent : UnityEvent<GraphNodeType> { }
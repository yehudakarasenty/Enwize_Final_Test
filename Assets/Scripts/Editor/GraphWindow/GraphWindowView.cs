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

    private UnityEvent onSelectionChange = new UnityEvent();
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
    private void OnNoedsSelectionChange() => onSelectionChange.Invoke();

    public void OnDisable()
    {
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
        ClearGraph();
        foreach (GraphNodeData nodeData in graphData.nodes)
            graph.CreateNode(nodeData.type, nodeData.Position, nodeData.additionalData, nodeData.GUID);

        foreach (GraphNodeLinkData link in graphData.links)
        {
            Node baseNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.BaseNodeGuid);
            Node targetNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.TargetNodeGuid);
            int portIndex = int.Parse(link.portName.Substring(link.portName.IndexOf('-') + 1));
            graph.LinkNodesTogether((Port)baseNode.outputContainer[portIndex], (Port)targetNode.inputContainer[0]);
        }
    }

    public void ClearGraph() => graph.ClearGraph();

    public void RegisterToOnCreateNodeClickEvent(UnityAction<GraphNodeType> action)
    {
        createNodeButtonClickEvent.AddListener(action);
    }

    public void RegisterToOnNodesSelectionChange(UnityAction action)
    {
        onSelectionChange.AddListener(action);
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
    public GraphData GetGraphData()
    {
        GraphData graphData = new GraphData();
        foreach (NodeView node in graph.nodes.ToList().Cast<NodeView>().ToList())
        {
            graphData.nodes.Add(new GraphNodeData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
                type = node.Type,
                additionalData = node.NodeAdditionalData
            });
        }
        foreach (Edge edge in graph.edges.ToList())
        {
            graphData.links.Add(new GraphNodeLinkData()
            {
                BaseNodeGuid = ((NodeView)edge.output.node).GUID,
                TargetNodeGuid = ((NodeView)edge.input.node).GUID,
                portName = edge.output.portName
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
                        additionalData = node.NodeAdditionalData,
                        GUID = node.GUID,
                        Position = node.GetPosition().position,
                        type = node.Type
                    });
            }
        }
        return nodesSelection;
    }
    #endregion
    #endregion
}

public class CreateNodeButtonClickEvent : UnityEvent<GraphNodeType> { }
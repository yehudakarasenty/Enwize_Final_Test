using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// Responsebility: Open and close graph window and comunicate with dependecies + save and load graph data
/// </summary>
public class GraphWindowView : EditorWindow, IGraphWindowView
{
    private IGraphWindowController mController;

    private GraphViewView graph;

    private CreateNodeButtonClickEvent createNodeButtonClickEvent = new CreateNodeButtonClickEvent();

    [MenuItem("Graph/Graph Window")]
    public static void Open() 
    {
        GetWindow<GraphWindowView>().titleContent = new GUIContent("Graph");
    }

    public void OnEnable()
    {
        mController = SingleManager.Get<IGraphWindowController>();
        mController.SetView(this);
    }

    public void ConsturctGraph()
    {
        graph = new GraphViewView
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

        Button inspectorWindowOpenButton = new Button(() => { GetWindow<GraphInspectorWindowView>().titleContent = new GUIContent("Inspector Window"); });
        inspectorWindowOpenButton.text = "Open Inspector Window";
        toolbar.Add(inspectorWindowOpenButton);

        Button nodeCreateButton1 = new Button(() => createNodeButtonClickEvent.Invoke(GraphNodeType.NODE_1));
        nodeCreateButton1.text = "Create Node Type 1";
        toolbar.Add(nodeCreateButton1);

        Button nodeCreateButton2 = new Button(() => createNodeButtonClickEvent.Invoke(GraphNodeType.NODE_2));
        nodeCreateButton2.text = "Create Node Type 2";
        toolbar.Add(nodeCreateButton2);

        Button nodeCreateButton3 = new Button(() => createNodeButtonClickEvent.Invoke(GraphNodeType.NODE_3));
        nodeCreateButton3.text = "Create Node Type 3";
        toolbar.Add(nodeCreateButton3);

        //Add the toolbar to the editor window
        rootVisualElement.Add(toolbar);
    }

    public void CreateNode(GraphNodeType nodeType, Vector2 position)
    {
        graph.CreateNode(nodeType, position);
    }

    public GraphData GetGraphData()
    {
        GraphData graphData = new GraphData();
        foreach(NodeView node in graph.nodes.ToList().Cast<NodeView>().ToList())
        {
            graphData.nodes.Add(new GraphNodeData()
            {
                GUID = node.GUID,
                Position = node.GetPosition().position,
                type = node.type
            });
        }
        foreach (Edge edgd in graph.edges.ToList())
        {
            graphData.links.Add(new GraphNodeLinkData()
            {
                BaseNodeGuid = ((NodeView)edgd.output.node).GUID,
                TargetNodeGuid = ((NodeView)edgd.input.node).GUID,
                portName = edgd.output.portName
            });
        }
        return graphData;
    }

    public void LoadGraphData(GraphData graphData)
    {
        ClearGraph();
        foreach (GraphNodeData nodeData in graphData.nodes)
        {
            graph.CreateNode(nodeData.type, nodeData.Position, nodeData.GUID);
        }

        foreach (GraphNodeLinkData link in graphData.links)
        {
            Node baseNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.BaseNodeGuid);
            Node targetNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.TargetNodeGuid);
            int portIndex = int.Parse(link.portName.Substring(link.portName.IndexOf('-') + 1));
            graph.LinkNodesTogether((Port)baseNode.outputContainer[portIndex], (Port)targetNode.inputContainer[0]);
        }
    }

    public void ClearGraph()
    {
        graph.ClearGraph();
    }

    public void RegisterToOnCreateNodeClickEvent(UnityAction<GraphNodeType> action) 
    {
        createNodeButtonClickEvent.AddListener(action);
    }

    public void OnDisable()
    {
        rootVisualElement.Remove(graph);
        GetWindow<GraphInspectorWindowView>().Close(); //TODO Myabe by controller interface
    }
}

public class CreateNodeButtonClickEvent : UnityEvent<GraphNodeType> { }
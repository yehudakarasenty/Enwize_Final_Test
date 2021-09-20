using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Responsebility: Open and close graph window and comunicate with dependecies + save and load graph data
/// </summary>
public class GraphWindow : EditorWindow, IGraphWindow
{
    private Graph graph;

    private void Awake()
    {
        SingleManager.Register<IGraphWindow>(this);
    }

    [MenuItem("Graph/Graph Window")]
    public static void Open() 
    {
        GetWindow<GraphWindow>().titleContent = new GUIContent("Graph");
    }

    private void OnEnable()
    {
        ConsturctGraph();
        GenerateToolbar();
    }

    private void ConsturctGraph()
    {
        graph = new Graph
        {
            name = "Graph"
        };

        graph.StretchToParentSize();
        rootVisualElement.Add(graph);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button extraWindowOpenButton = new Button(() => { GetWindow<ExtraEditorWindow>().titleContent = new GUIContent("Editor Window"); });
        extraWindowOpenButton.text = "Open Extra Editor Window";
        toolbar.Add(extraWindowOpenButton);

        Button nodeCreateButton1 = new Button(() =>{ graph.CreateNode("Node_1", GraphNodeType.NODE_1); }); //TO Understand: what is the parameter "node name" that I provide the string "Node"
        nodeCreateButton1.text = "Create Node Type 1";
        toolbar.Add(nodeCreateButton1);

        Button nodeCreateButton2 = new Button(() => { graph.CreateNode("Node_2", GraphNodeType.NODE_2); }); //TO Understand: what is the parameter "node name" that I provide the string "Node"
        nodeCreateButton2.text = "Create Node Type 2";
        toolbar.Add(nodeCreateButton2);

        Button nodeCreateButton3 = new Button(() => { graph.CreateNode("Node_3", GraphNodeType.NODE_3); }); //TO Understand: what is the parameter "node name" that I provide the string "Node"
        nodeCreateButton3.text = "Create Node Type 3";
        toolbar.Add(nodeCreateButton3);

        //Add the toolbar to the editor window
        rootVisualElement.Add(toolbar);
    }

    public void SaveGraph(string fileName)
    {
        Debug.LogError("SaveGraph");
        GraphData graphData = new GraphData();
        foreach(GraphNode node in graph.nodes.ToList().Cast<GraphNode>().ToList())
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
                BaseNodeGuid = ((GraphNode)edgd.output.node).GUID,
                TargetNodeGuid = ((GraphNode)edgd.input.node).GUID,
            });
        }

        string s = JsonService.ObjectToJson(graphData, false, true);
        Debug.LogError(s);
    }

    public void LoadGraph(string fileName)
    {
        //TODO
        Debug.LogError("LoadGraph");
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graph);
        GetWindow<ExtraEditorWindow>().Close();
    }

    private void OnDestroy()
    {
        SingleManager.Remove<IGraphWindow>();
    }
}

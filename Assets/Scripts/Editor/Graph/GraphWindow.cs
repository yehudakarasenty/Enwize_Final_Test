using UnityEditor;
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

    public void RequestDataOperation(string fileName, bool save)
    {
        //TODO
        throw new System.NotImplementedException();
    }

    private  GraphData GetGraphData()
    {
        //TODO
        throw new System.NotImplementedException();
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

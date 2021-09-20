using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphWindow : EditorWindow
{
    private GraphWindowView view;

    [MenuItem("Graph/Graph Window")]
    public static void Open() 
    {
        GetWindow<GraphWindow>().titleContent = new GUIContent("Graph");
    }

    private void OnEnable()
    {
        ConsturctView();
        GenerateToolbar();
    }

    private void ConsturctView()
    {
        view = new GraphWindowView
        {
            name = "Graph"
        };

        view.StretchToParentSize();
        rootVisualElement.Add(view);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button nodeCreateButton1 = new Button(() =>{ view.CreateNode("Node_1", GraphNodeType.NODE_1); }); //TO Understand: what is the parameter "node name" that I provide the string "Node"
        nodeCreateButton1.text = "Create Node Type 1";
        toolbar.Add(nodeCreateButton1);

        Button nodeCreateButton2 = new Button(() => { view.CreateNode("Node_2", GraphNodeType.NODE_2); }); //TO Understand: what is the parameter "node name" that I provide the string "Node"
        nodeCreateButton2.text = "Create Node Type 2";
        toolbar.Add(nodeCreateButton2);

        Button nodeCreateButton3 = new Button(() => { view.CreateNode("Node_3", GraphNodeType.NODE_3); }); //TO Understand: what is the parameter "node name" that I provide the string "Node"
        nodeCreateButton3.text = "Create Node Type 3";
        toolbar.Add(nodeCreateButton3);

        //Add the toolbar to the editor window
        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(view);
    }
}

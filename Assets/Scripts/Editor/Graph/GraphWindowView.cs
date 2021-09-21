using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Responsebility: Open and close graph window and comunicate with dependecies + save and load graph data
/// </summary>
public class GraphWindowView : EditorWindow, IGraphWindow
{
    private const string FILE_PATH = "C:/Users/yehud/Desktop/JsonsFiles/"; //TODO: to config

    private GraphViewView graph;

    private void Awake()
    {
        SingleManager.Register<IGraphWindow>(this);
    }

    [MenuItem("Graph/Graph Window")]
    public static void Open() 
    {
        GetWindow<GraphWindowView>().titleContent = new GUIContent("Graph");
    }

    private void OnEnable()
    {
        ConsturctGraph();
        GenerateToolbar();
    }

    private void ConsturctGraph()
    {
        graph = new GraphViewView
        {
            name = "Graph"
        };

        graph.StretchToParentSize();
        rootVisualElement.Add(graph);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button extraWindowOpenButton = new Button(() => { GetWindow<GraphInspectorWindowView>().titleContent = new GUIContent("Editor Window"); });
        extraWindowOpenButton.text = "Open Extra Editor Window";
        toolbar.Add(extraWindowOpenButton);

        Button nodeCreateButton1 = new Button(() =>{ graph.CreateNode(GraphNodeType.NODE_1, Vector2.zero); });
        nodeCreateButton1.text = "Create Node Type 1";
        toolbar.Add(nodeCreateButton1);

        Button nodeCreateButton2 = new Button(() => { graph.CreateNode(GraphNodeType.NODE_2, Vector2.zero); });
        nodeCreateButton2.text = "Create Node Type 2";
        toolbar.Add(nodeCreateButton2);

        Button nodeCreateButton3 = new Button(() => { graph.CreateNode(GraphNodeType.NODE_3, Vector2.zero); });
        nodeCreateButton3.text = "Create Node Type 3";
        toolbar.Add(nodeCreateButton3);

        //Add the toolbar to the editor window
        rootVisualElement.Add(toolbar);
    }

    public void SaveGraph(string fileName)
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

        JsonService.WriteJsonFile(graphData, FILE_PATH + fileName);
    }

    public void LoadGraph(string fileName)
    {
        graph.ClearGraph();

        GraphData graphData = JsonService.ReadJsonFile<GraphData>(FILE_PATH + fileName);

        foreach (GraphNodeData nodeData in graphData.nodes)
        {
            graph.CreateNode(nodeData.type, nodeData.Position, nodeData.GUID);
        }

        foreach (GraphNodeLinkData link in graphData.links)
        {
            Node baseNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.BaseNodeGuid);
            Node targetNode = graph.nodes.ToList().First(x => ((NodeView)x).GUID == link.TargetNodeGuid);
            int portIndex = int.Parse(link.portName.Substring(link.portName.IndexOf('-') + 1));
            LinkNodesTogether((Port)baseNode.outputContainer[portIndex], (Port)targetNode.inputContainer[0]);
        }
    }

    private void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        Edge tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        graph.Add(tempEdge);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graph);
        GetWindow<GraphInspectorWindowView>().Close();
    }

    private void OnDestroy()
    {
        SingleManager.Remove<IGraphWindow>();
    }
}

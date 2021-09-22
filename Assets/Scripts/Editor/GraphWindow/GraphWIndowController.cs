using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GraphWIndowController : IGraphWindowController
{
    private const string FILE_PATH = "C:/Users/yehud/Desktop/JsonsFiles/"; //TODO: to config

    private IGraphWindowView mView;

    public GraphWIndowController()
    {
        SingleManager.Register<IGraphWindowController>(this);
        EditorApplication.playModeStateChanged += PlayModeChanged; 
    }

    public void InitDependencies()
    {
        
    }

    public void SetView(IGraphWindowView view)
    {
        mView = view;
        mView.ConsturctGraph();
        mView.RegisterToOnCreateNodeClickEvent(new UnityAction<GraphNodeType>(OnCreateNodeButtonClick));
    }

    void PlayModeChanged(PlayModeStateChange playModeState)
    {
        mView.ClearGraph();
    }

    private void OnCreateNodeButtonClick(GraphNodeType nodeType)
    {
        mView.CreateNode(nodeType, Vector2.zero);
    }

    public void LoadGraph(string fileName)
    {
        mView.LoadGraphData(JsonService.ReadJsonFile<GraphData>(FILE_PATH + fileName));
    }

    public void SaveGraph(string fileName)
    {
        JsonService.WriteJsonFile(mView.GetGraphData(), FILE_PATH + fileName);
    }
}

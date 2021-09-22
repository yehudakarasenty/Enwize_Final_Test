using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GraphWIndowController : IGraphWindowController
{
    private const string FILE_PATH = "C:/Users/yehud/Desktop/JsonsFiles/"; //TODO: to config

    private IGraphWindowView mView;

    public List<GraphNodeData> NodesSelections => mView.GetNodesSelectionList();

    private UnityEvent onNodesSelectionsChange = new UnityEvent();

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
        mView.RegisterToOnNodesSelectionChange(()=> onNodesSelectionsChange.Invoke());
    }

    void PlayModeChanged(PlayModeStateChange playModeState)
    {
        if (mView != null)
            mView.ClearGraph();
    }

    private void OnCreateNodeButtonClick(GraphNodeType nodeType)
    {
        mView.CreateNode(nodeType, Vector2.zero);
    }

    public void LoadGraph(string fileName)
    {
        if (mView != null)
            mView.LoadGraphData(JsonService.ReadJsonFile<GraphData>(FILE_PATH + fileName));
    }

    public void SaveGraph(string fileName)
    {
        if (mView != null)
            JsonService.WriteJsonFile(mView.GetGraphData(), FILE_PATH + fileName);
    }

    public void RegisterToNodeSelectionsChange(UnityAction action)
    {
        onNodesSelectionsChange.AddListener(action);
    }
}

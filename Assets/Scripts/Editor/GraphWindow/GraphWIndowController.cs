using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GraphWIndowController : IGraphWindowController
{
    private string filePath;

    private IGraphWindowView mView;
    private IGraphInspectorWindowController mGraphInspectorWindowController;

    public List<GraphNodeData> NodesSelections => mView.GetNodesSelectionList();

    private UnityEvent onNodesSelectionsChange = new UnityEvent();

    public GraphWIndowController()
    {
        SingleManager.Register<IGraphWindowController>(this);
        EditorApplication.playModeStateChanged += PlayModeChanged; 
    }

    public void InitDependencies()
    {
        mGraphInspectorWindowController = SingleManager.Get<IGraphInspectorWindowController>();
        mGraphInspectorWindowController.RegisterToOnAdditionalDataFieldsChange(OnAdditionalDataInspectorChange);
        filePath = ConfigUtility.Configuration.SaveLoadPath;
    }

    private void OnAdditionalDataInspectorChange()
    {
        NodeAdditionalData nodeAdditionalData = mGraphInspectorWindowController.NodeAdditionalDataFields();
        mView.InjectAdditionalDataToSelectionNodes(nodeAdditionalData);
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
        mView.CreateNode(nodeType, Vector2.zero, new NodeAdditionalData());
    }

    public void LoadGraph(string fileName)
    {
        if (mView != null)
            mView.LoadGraphData(JsonService.ReadJsonFile<GraphData>(filePath + fileName));
        Debug.Log("Graph Loaded");
    }

    public void SaveGraph(string fileName)
    {
        if (mView != null)
            JsonService.WriteJsonFile(mView.GetGraphData(), filePath + fileName);
        Debug.Log("Graph Saved");
    }

    public void RegisterToNodeSelectionsChange(UnityAction action)
    {
        onNodesSelectionsChange.AddListener(action);
    }
}

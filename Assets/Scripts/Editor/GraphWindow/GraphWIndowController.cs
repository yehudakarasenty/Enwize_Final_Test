using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsibility: Control GraphWindow Window and comunicate with dependencies
/// </summary>
public class GraphWindowController : IGraphWindowController
{
    #region Members
    #region Dependecis
    private IGraphInspectorWindowController mGraphInspectorWindowController;
    #endregion

    private string filePath;

    private IGraphWindowView mView;

    public List<GraphNodeData> NodesSelections => mView.GetNodesSelectionList();

    private UnityEvent onNodesSelectionsChange = new UnityEvent();
    #endregion

    #region Functions
    #region Init
    public GraphWindowController()
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

    public void SetView(IGraphWindowView view)
    {
        mView = view;
        mView.ConsturctGraph();
        mView.RegisterToOnCreateNodeClickEvent(new UnityAction<GraphNodeType>(OnCreateNodeButtonClick));
        mView.RegisterToOnNodesSelectionChange(() => onNodesSelectionsChange.Invoke());
    }
    #endregion

    #region Handle Events
    private void OnAdditionalDataInspectorChange()
    {
        NodeAdditionalData nodeAdditionalData = mGraphInspectorWindowController.NodeAdditionalDataFields();
        mView.InjectAdditionalDataToSelectionNodes(nodeAdditionalData);
    }

    void PlayModeChanged(PlayModeStateChange playModeState)
    {
        if (mView != null)
            mView.ClearGraph();
    }

    private void OnCreateNodeButtonClick(GraphNodeType nodeType) => mView.CreateNode(nodeType, Vector2.zero, new NodeAdditionalData());
    #endregion

    #region Actions
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

    public void RegisterToNodeSelectionsChange(UnityAction action) => onNodesSelectionsChange.AddListener(action);
    #endregion
    #endregion
}

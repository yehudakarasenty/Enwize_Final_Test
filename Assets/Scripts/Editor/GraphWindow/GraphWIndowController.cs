using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsibility: Control GraphWindow Window and comunicate with dependencies
/// </summary>
public class GraphWindowController : IGraphWindowController
{
    #region Const
    private const string GRAPH_DATA_KEY_FOR_EDITOR_MODE = "LastGraphDataEditorMode";
    private const string GRAPH_DATA_KEY_FOR_PLAY_MODE = "LastGraphDataPlayMode";
    #endregion

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
        mView.RegisterToOnDisable(() => SaveEditorPlayGraph());
        if (EditorApplication.isPlaying)
            LoadPlayModeGraph();
        else
            LoadEditorModeGraph();
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
        if(playModeState == PlayModeStateChange.EnteredPlayMode || playModeState == PlayModeStateChange.EnteredEditMode)
            LoadEditorModeGraph();
    }

    private void OnCreateNodeButtonClick(GraphNodeType nodeType) => mView.CreateNode(nodeType, Vector2.zero, new NodeAdditionalData());
    #endregion

    #region Actions
    public void LoadGraph(string fileName)
    {
        if (mView != null)
            mView.LoadGraphData(JsonService.ReadJsonFile<GraphData>(filePath + fileName));
    }

    private void LoadPlayModeGraph()
    {
        mView.LoadGraphData(JsonService.JsonToObject<GraphData>(EditorPrefs.GetString(GRAPH_DATA_KEY_FOR_PLAY_MODE)));
    }

    private void LoadEditorModeGraph()
    {
        mView.LoadGraphData(JsonService.JsonToObject<GraphData>(EditorPrefs.GetString(GRAPH_DATA_KEY_FOR_EDITOR_MODE)));
    }

    public void SaveGraph(string fileName)
    {
        if (mView != null)
            JsonService.WriteJsonFile(mView.GetGraphData(), filePath + fileName);
    }

    private void SaveEditorPlayGraph()
    {
        if (EditorApplication.isPlaying)
            EditorPrefs.SetString(GRAPH_DATA_KEY_FOR_PLAY_MODE, JsonService.ObjectToJson(mView.GetGraphData()));
        else
            EditorPrefs.SetString(GRAPH_DATA_KEY_FOR_EDITOR_MODE, JsonService.ObjectToJson(mView.GetGraphData()));
    }

    public void RegisterToNodeSelectionsChange(UnityAction action) => onNodesSelectionsChange.AddListener(action);
    #endregion
    #endregion
}

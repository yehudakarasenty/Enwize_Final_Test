using UnityEngine.Events;

/// <summary>
/// Responsibility: Control GraphInspector Window and comunicate with dependencies
/// </summary>
public class GraphInspectorController : IGraphInspectorWindowController
{
    #region Members
    #region Dependencies
    private IGraphWindowController mGraphWindowController;
    #endregion

    private IGraphInspectorWindowView mView;

    private UnityEvent onAdditionalDataFieldsChange = new UnityEvent();
    #endregion

    #region Functions
    #region Init
    public GraphInspectorController()
    {
        SingleManager.Register<IGraphInspectorWindowController>(this);
    }

    public void InitDependencies()
    {
        mGraphWindowController = SingleManager.Get<IGraphWindowController>();
        mGraphWindowController.RegisterToNodeSelectionsChange(NodeSelectionsChange);
    }

    public void SetView(IGraphInspectorWindowView view)
    {
        mView = view;
        mView.ConsturctView();
        mView.RegisterToOnAdditionalDataFieldsChange(OnAdditionalDataFieldsChange);
        NodeSelectionsChange();
    }
    #endregion

    #region Actions
    private bool AdditionalDataIsEqual(NodeAdditionalData node1, NodeAdditionalData node2)
    {
        return node1.mySpecialNumber == node2.mySpecialNumber && node1.mySpecialSecret == node2.mySpecialSecret;
    }

    public void RegisterToOnAdditionalDataFieldsChange(UnityAction action)
    {
        onAdditionalDataFieldsChange.AddListener(action);
    }
    #endregion

    #region GetData
    public NodeAdditionalData NodeAdditionalDataFields()
    {
        if (mView != null)
            return mView.GetNodeAdditionalDataFields();
        else
            return new NodeAdditionalData();
    }
    #endregion

    #region Events Hande
    private void NodeSelectionsChange()
    {
        if (mView != null)
        {
            mView.HideAdditionalDataFields();
            if (mGraphWindowController.NodesSelections.Count > 0 &&
                 mGraphWindowController.NodesSelections.TrueForAll
                 (x => AdditionalDataIsEqual(x.additionalData, mGraphWindowController.NodesSelections[0].additionalData)))
                mView.ShowAdditionalDataFields(mGraphWindowController.NodesSelections[0].additionalData);
        }
    }

    public void LoadClicked(string fileName)=> mGraphWindowController.LoadGraph(fileName);

    public void SaveClicked(string fileName) => mGraphWindowController.SaveGraph(fileName);

    private void OnAdditionalDataFieldsChange()=> onAdditionalDataFieldsChange.Invoke();
    #endregion
    #endregion
}

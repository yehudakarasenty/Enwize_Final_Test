using UnityEngine.Events;

public class GraphInspectorController : IGraphInspectorWindowController
{
    private IGraphWindowController mGraphWindowController;
    private IGraphInspectorWindowView mView;

    private UnityEvent onAdditionalDataFieldsChange = new UnityEvent();

    public GraphInspectorController()
    {
        SingleManager.Register<IGraphInspectorWindowController>(this);
    }

    public void InitDependencies()
    {
        mGraphWindowController = SingleManager.Get<IGraphWindowController>();
        mGraphWindowController.RegisterToNodeSelectionsChange(NodeSelectionsChange);
    }

    private void NodeSelectionsChange()
    {
        if (mView != null)
        {
            mView.HideAdditionalDataFields();
            if (mGraphWindowController.NodesSelections.Count > 0 &&
                 mGraphWindowController.NodesSelections.TrueForAll
                 (x => AdditionalDataIsEqual(x.additionalData , mGraphWindowController.NodesSelections[0].additionalData)))
                mView.ShowAdditionalDataFields(mGraphWindowController.NodesSelections[0].additionalData);
        }
    }

    private bool AdditionalDataIsEqual(NodeAdditionalData node1, NodeAdditionalData node2)
    {
        return node1.mySpecialNumber == node2.mySpecialNumber && node1.mySpecialSecret == node2.mySpecialSecret;
    }

    public void SetView(IGraphInspectorWindowView view)
    {
        mView = view;
        mView.ConsturctView();
        mView.RegisterToOnAdditionalDataFieldsChange(OnAdditionalDataFieldsChange);
        NodeSelectionsChange();
    }

    public void LoadClicked(string fileName)
    {
        mGraphWindowController.LoadGraph(fileName);
    }

    public void SaveClicked(string fileName)
    {
        mGraphWindowController.SaveGraph(fileName);
    }

    public void RegisterToOnAdditionalDataFieldsChange(UnityAction action)
    {
        onAdditionalDataFieldsChange.AddListener(action);
    }

    private void OnAdditionalDataFieldsChange()
    {
        onAdditionalDataFieldsChange.Invoke();
    }

    public NodeAdditionalData NodeAdditionalDataFields()
    {
        if (mView != null)
            return mView.GetNodeAdditionalDataFields();
        else
            return new NodeAdditionalData();
    }
}

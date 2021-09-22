public class GraphInspectorController : IGraphInspectorWindowController
{
    private IGraphWindowController mGraphWindowController;
    private IGraphInspectorWindowView mView;

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
            mView.HideExtraDataFields();
            if (mGraphWindowController.NodesSelections.Count > 0 &&
                 mGraphWindowController.NodesSelections.TrueForAll
                 (x => ExtraDataIsEqual(x.extraData , mGraphWindowController.NodesSelections[0].extraData)))
                mView.ShowExtraDataFields(mGraphWindowController.NodesSelections[0].extraData);
        }
    }

    private bool ExtraDataIsEqual(NodeExtraData node1, NodeExtraData node2)
    {
        return node1.mySpecialNumber == node2.mySpecialNumber && node1.mySpecialSecret == node2.mySpecialSecret;
    }

    public void SetView(IGraphInspectorWindowView view)
    {
        mView = view;
        mView.ConsturctView();
    }

    public void LoadClicked(string fileName)
    {
        mGraphWindowController.LoadGraph(fileName);
    }

    public void SaveClicked(string fileName)
    {
        mGraphWindowController.SaveGraph(fileName);
    }
}

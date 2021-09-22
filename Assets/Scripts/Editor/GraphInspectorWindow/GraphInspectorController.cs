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

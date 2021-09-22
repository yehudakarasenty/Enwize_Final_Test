public interface IGraphInspectorWindowController : IController
{
    void SetView(IGraphInspectorWindowView view);

    void SaveClicked(string fileName);

    void LoadClicked(string fileName);
}
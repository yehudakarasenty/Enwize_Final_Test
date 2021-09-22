public interface IGraphWindowController : IController
{
    void SetView(IGraphWindowView view);
    void LoadGraph(string fileName);
    void SaveGraph(string fileName);
}
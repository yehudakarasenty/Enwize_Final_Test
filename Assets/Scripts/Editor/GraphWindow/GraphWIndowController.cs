using System.Collections;
using UnityEngine;


public class GraphWIndowController : IGraphWindowController
{
    private const string FILE_PATH = "C:/Users/yehud/Desktop/JsonsFiles/"; //TODO: to config

    private IGraphWindowView mView;

    public GraphWIndowController()
    {
        SingleManager.Register<IGraphWindowController>(this);
    }

    public void InitDependencies()
    {
        
    }

    public void SetView(IGraphWindowView view)
    {
        mView = view;
        mView.ConsturctGraph();
    }

    public void LoadGraph(string fileName)
    {
        mView.LoadGraphData(JsonService.ReadJsonFile<GraphData>(FILE_PATH + fileName));
    }

    public void SaveGraph(string fileName)
    {
        JsonService.WriteJsonFile(mView.GetGraphData(), FILE_PATH + fileName);
    }
}

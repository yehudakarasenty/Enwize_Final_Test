using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface IGraphWindowView
{
    GraphData GetGraphData();
    void LoadGraphData(GraphData graphData);
    void ConsturctGraph();
    void ClearGraph();
}

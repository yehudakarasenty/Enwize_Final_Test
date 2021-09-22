using UnityEngine;
using UnityEngine.Events;

public interface IGraphWindowView
{
    GraphData GetGraphData();
    void LoadGraphData(GraphData graphData);
    void ConsturctGraph();
    void ClearGraph();
    void RegisterToOnCreateNodeClickEvent(UnityAction<GraphNodeType> action);
    void CreateNode(GraphNodeType nodeType, Vector2 position);
}
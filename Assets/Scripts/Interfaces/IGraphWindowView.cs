using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGraphWindowView
{
    GraphData GetGraphData();
    void LoadGraphData(GraphData graphData);
    void ConsturctGraph();
    void ClearGraph();
    void RegisterToOnCreateNodeClickEvent(UnityAction<GraphNodeType> action);
    void RegisterToOnNodesSelectionChange(UnityAction action);
    void CreateNode(GraphNodeType nodeType, Vector2 position);
    List<GraphNodeData> GetNodesSelectionList();
}
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
    void CreateNode(GraphNodeType nodeType, Vector2 position, NodeAdditionalData extraData);
    List<GraphNodeData> GetNodesSelectionList();
    void InjectAdditionalDataToSelectionNodes(NodeAdditionalData nodeExtraData);
    void RegisterToOnDisable(UnityAction action);
}
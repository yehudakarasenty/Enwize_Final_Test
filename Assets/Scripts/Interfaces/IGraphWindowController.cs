using System.Collections.Generic;
using UnityEngine.Events;

public interface IGraphWindowController : IController
{
    void SetView(IGraphWindowView view);
    void LoadGraph(string fileName);
    void SaveGraph(string fileName);
    void RegisterToNodeSelectionsChange(UnityAction action);
    List<GraphNodeData> NodesSelections { get; }
}
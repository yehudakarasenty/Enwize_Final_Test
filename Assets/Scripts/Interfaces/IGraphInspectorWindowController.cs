using UnityEngine.Events;

public interface IGraphInspectorWindowController : IController
{
    void SetView(IGraphInspectorWindowView view);

    void SaveClicked(string fileName);

    void LoadClicked(string fileName);

    void RegisterToOnExtraDataFieldsChange(UnityAction action);

    NodeExtraData NodeExtraDataFields();
}
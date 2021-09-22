using UnityEngine.Events;

public interface IGraphInspectorWindowController : IController
{
    void SetView(IGraphInspectorWindowView view);

    void SaveClicked(string fileName);

    void LoadClicked(string fileName);

    void RegisterToOnAdditionalDataFieldsChange(UnityAction action);

    NodeAdditionalData NodeAdditionalDataFields();
}
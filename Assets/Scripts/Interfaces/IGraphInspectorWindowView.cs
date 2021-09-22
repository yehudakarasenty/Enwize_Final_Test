using UnityEngine.Events;

public interface IGraphInspectorWindowView
{
    void ConsturctView();
    void ShowAdditionalDataFields(NodeAdditionalData extraData);
    void HideAdditionalDataFields();
    void RegisterToOnAdditionalDataFieldsChange(UnityAction action);
    NodeAdditionalData GetNodeAdditionalDataFields();
}
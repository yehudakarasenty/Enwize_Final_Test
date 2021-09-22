using UnityEngine.Events;

public interface IGraphInspectorWindowView
{
    void ConsturctView();
    void UpdateView();
    void ShowExtraDataFields(NodeExtraData extraData);
    void HideExtraDataFields();
    void RegisterToOnExtraDataFieldsChange(UnityAction action);
}
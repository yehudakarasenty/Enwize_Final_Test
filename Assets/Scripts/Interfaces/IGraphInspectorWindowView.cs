using UnityEngine.Events;

public interface IGraphInspectorWindowView
{
    void ConsturctView();
    void ShowExtraDataFields(NodeExtraData extraData);
    void HideExtraDataFields();
    void RegisterToOnExtraDataFieldsChange(UnityAction action);
    NodeExtraData GetNodeExtraDataFields();
}
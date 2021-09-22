using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;

public class NodeView : Node
{
    public string GUID;
    public GraphNodeType Type;
    public NodeAdditionalData NodeAdditionalData;
    private UnityEvent OnSelecteChange = new UnityEvent();
    public NodeView(string gUID, GraphNodeType type, NodeAdditionalData nodeAdditionalData)
    {
        GUID = gUID;
        Type = type;
        NodeAdditionalData = nodeAdditionalData;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnSelecteChange.Invoke();
    }

    public override void OnUnselected()
    {
        base.OnUnselected();
        OnSelecteChange.Invoke();
    }

    public void RegisterToOnSelectChange(UnityAction action)
    {
        OnSelecteChange.AddListener(action);
    }
}

using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;

public class NodeView : Node
{
    #region Members
    public string GUID;

    public GraphNodeType Type;

    public NodeAdditionalData NodeAdditionalData;

    private UnityEvent onSelecteChange = new UnityEvent();
    #endregion

    #region Functions
    public NodeView(string gUID, GraphNodeType type, NodeAdditionalData nodeAdditionalData)
    {
        GUID = gUID;
        Type = type;
        NodeAdditionalData = nodeAdditionalData;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        onSelecteChange.Invoke();
    }

    public override void OnUnselected()
    {
        base.OnUnselected();
        onSelecteChange.Invoke();
    }

    public void RegisterToOnSelectChange(UnityAction action)
    {
        onSelecteChange.AddListener(action);
    }
    #endregion
}

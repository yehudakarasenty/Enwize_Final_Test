using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphInspectorView : VisualElement
{
    public string FileName { get; private set; } = "new narrative";

    private UnityEvent OnSaveClick = new UnityEvent();
    private UnityEvent OnLoadClick = new UnityEvent();

    public GraphInspectorView()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        Button saveButtons = new Button(()=> OnSaveClick.Invoke()) { text = "Save Graph" };
        Add(saveButtons);

        Button loadButtons = new Button(() => OnLoadClick.Invoke()) {text = "Load Graph" };
        Add(loadButtons);

        TextField fileNameTestField = new TextField("File Name");
        fileNameTestField.SetValueWithoutNotify(FileName);
        fileNameTestField.MarkDirtyRepaint();
        fileNameTestField.RegisterValueChangedCallback(evt => FileName = evt.newValue);

        Add(fileNameTestField);
    }

    public void RegisterToOnSaveClick(UnityAction action)
    {
        OnSaveClick.AddListener(action);
    }

    public void RemoveFromOnSaveClick(UnityAction action)
    {
        OnSaveClick.RemoveListener(action);
    }

    public void RegisterToOnLoadClick(UnityAction action)
    {
        OnLoadClick.AddListener(action);
    }

    public void RemoveFromOnLoadClick(UnityAction action)
    {
        OnLoadClick.RemoveListener(action);
    }
}

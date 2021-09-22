using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphInspectorView : VisualElement
{
    public string FileName { get; private set; } = "new narrative";

    private UnityEvent onSaveClick = new UnityEvent();
    private UnityEvent onLoadClick = new UnityEvent();
    private UnityEvent onExtraDataChange = new UnityEvent();

    TextField specialSecretTextField;
    TextField specialNumberTextField;

    public string SpecialSecretText { get => specialSecretTextField.value; }
    public string SpecialNumberText { get => specialNumberTextField.value; }

    public GraphInspectorView()
    {
        CreateElements();
    }

    private void CreateElements()
    {
        Button saveButtons = new Button(()=> onSaveClick.Invoke()) { text = "Save Graph" };
        Add(saveButtons);

        Button loadButtons = new Button(() => onLoadClick.Invoke()) {text = "Load Graph" };
        Add(loadButtons);

        TextField fileNameTestField = new TextField("File Name");
        fileNameTestField.SetValueWithoutNotify(FileName);
        fileNameTestField.MarkDirtyRepaint();//TO Understand
        fileNameTestField.RegisterValueChangedCallback(evt => FileName = evt.newValue);

        Add(fileNameTestField);

        specialNumberTextField = new TextField("My Special Number"); //TODO Only numbers input
        specialNumberTextField.MarkDirtyRepaint();//TO Understand

        specialSecretTextField = new TextField("My Special Secret");
        specialSecretTextField.MarkDirtyRepaint();//TO Understand

        specialNumberTextField.RegisterValueChangedCallback(evt => OnExtraDataChange());
        specialSecretTextField.RegisterValueChangedCallback(evt => OnExtraDataChange());

        Add(specialNumberTextField);
        Add(specialSecretTextField);
        HideExtraDataFileds();
    }

    private void OnExtraDataChange()
    {
        if (string.IsNullOrEmpty(specialNumberTextField.value))
            specialNumberTextField.SetValueWithoutNotify("0");
        else
            specialNumberTextField.SetValueWithoutNotify(Regex.Replace(specialNumberTextField.value, "[^0-9]", ""));
        onExtraDataChange.Invoke();
    }

    public void HideExtraDataFileds()
    {
        specialSecretTextField.visible = false;
        specialNumberTextField.visible = false;
    }

    public void ShowExtraDataFileds(NodeExtraData extraData)
    {
        specialNumberTextField.visible = true;
        specialSecretTextField.visible = true;

        specialNumberTextField.SetValueWithoutNotify(extraData.mySpecialNumber.ToString());
        specialSecretTextField.SetValueWithoutNotify(extraData.mySpecialSecret);
    }

    public void RegisterToOnSaveClick(UnityAction action)
    {
        onSaveClick.AddListener(action);
    }

    public void RemoveFromOnSaveClick(UnityAction action)
    {
        onSaveClick.RemoveListener(action);
    }

    public void RegisterToOnLoadClick(UnityAction action)
    {
        onLoadClick.AddListener(action);
    }

    public void RemoveFromOnLoadClick(UnityAction action)
    {
        onLoadClick.RemoveListener(action);
    }

    public void RegisterToOnExtraDataChange(UnityAction action)
    {
        onExtraDataChange.AddListener(action);
    }

    public void RemoveFromOnExtraDataChange(UnityAction action)
    {
        onExtraDataChange.RemoveListener(action);
    }
}

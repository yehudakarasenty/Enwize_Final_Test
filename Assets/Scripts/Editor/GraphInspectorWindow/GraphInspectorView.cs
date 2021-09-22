using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphInspectorView : VisualElement
{
    public string FileName { get; private set; } = "new narrative";

    private UnityEvent onSaveClick = new UnityEvent();
    private UnityEvent onLoadClick = new UnityEvent();
    private UnityEvent onAdditionalDataChange = new UnityEvent();

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
        fileNameTestField.MarkDirtyRepaint();//Triggers a repaint of the VisualElement on the next frame.
        fileNameTestField.RegisterValueChangedCallback(evt => FileName = evt.newValue);

        Add(fileNameTestField);

        specialNumberTextField = new TextField("My Special Number");
        specialNumberTextField.MarkDirtyRepaint();//Triggers a repaint of the VisualElement on the next frame.

        specialSecretTextField = new TextField("My Special Secret");
        specialSecretTextField.MarkDirtyRepaint();//Triggers a repaint of the VisualElement on the next frame.

        specialNumberTextField.RegisterValueChangedCallback(evt => OnAdditionalDataChange());
        specialSecretTextField.RegisterValueChangedCallback(evt => OnAdditionalDataChange());

        Add(specialNumberTextField);
        Add(specialSecretTextField);
        HideAdditionalDataFileds();
    }

    private void OnAdditionalDataChange()
    {
        if (string.IsNullOrEmpty(specialNumberTextField.value))
            specialNumberTextField.SetValueWithoutNotify("0");
        else
            specialNumberTextField.SetValueWithoutNotify(Regex.Replace(specialNumberTextField.value, "[^0-9]", ""));
        onAdditionalDataChange.Invoke();
    }

    public void HideAdditionalDataFileds()
    {
        specialSecretTextField.visible = false;
        specialNumberTextField.visible = false;
    }

    public void ShowAdditionalDataFileds(NodeAdditionalData additionalData)
    {
        specialNumberTextField.visible = true;
        specialSecretTextField.visible = true;

        specialNumberTextField.SetValueWithoutNotify(additionalData.mySpecialNumber.ToString());
        specialSecretTextField.SetValueWithoutNotify(additionalData.mySpecialSecret);
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

    public void RegisterToOnAdditionalDataChange(UnityAction action)
    {
        onAdditionalDataChange.AddListener(action);
    }

    public void RemoveFromOnAdditionalDataChange(UnityAction action)
    {
        onAdditionalDataChange.RemoveListener(action);
    }
}

using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphInspectorElementsView : VisualElement
{
    #region Members
    #region Events 
    private UnityEvent onSaveClick = new UnityEvent();
    private UnityEvent onLoadClick = new UnityEvent();
    private UnityEvent onAdditionalDataChange = new UnityEvent();
    #endregion

    #region UI
    TextField specialSecretTextField;
    TextField specialNumberTextField;
    #endregion

    #region Getters
    public string FileName { get; private set; } = "name";

    public string SpecialSecretText { get => specialSecretTextField.value; }

    public string SpecialNumberText { get => specialNumberTextField.value; }
    #endregion
    #endregion

    #region Functions
    #region Init
    public GraphInspectorElementsView()
    {
        CreateElements();
    }

    private void CreateElements()
    {
        //create file name text field
        TextField fileNameTestField = new TextField("File Name");
        fileNameTestField.SetValueWithoutNotify(FileName);
        fileNameTestField.MarkDirtyRepaint();
        fileNameTestField.RegisterValueChangedCallback(evt => FileName = evt.newValue);
        Add(fileNameTestField);

        //create save/load buttons
        Button saveButtons = new Button(() => onSaveClick.Invoke()) { text = "Save Graph" };
        Add(saveButtons);
        Button loadButtons = new Button(() => onLoadClick.Invoke()) { text = "Load Graph" };
        Add(loadButtons);

        //additional data
        Add(new Label(" "));
        Add(new Label(" "));
        Add(new Label("Additional Data:"));

        //special number text field
        specialNumberTextField = new TextField("My Special Number");
        specialNumberTextField.MarkDirtyRepaint();
        specialNumberTextField.RegisterValueChangedCallback(evt => OnAdditionalDataChange());
        Add(specialNumberTextField);

        //secret text field
        specialSecretTextField = new TextField("My Special Secret");
        specialSecretTextField.MarkDirtyRepaint();
        specialSecretTextField.RegisterValueChangedCallback(evt => OnAdditionalDataChange());
        Add(specialSecretTextField);

        HideAdditionalDataFileds();
    }

    #endregion

    #region Handle Events
    private void OnAdditionalDataChange()
    {
        if (string.IsNullOrEmpty(specialNumberTextField.value))
            specialNumberTextField.SetValueWithoutNotify("0");
        else
            specialNumberTextField.SetValueWithoutNotify(Regex.Replace(specialNumberTextField.value, "[^0-9]", ""));
        onAdditionalDataChange.Invoke();
    }

    #endregion

    #region Actions
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

    public void RegisterToOnSaveClick(UnityAction action) => onSaveClick.AddListener(action);

    public void RemoveFromOnSaveClick(UnityAction action) => onSaveClick.RemoveListener(action);

    public void RegisterToOnLoadClick(UnityAction action) => onLoadClick.AddListener(action);

    public void RemoveFromOnLoadClick(UnityAction action) => onLoadClick.RemoveListener(action);

    public void RegisterToOnAdditionalDataChange(UnityAction action) => onAdditionalDataChange.AddListener(action);

    public void RemoveFromOnAdditionalDataChange(UnityAction action) => onAdditionalDataChange.RemoveListener(action);
    #endregion
    #endregion
}

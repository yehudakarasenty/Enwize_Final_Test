using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphInspectorWindowView : EditorWindow, IGraphInspectorWindowView
{
    private IGraphInspectorWindowController mWindowController;

    private GraphInspectorView view;

    public void OnEnable()
    {
        mWindowController = SingleManager.Get<IGraphInspectorWindowController>();
        mWindowController.SetView(this);
    }

    public void ConsturctView()
    {
        view = new GraphInspectorView()
        {
            name = "AdditionalEditorWindow"
        };

        view.RegisterToOnLoadClick(LoadClicked);
        view.RegisterToOnSaveClick(SaveClicked);

        view.StretchToParentSize();
        rootVisualElement.Add(view);
    }

    public void ShowAdditionalDataFields(NodeAdditionalData additionalData)
    {
        view.ShowAdditionalDataFileds(additionalData);
    }

    public void HideAdditionalDataFields()
    {
        view.HideAdditionalDataFileds();
    }

    public void RegisterToOnAdditionalDataFieldsChange(UnityAction action)
    {
        view.RegisterToOnAdditionalDataChange(action);
    }

    private void SaveClicked()
    {
        if (string.IsNullOrEmpty(view.FileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name", "ok");
            return;
        }
        mWindowController.SaveClicked(view.FileName);
    }

    private void LoadClicked()
    {
        if (string.IsNullOrEmpty(view.FileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name", "ok");
            return;
        }
        mWindowController.LoadClicked(view.FileName);
    }

    public NodeAdditionalData GetNodeAdditionalDataFields()
    {
        return new NodeAdditionalData(int.Parse(view.SpecialNumberText), view.SpecialSecretText);
    }

    public void OnDisable()
    {
        view.RemoveFromOnLoadClick(LoadClicked);
        view.RemoveFromOnSaveClick(SaveClicked);
        rootVisualElement.Remove(view);
    }
}

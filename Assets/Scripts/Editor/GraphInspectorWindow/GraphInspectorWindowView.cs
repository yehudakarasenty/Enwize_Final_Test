using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GraphInspectorWindowView : EditorWindow, IGraphInspectorWindowView
{
    private IGraphInspectorWindowController mWindowController;

    private GraphInspectorView view;

    private UnityEvent onExtraDataChange = new UnityEvent();

    public void OnEnable()
    {
        mWindowController = SingleManager.Get<IGraphInspectorWindowController>();
        mWindowController.SetView(this);
    }

    public void ConsturctView()
    {
        view = new GraphInspectorView()
        {
            name = "ExtraEditorWindow"
        };

        view.RegisterToOnLoadClick(LoadClicked);
        view.RegisterToOnSaveClick(SaveClicked);

        view.StretchToParentSize();
        rootVisualElement.Add(view);
    }
    
    public void ShowExtraDataFields(NodeExtraData extraData)
    {
        view.ShowExtraDataFileds(extraData);
    }

    public void HideExtraDataFields()
    {
        view.HideExtraDataFileds();
    }

    public void RegisterToOnExtraDataFieldsChange(UnityAction action)
    {
        onExtraDataChange.AddListener(action);
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

    public void UpdateView()
    {
        //TODO
    }

    public void OnDisable()
    {
        view.RemoveFromOnLoadClick(LoadClicked);
        view.RemoveFromOnSaveClick(SaveClicked);
        rootVisualElement.Remove(view);
    }
}

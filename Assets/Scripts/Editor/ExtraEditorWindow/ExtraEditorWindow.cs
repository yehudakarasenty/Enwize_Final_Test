using UnityEditor;
using UnityEngine.UIElements;

public class ExtraEditorWindow : EditorWindow, IExtraEditorWindow
{
    private IGraphWindow mGraphWindow;

    private ExtraEditorWindowView view;

    private void Awake()
    {
        SingleManager.Register<IExtraEditorWindow>(this);
    }

    private void OnEnable()
    {
        mGraphWindow = SingleManager.Get<IGraphWindow>();
        ConsturctView();
    }

    private void ConsturctView()
    {
        view = new ExtraEditorWindowView(this)
        {
            name = "ExtraEditorWindow"
        };

        view.StretchToParentSize();
        rootVisualElement.Add(view);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(view);
    }

    public void SaveOrLoadClicked(string fileName, bool save)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name", "ok");
            return;
        }
        if(save) 
            mGraphWindow.SaveGraph(fileName);
        else 
            mGraphWindow.LoadGraph(fileName);
    }

    private void OnDestroy()
    {
        SingleManager.Remove<IExtraEditorWindow>();
    }
}

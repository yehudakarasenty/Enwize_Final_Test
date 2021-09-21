using UnityEngine.UIElements;

public class GraphInspectorView : VisualElement
{
    private IExtraEditorWindow mWindow;

    private string fileName = "new narrative";

    public GraphInspectorView(IExtraEditorWindow window)
    {
        mWindow = window;
        CreateButtons();
    }

    private void CreateButtons()
    {
        Button saveButtons = new Button(()=> SaveOrLoadClicked(true)) { text = "Save Graph" };
        Add(saveButtons);

        Button loadButtons = new Button(() => SaveOrLoadClicked(false)) {text = "Load Graph" };
        Add(loadButtons);

        TextField fileNameTestField = new TextField("File Name");
        fileNameTestField.SetValueWithoutNotify(fileName);
        fileNameTestField.MarkDirtyRepaint();
        fileNameTestField.RegisterValueChangedCallback(evt => fileName = evt.newValue);

        Add(fileNameTestField);
    }

    private void SaveOrLoadClicked(bool save)
    {
        mWindow.SaveOrLoadClicked(fileName, save);
    }
}

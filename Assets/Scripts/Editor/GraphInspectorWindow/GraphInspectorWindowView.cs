﻿using UnityEditor;
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
            name = "ExtraEditorWindow"
        };

        view.RegisterToOnLoadClick(LoadClicked);
        view.RegisterToOnSaveClick(SaveClicked);

        view.StretchToParentSize();
        rootVisualElement.Add(view);
    }

    public void SaveOrLoadClicked(string fileName, bool save)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name", "ok");
            return;
        }
        if (save)
            mWindowController.SaveClicked(fileName);
        else
            mWindowController.LoadClicked(fileName);
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

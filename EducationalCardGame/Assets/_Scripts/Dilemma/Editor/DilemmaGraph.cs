using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DilemmaGraph : EditorWindow
{
    private DilemmaGraphView _graphView;
    private string _fileName = "New Narrative";

    [MenuItem("Graph/Dilemma Graph")]
    public static void OpenDilemmaGraphWindow()
    {
        var window = GetWindow<DilemmaGraph>();
        window.titleContent = new GUIContent(text:"Dilemma Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMiniMap();
    }

    private void ConstructGraphView()
    {
        _graphView = new DilemmaGraphView
        {
            name = "Dilemma Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField(label: "File Name");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterCallback((EventCallback<ChangeEvent<string>>)(evt => _fileName = evt.newValue));
        toolbar.Add(fileNameTextField);

        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(save:true)) { text = "Save Data" });
        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(save:false)) { text = "Load Data" });

        var nodeCreateButton = new Button(clickEvent: () => { _graphView.CreateNode("Dilemma Node"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap{anchored = true};
        miniMap.SetPosition(newPos: new Rect(x: 10, y: 30, width: 200, height: 140));
        _graphView.Add(miniMap);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog(title: "Invalid file name!", message: "Please enter a valid file name.", ok: "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_fileName);
        }
        else
        {
            saveUtility.LoadGraph(_fileName);
        }
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
}

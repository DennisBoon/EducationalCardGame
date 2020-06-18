using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GenerateBlackBoard();
    }

    private void GenerateBlackBoard()
    {
        var blackboard = new Blackboard(_graphView);
        blackboard.Add(child: new BlackboardSection { title = "Exposed Properties" });
        blackboard.addItemRequested = _blackboard => { _graphView.AddPropertyToBlackBoard(new ExposedProperty()); };
        blackboard.editTextRequested = (blackboard1, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposedProperties.Any(XboxBuildSubtarget => XboxBuildSubtarget.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog(title: "Error", message: "This property name already exists, please choose another one.",
                    ok: "OK");
                return;
            }

            var propertyIndex = _graphView.ExposedProperties.FindIndex(match: x => x.PropertyName == oldPropertyName);
            _graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
            ((BlackboardField)element).text = newValue;
        };
        blackboard.SetPosition(newPos: new Rect(x: 10, y: 30, width: 200, height: 300));
        _graphView.Add(blackboard);
        _graphView.Blackboard = blackboard;
    }

    private void ConstructGraphView()
    {
        _graphView = new DilemmaGraphView(this)
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

        rootVisualElement.Add(toolbar);
    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap{anchored = true};
        var cords = _graphView.contentViewContainer.WorldToLocal(p: new Vector2(x: this.maxSize.x - 10, y: 30));
        miniMap.SetPosition(newPos: new Rect(cords.x, cords.y, width: 300, height: 240));
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

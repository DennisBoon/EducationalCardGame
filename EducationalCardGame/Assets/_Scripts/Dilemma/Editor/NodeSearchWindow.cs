using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DilemmaGraphView _graphView;
    private EditorWindow _window;
    private Texture2D _indentationIcon;

    public void Init(EditorWindow window, DilemmaGraphView graphView)
    {
        _graphView = graphView;
        _window = window;

        _indentationIcon = new Texture2D(width: 1, height: 1);
        _indentationIcon.SetPixel(x: 0, y: 0, new Color(r: 0, g: 0, b: 0, a: 0));
        _indentationIcon.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent(text: "Create Element"), level: 0),
            new SearchTreeGroupEntry(new GUIContent(text: "Dilemma"), level: 1),
            new SearchTreeEntry(new GUIContent(text: "Dilemma Node", _indentationIcon))
            {
                userData = new DilemmaNode(),level = 2
            },
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
            point: context.screenMousePosition - _window.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

        switch (SearchTreeEntry.userData)
        {
            case DilemmaNode dilemmaNode:
                _graphView.CreateNode("Dilemma Node", localMousePosition);
                return true;
            default:
                return false;
        }
    }
}

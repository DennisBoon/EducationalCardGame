using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;
using UnityEditor;

public class DilemmaGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(x:150, y:200);

    public Blackboard Blackboard;
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
    private NodeSearchWindow _searchWindow;

    public DilemmaGraphView(EditorWindow editorWindow)
    {
        styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "DilemmaGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(index: 0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(editorWindow, this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(funcCall: (port) =>
        {
            if (startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private Port GeneratePort(DilemmaNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); // Arbitrary type
    }

    private DilemmaNode GenerateEntryPointNode()
    {
        var node = new DilemmaNode
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DilemmaText = "ENTRYPOINT",
            EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }

    public void CreateNode(string nodeName, Vector2 position)
    {
        AddElement(CreateDilemmaNode(nodeName, position));
    }

    public DilemmaNode CreateDilemmaNode(string nodeName, Vector2 position)
    {
        var dilemmaNode = new DilemmaNode
        {
            title = nodeName,
            DilemmaText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(dilemmaNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dilemmaNode.inputContainer.Add(inputPort);

        dilemmaNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "Node"));

        var button = new Button(clickEvent: () => { AddChoicePort(dilemmaNode); });
        button.text = "New Choice";
        dilemmaNode.titleContainer.Add(button);

        var textField = new TextField(label: string.Empty);
        textField.RegisterCallback((EventCallback<ChangeEvent<string>>)(evt => 
        {
            dilemmaNode.DilemmaText = evt.newValue;
            dilemmaNode.title = evt.newValue;
        }));
        textField.SetValueWithoutNotify(dilemmaNode.title);
        dilemmaNode.mainContainer.Add(textField);

        dilemmaNode.RefreshExpandedState();
        dilemmaNode.RefreshPorts();
        dilemmaNode.SetPosition(new Rect(position: position, defaultNodeSize));

        return dilemmaNode;
    }

    public void AddChoicePort(DilemmaNode dilemmaNode, string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(dilemmaNode, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>(name: "type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = dilemmaNode.outputContainer.Query(name: "connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {outputPortCount + 1}"
            : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterCallback((EventCallback<ChangeEvent<string>>)(evt => generatedPort.portName = evt.newValue));
        generatedPort.contentContainer.Add(child: new Label(text: "  "));
        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(clickEvent: () => RemovePort(dilemmaNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;
        dilemmaNode.outputContainer.Add(generatedPort);
        dilemmaNode.RefreshPorts();
        dilemmaNode.RefreshExpandedState();
    }

    private void RemovePort(DilemmaNode dilemmaNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any()) return;
        var edge = targetEdge.First();
        edge.input.Disconnect(edge);
        RemoveElement(targetEdge.First());

        dilemmaNode.outputContainer.Remove(generatedPort);
        dilemmaNode.RefreshPorts();
        dilemmaNode.RefreshExpandedState();
    }

    public void ClearBlackBoardAndExposedProperties()
    {
        ExposedProperties.Clear();
        Blackboard.Clear();
    }

    public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
    {
        var localPropertyName = exposedProperty.PropertyName;
        var localPropertyValue = exposedProperty.PropertyValue;
        while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
            localPropertyName = $"{localPropertyName}(1)";

        var property = new ExposedProperty();
        property.PropertyName = localPropertyName;
        property.PropertyValue = localPropertyValue;
        ExposedProperties.Add(property);

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string property" };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField(label: "Value:")
        {
            value = localPropertyValue
        };
        propertyValueTextField.RegisterCallback((EventCallback<ChangeEvent<string>>)(evt =>
        {
            var changingPropertyIndex = ExposedProperties.FindIndex(match: x => x.PropertyName == property.PropertyName);
            ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
        }));
        var blackBoardValueRow = new BlackboardRow(item: blackboardField, propertyView: propertyValueTextField);
        container.Add(blackBoardValueRow);

        Blackboard.Add(container);
    }
}

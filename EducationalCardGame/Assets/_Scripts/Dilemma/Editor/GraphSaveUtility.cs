using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DilemmaGraphView _targetGraphView;
    private DilemmaContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DilemmaNode> Nodes => _targetGraphView.nodes.ToList().Cast<DilemmaNode>().ToList();

    public static GraphSaveUtility GetInstance(DilemmaGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        var dilemmaContainer = ScriptableObject.CreateInstance<DilemmaContainer>();
        if (!SaveNodes(dilemmaContainer)) return;
        SaveExposedProperties(dilemmaContainer);
        // Auto creates resources / graphs folder if it doesn't exist
        if (!AssetDatabase.IsValidFolder(path: "Assets/Resources"))
            AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");
        if (!AssetDatabase.IsValidFolder(path: "Assets/Resources/Graphs"))
            AssetDatabase.CreateFolder(parentFolder: "Assets/Resources", newFolderName: "Graphs");

        AssetDatabase.CreateAsset(dilemmaContainer, path: $"Assets/Resources/Graphs/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    private bool SaveNodes(DilemmaContainer dilemmaContainer)
    {
        if (!Edges.Any()) return false; // If there are no edges (no connection) then return

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DilemmaNode;
            var inputNode = connectedPorts[i].input.node as DilemmaNode;

            Debug.Log(((DilemmaNode)connectedPorts[i].output.node).GUID);
            Debug.Log(connectedPorts[i].output.contentContainer.childCount);

            string ResourceUpName = "";
            string ResourceDownName = "";

            foreach(VisualElement visualElement in connectedPorts[i].output.contentContainer.Children().Where(x => x.name == "choiceResourceUpName"))
            {
                Debug.Log(visualElement.name);
                Debug.Log(((TextField)visualElement).value);
                ResourceUpName = ((TextField)visualElement).value;
            }
    
            foreach(VisualElement visualElement in connectedPorts[i].output.contentContainer.Children().Where(x => x.name == "choiceResourceDownName"))
            {
                Debug.Log(visualElement.name);
                Debug.Log(((TextField) visualElement).value);
                ResourceDownName = ((TextField) visualElement).value;
            }

            dilemmaContainer.NodeLinks.Add(item: new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                CardImageName = connectedPorts[i].output.name,
                ResourceUpName = ResourceUpName,
                ResourceDownName = ResourceDownName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        foreach (var dilemmaNode in Nodes.Where(node => !node.EntryPoint))
        {
            dilemmaContainer.DilemmaNodeData.Add(item: new DilemmaNodeData
            {
                Guid = dilemmaNode.GUID,
                DilemmaText = dilemmaNode.DilemmaText,
                Position = dilemmaNode.GetPosition().position
            });
        }

        return true;
    }

    private void SaveExposedProperties(DilemmaContainer dilemmaContainer)
    {
        dilemmaContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = Resources.Load<DilemmaContainer>("Graphs/" + fileName);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog(title: "File Not Found", message: "Target dilemma graph file does not exist!", ok: "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
        CreateExposedProperties();
    }

    private void CreateExposedProperties()
    {
        // Clear existing properties on hot-reload
        _targetGraphView.ClearBlackBoardAndExposedProperties();
        // Add properties from data.
        foreach (var exposedProperty in _containerCache.ExposedProperties)
        {
            _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }
    }

    private void ConnectNodes()
    {
        for (var i = 0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(output: Nodes[i].outputContainer[j].Q<Port>(), input: (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(newPos: new Rect(
                    _containerCache.DilemmaNodeData.First(x=>x.Guid==targetNodeGuid).Position,
                    _targetGraphView.defaultNodeSize
                    ));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.DilemmaNodeData)
        {
            var tempNode = _targetGraphView.CreateDilemmaNode(nodeData.DilemmaText, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName, x.CardImageName, x.ResourceUpName, x.ResourceDownName));
        }
    }

    private void ClearGraph()
    {
        // Set entry points guid back from the save. Discard existing guid.
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;

            // Remove edges that connected to this node
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            // Then remove the node
            _targetGraphView.RemoveElement(node);
        }
    }
}

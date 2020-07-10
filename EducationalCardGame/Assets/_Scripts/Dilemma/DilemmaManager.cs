using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DilemmaManager : MonoBehaviour
{
    public Text dilemmaText, controlsText, resourcesText, scenarioText,
        roleDescriptionText, controlsTextGap, resourcesTextGap,
        scenarioTextGap, roleDescriptionTextGap;

    public string controls, scenario;

    private string currentNode;

    public DilemmaContainer dilemmaContainer;

    private void Awake()
    {
        currentNode = dilemmaContainer.DilemmaNodeData[0].Guid;
        Debug.Log("STARTING NODE: " + currentNode);
        int selectedNodeIndex = dilemmaContainer.DilemmaNodeData.IndexOf(new DilemmaNodeData { Guid = currentNode }) + 1;
        Debug.Log("INDEX OF CURRENTLY SELECTED NODE: " + selectedNodeIndex);
        UpdateDilemmaText(dilemmaContainer.DilemmaNodeData[selectedNodeIndex].DilemmaText);
        UpdateResourcesText();
    }

    public void UpdateDilemmaText(string dilemma)
    {
        dilemmaText.text = dilemma;
    }

    public void UpdateResourcesText()
    {
        resourcesText.text = dilemmaContainer.ExposedProperties[0].PropertyName + ": " + dilemmaContainer.ExposedProperties[0].PropertyValue + "\t" +
        dilemmaContainer.ExposedProperties[1].PropertyName + ": " + dilemmaContainer.ExposedProperties[1].PropertyValue + "\n" +
        dilemmaContainer.ExposedProperties[2].PropertyName + ": " + dilemmaContainer.ExposedProperties[2].PropertyValue + "\t" +
        dilemmaContainer.ExposedProperties[3].PropertyName + ": " + dilemmaContainer.ExposedProperties[3].PropertyValue;
    }

    public void MoveNode(string targetNodeGuid)
    {

    }

    public void SelectOption()
    {

    }
}

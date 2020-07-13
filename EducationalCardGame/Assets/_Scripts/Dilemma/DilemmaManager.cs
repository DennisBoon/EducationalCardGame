using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DilemmaManager : MonoBehaviour
{
    public Text dilemmaText, controlsText, resourcesText, scenarioText,
        roleDescriptionText, controlsTextGap, resourcesTextGap,
        scenarioTextGap, roleDescriptionTextGap, gameCardOneText,
        gameCardTwoText;

    public string controls, scenario;

    private string currentNode;

    public GameSetupController gameSetupController;
    public DilemmaContainer dilemmaContainer;

    private void Awake()
    {
        currentNode = dilemmaContainer.DilemmaNodeData[0].Guid;

        controlsText.text = controls;
        controlsTextGap.text = controls;

        scenarioText.text = scenario;
        scenarioTextGap.text = scenario;
    }

    public void UpdateDilemma()
    {
        int selectedNodeIndex = dilemmaContainer.DilemmaNodeData.IndexOf(new DilemmaNodeData { Guid = currentNode }) + 1;
        dilemmaText.text = dilemmaContainer.DilemmaNodeData[selectedNodeIndex].DilemmaText;
        UpdateCards();
        UpdateResourcesText();
    }

    public void UpdateResourcesText()
    {
        resourcesText.text = dilemmaContainer.ExposedProperties[0].PropertyName + ": " + dilemmaContainer.ExposedProperties[0].PropertyValue + "\t" +
        dilemmaContainer.ExposedProperties[1].PropertyName + ": " + dilemmaContainer.ExposedProperties[1].PropertyValue + "\n" +
        dilemmaContainer.ExposedProperties[2].PropertyName + ": " + dilemmaContainer.ExposedProperties[2].PropertyValue + "\t" +
        dilemmaContainer.ExposedProperties[3].PropertyName + ": " + dilemmaContainer.ExposedProperties[3].PropertyValue;
        resourcesTextGap.text = resourcesText.text;
    }

    public void UpdateCards()
    {
        gameSetupController.gameCardOne.SetActive(true);
        gameSetupController.gameCardTwo.SetActive(true);
        int firstOptionIndex = dilemmaContainer.NodeLinks.IndexOf(new NodeLinkData { BaseNodeGuid = currentNode }) + 2;
        int secondOptionIndex = dilemmaContainer.NodeLinks.IndexOf(new NodeLinkData { BaseNodeGuid = currentNode }) + 3;
        gameCardOneText.text = dilemmaContainer.NodeLinks[firstOptionIndex].PortName;
        gameCardTwoText.text = dilemmaContainer.NodeLinks[secondOptionIndex].PortName;
    }

    public void MoveNode(string targetNodeGuid)
    {

    }

    public void SelectOption()
    {

    }
}

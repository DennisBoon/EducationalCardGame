using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using UnityEditor;

public class DilemmaManager : MonoBehaviour
{
    public Text dilemmaText, controlsText, resourcesText, scenarioText,
        roleDescriptionText;

    public string controls, scenario;

    private string currentNode, endingText, currentDilemmaText;

    public GameSetupController gameSetup;
    public DilemmaContainer dilemmaContainer;
    public TurnManager turnManager;

    public List<GameObject> gameCardImages = new List<GameObject>();
    public List<Sprite> cardImages = new List<Sprite>();

    [SerializeField]
    private int endingSceneIndex;

    private void Awake()
    {
        string startTargetNode = dilemmaContainer.NodeLinks[0].TargetNodeGuid;
        currentNode = startTargetNode;

        controlsText.text = controls;

        scenarioText.text = scenario;

        //UpdateDilemma();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        string guid = dilemmaContainer.NodeLinks.FindAll(x => x.BaseNodeGuid == currentNode)[0].TargetNodeGuid;

    //        currentNode = guid;
    //        UpdateDilemma();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        string guid = dilemmaContainer.NodeLinks.FindAll(x => x.BaseNodeGuid == currentNode)[1].TargetNodeGuid;

    //        currentNode = guid;
    //        UpdateDilemma();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        string guid = dilemmaContainer.NodeLinks.FindAll(x => x.BaseNodeGuid == currentNode)[2].TargetNodeGuid;

    //        currentNode = guid;
    //        UpdateDilemma();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        string guid = dilemmaContainer.NodeLinks.FindAll(x => x.BaseNodeGuid == currentNode)[3].TargetNodeGuid;

    //        currentNode = guid;
    //        UpdateDilemma();
    //    }
    //}

    public void UpdateDilemma()
    {
        int selectedNodeIndex = dilemmaContainer.DilemmaNodeData.FindIndex(x => x.Guid == currentNode);
        Debug.Log("SELECTED NODE INDEX: " + selectedNodeIndex);
        currentDilemmaText = dilemmaContainer.DilemmaNodeData[selectedNodeIndex].DilemmaText;

        if (currentDilemmaText.Contains("ENDING:"))
        {
            Debug.Log("UPDATING ENDING");
            endingText = currentDilemmaText;
            endingText.Replace("ENDING: ", string.Empty);
            UpdateResourcesText();
            MPGameManager.Instance.EndGame(endingText);
        }
        else
        {
            Debug.Log("UPDATING DILEMMA");
            dilemmaText.text = currentDilemmaText;
            UpdateCards();
            UpdateResourcesText();
        }
    }

    public void UpdateResourcesText()
    {
        Debug.Log("UPDATING RESOURCES TEXT");
        resourcesText.text = dilemmaContainer.ExposedProperties[0].PropertyName + ": " + dilemmaContainer.ExposedProperties[0].PropertyValue + "\n" +
        dilemmaContainer.ExposedProperties[1].PropertyName + ": " + dilemmaContainer.ExposedProperties[1].PropertyValue + "\n" +
        dilemmaContainer.ExposedProperties[2].PropertyName + ": " + dilemmaContainer.ExposedProperties[2].PropertyValue + "\n" +
        dilemmaContainer.ExposedProperties[3].PropertyName + ": " + dilemmaContainer.ExposedProperties[3].PropertyValue;
    }

    public void UpdateCards()
    {
        Debug.Log("UPDATING CARDS");

        List<NodeLinkData> nodeLinks = dilemmaContainer.NodeLinks.FindAll(x => x.BaseNodeGuid == currentNode);
        Debug.Log("OPTIONS AVAILABLE FOR THIS DILEMMA: " + nodeLinks.Count);

        for (int i = 0; i < nodeLinks.Count; i++)
        {
            Debug.Log("OPTION FROM LIST: " + nodeLinks[i] + " " + i);
            gameSetup.gameCards[i].SetActive(true);
            Debug.Log(nodeLinks[i] + " " + i + " ACTIVE: " + gameSetup.gameCards[i].activeSelf);
            Debug.Log("POSITION: " + gameSetup.gameCards[i].transform.position);
            MPGameManager.Instance.gameCardDescriptions[i].GetComponent<Text>().text = nodeLinks[i].PortName;
            string imageName = nodeLinks[i].CardImageName;
            Sprite cardSprite = cardImages.Find((x) => x.name == imageName);
            gameCardImages[i].GetComponent<Image>().sprite = cardSprite;
        }

        nodeLinks.Clear();
    }

    public void MoveNode(string targetNodeGuid)
    {
        Debug.Log("MOVING NODE");
        currentNode = targetNodeGuid;
        turnManager.NextTurn();
    }
}

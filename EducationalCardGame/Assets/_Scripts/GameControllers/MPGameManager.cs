using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;

public class MPGameManager : MonoBehaviour
{
    private static MPGameManager _instance;
    public static MPGameManager Instance { get { return _instance; } }

    [HideInInspector]
    public int playersReady = 0;
    [HideInInspector]
    public List<int> optionVotes = new List<int>();
    public List<GameObject> gameCardDescriptions = new List<GameObject>();

    public GameObject infoBoards;
    public GameObject turnManager;
    public GameObject roleManagerCanvas;

    public CheckCard check;
    public GameSetupController gameSetup;
    public DilemmaManager dilemma;

    private string cardText;

    public Text endText;
    public Text endResourcesText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        for (int i = 0; i < 4; i++)
        {
            optionVotes.Add(new int());
            Debug.Log(optionVotes[i]);
        }
    }

    public void CheckIfReady()
    {
        playersReady++;

        if (PhotonNetwork.CurrentRoom.PlayerCount == playersReady)
        {
            this.GetComponent<PhotonView>().RPC("RPC_StartGame", RpcTarget.AllBuffered);
        }
        else
        {
            Debug.Log("Waiting on other players...");
        }
    }

    public void CheckIfVoted()
    {
        playersReady++;

        if (PhotonNetwork.CurrentRoom.PlayerCount == playersReady)
        {
            this.GetComponent<PhotonView>().RPC("RPC_FinishTurn", RpcTarget.AllBuffered);
        }
        else
        {
            Debug.Log("Waiting for other players to vote...");
        }
    }

    public string GetCardDescription()
    {
        var max = optionVotes[0];
        var index = 0;

        for (int i = 1; i < optionVotes.Count; i++)
        {
            if (optionVotes[i] > max)
            {
                max = optionVotes[i];
                index = i;
            }
        }

        cardText = gameCardDescriptions[index].GetComponent<Text>().text;
        Debug.Log("CARD TEXT: " + cardText);
        return cardText;
    }

    public void EndGame(string endingText)
    {
        gameSetup.infoBoardWithText.SetActive(false);
        gameSetup.endingCanvas.SetActive(true);
        Debug.Log("UPDATING RESOURCES TEXT");
        endResourcesText.text = dilemma.dilemmaContainer.ExposedProperties[0].PropertyName + ": " + dilemma.dilemmaContainer.ExposedProperties[0].PropertyValue + "\n" +
        dilemma.dilemmaContainer.ExposedProperties[1].PropertyName + ": " + dilemma.dilemmaContainer.ExposedProperties[1].PropertyValue + "\n" +
        dilemma.dilemmaContainer.ExposedProperties[2].PropertyName + ": " + dilemma.dilemmaContainer.ExposedProperties[2].PropertyValue + "\n" +
        dilemma.dilemmaContainer.ExposedProperties[3].PropertyName + ": " + dilemma.dilemmaContainer.ExposedProperties[3].PropertyValue;
        endText.text = endingText;
    }

    [PunRPC]
    public void RPC_StartGame()
    {
        Debug.Log("Start the game!");
        for (int i = 0; i < 4; i++)
        {
            infoBoards.transform.GetChild(i).gameObject.SetActive(true);
        }
        roleManagerCanvas.SetActive(false);
        turnManager.SetActive(true);
        playersReady = 0;
    }

    [PunRPC]
    public void RPC_FinishTurn()
    {
        Debug.Log("FINISH TURN");
        int votes = optionVotes[0];
        bool tie = true;

        for (int i = 1; i < optionVotes.Count; i++)
        {
            if (optionVotes[i] != votes)
            {
                tie = false;
            }
        }

        if (tie)
        {
            Debug.Log("VOTES ARE TIED, RESTART VOTE");
            check.ResetCards();
        }
        else
        {
            Debug.Log("VOTING ENDED, CONTINUING TO NEXT DILEMMA");
            cardText = GetCardDescription();
            int selectCardIndex = dilemma.dilemmaContainer.NodeLinks.FindIndex(x => x.PortName == cardText);
            Debug.Log("ADJUST RESOURCES BASED ON CHOSEN CARD");
            string resourceUp = dilemma.dilemmaContainer.NodeLinks[selectCardIndex].ResourceUpName;
            string resourceDown = dilemma.dilemmaContainer.NodeLinks[selectCardIndex].ResourceDownName;

            if (resourceUp != string.Empty)
            {
                for (int i = 0; i < dilemma.dilemmaContainer.ExposedProperties.Count; i++)
                {
                    if (dilemma.dilemmaContainer.ExposedProperties[i].PropertyName == resourceUp)
                    {
                        dilemma.dilemmaContainer.ExposedProperties[i].PropertyValue++;
                    }
                }
            }

            if (resourceDown != string.Empty)
            {
                for (int i = 0; i < dilemma.dilemmaContainer.ExposedProperties.Count; i++)
                {
                    if (dilemma.dilemmaContainer.ExposedProperties[i].PropertyName == resourceDown)
                    {
                        dilemma.dilemmaContainer.ExposedProperties[i].PropertyValue--;
                    }
                }
            }

            Debug.Log("CARD INDEX: " + selectCardIndex);
            string targetNodeGuid = dilemma.dilemmaContainer.NodeLinks[selectCardIndex].TargetNodeGuid;
            Debug.Log("TARGET NODE GUID: " + targetNodeGuid);
            dilemma.MoveNode(targetNodeGuid);
            check.votingBoard.SetActive(true);
            check.waitingBoard.SetActive(false);
        }

        check.placedCard = null;

        for (int i = 0; i < optionVotes.Count; i++)
        {
            optionVotes[i] = 0;
        }

        playersReady = 0;
    }
}

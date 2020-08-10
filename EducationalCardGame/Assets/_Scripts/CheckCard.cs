using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CheckCard : MonoBehaviour
{
    [HideInInspector]
    public GameObject placedCard;

    public GameSetupController gameSetup;
    public GameObject leftHand;
    public GameObject rightHand;
    public Button confirmButton;
    public GameObject votingBoard;
    public GameObject waitingBoard;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GameCardOne" && placedCard == null ||
            other.gameObject.name == "GameCardTwo" && placedCard == null ||
            other.gameObject.name == "GameCardThree" && placedCard == null ||
            other.gameObject.name == "GameCardFour" && placedCard == null)
        {
            leftHand.GetComponent<OVRGrabber>().GrabEnd();
            rightHand.GetComponent<OVRGrabber>().GrabEnd();
            placedCard = other.gameObject;
            placedCard.GetComponent<BoxCollider>().enabled = false;
            placedCard.GetComponent<Rigidbody>().isKinematic = true;
            placedCard.GetComponent<OVRGrabbable>().resetTransform = false;
            placedCard.transform.position = transform.GetChild(0).position;
            placedCard.transform.rotation = transform.GetChild(0).rotation;
            confirmButton.interactable = true;
        }
        else if (other.gameObject.name == "GameCardOne" && placedCard != null ||
            other.gameObject.name == "GameCardTwo" && placedCard != null ||
            other.gameObject.name == "GameCardThree" && placedCard != null ||
            other.gameObject.name == "GameCardFour" && placedCard != null)
        {
            leftHand.GetComponent<OVRGrabber>().GrabEnd();
            rightHand.GetComponent<OVRGrabber>().GrabEnd();
            placedCard.GetComponent<OVRGrabbable>().resetTransform = true;
            placedCard.GetComponent<OVRGrabbable>().ResetTransform();
            placedCard.GetComponent<BoxCollider>().enabled = true;
            placedCard.GetComponent<Rigidbody>().isKinematic = false;
            placedCard = other.gameObject;
            placedCard.GetComponent<BoxCollider>().enabled = false;
            placedCard.GetComponent<Rigidbody>().isKinematic = true;
            placedCard.GetComponent<OVRGrabbable>().resetTransform = false;
            placedCard.transform.position = transform.GetChild(0).position;
            placedCard.transform.rotation = transform.GetChild(0).rotation;
            confirmButton.interactable = true;
        }
    }

    // Call this method when there is a tie in the votes
    public void ResetCards()
    {
        confirmButton.interactable = false;
        if (placedCard != null)
        {
            placedCard.GetComponent<OVRGrabbable>().resetTransform = true;
            placedCard.GetComponent<Rigidbody>().isKinematic = false;
            placedCard = null;
        }
        foreach(GameObject card in gameSetup.gameCards)
        {
            if (card.activeSelf)
            {
                card.GetComponent<OVRGrabbable>().ResetTransform();
                card.GetComponent<BoxCollider>().enabled = true;
            }
        }
        votingBoard.SetActive(true);
        waitingBoard.SetActive(false);
    }

    public void ConfirmCard()
    {
        foreach (GameObject card in gameSetup.gameCards)
        {
            if (card.activeSelf)
            {
                card.SetActive(false);
                card.GetComponent<OVRGrabbable>().ResetTransform();
            }
        }
        placedCard.GetComponent<OVRGrabbable>().resetTransform = true;
        placedCard.GetComponent<BoxCollider>().enabled = true;
        placedCard.GetComponent<Rigidbody>().isKinematic = false;
        votingBoard.SetActive(false);
        waitingBoard.SetActive(true);
        confirmButton.interactable = false;
        this.GetComponent<PhotonView>().RPC("RPC_ConfirmCard", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_ConfirmCard()
    {
        Debug.Log("PLACED CARD: " + placedCard.name);
        if (placedCard.name == "GameCardOne")
        {
            Debug.Log("VOTE FOR OPTION ONE");
            MPGameManager.Instance.optionVotes[0]++;
        }
        else if (placedCard.name == "GameCardTwo")
        {
            Debug.Log("VOTE FOR OPTION TWO");
            MPGameManager.Instance.optionVotes[1]++;
        }
        else if (placedCard.name == "GameCardThree")
        {
            Debug.Log("VOTE FOR OPTION THREE");
            MPGameManager.Instance.optionVotes[2]++;
        }
        else if (placedCard.name == "GameCardFour")
        {
            Debug.Log("VOTE FOR OPTION FOUR");
            MPGameManager.Instance.optionVotes[3]++;
        }

        MPGameManager.Instance.playersReady++;
        MPGameManager.Instance.CheckIfVoted();
    }
}

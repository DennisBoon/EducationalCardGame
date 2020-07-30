using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class EndingScreen : MonoBehaviour
{
    private int replayPlayers = 0;

    public void ReplayGame()
    {
        MPGameManager.Instance.gameSetup.endingCanvas.transform.GetChild(0).gameObject.SetActive(false);
        MPGameManager.Instance.gameSetup.endingCanvas.transform.GetChild(1).gameObject.SetActive(true);
        this.GetComponent<PhotonView>().RPC("RPC_ReplayGame", RpcTarget.AllBuffered);
    }

    public void LeaveGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("HOST LEFT");
            this.GetComponent<PhotonView>().RPC("RPC_RemoveRoom", RpcTarget.AllBuffered);
        }
        else
        {
            Debug.Log("PLAYER LEFT");
            PhotonNetwork.LoadLevel(0);
        }
    }

    [PunRPC]
    public void RPC_ReplayGame()
    {
        replayPlayers++;

        if (PhotonNetwork.CurrentRoom.PlayerCount == replayPlayers)
        {
            for (int i = 0; i < MPGameManager.Instance.dilemma.dilemmaContainer.ExposedProperties.Count; i++)
            {
                MPGameManager.Instance.dilemma.dilemmaContainer.ExposedProperties[i].PropertyValue = 0;
            }
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name); // Restart the game
        }
        else
        {
            Debug.Log("Waiting on other players...");
        }
    }

    [PunRPC]
    public void RPC_RemoveRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
}

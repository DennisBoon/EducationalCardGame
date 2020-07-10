using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPGameManager : MonoBehaviour
{
    private static MPGameManager _instance;
    public static MPGameManager Instance { get { return _instance; } }

    [HideInInspector]
    public int playersReady = 0;

    public GameObject infoBoards;
    public GameObject turnManager;
    public GameObject roleManagerCanvas;

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
    }

    public void CheckIfReady()
    {
        playersReady++;

        if (PhotonNetwork.CurrentRoom.PlayerCount == playersReady)
        {
            Debug.Log("Start the game!");
            for (int i = 0; i < 4; i++)
            {
                infoBoards.transform.GetChild(i).gameObject.SetActive(true);
            }
            roleManagerCanvas.SetActive(false);
            turnManager.SetActive(true);
        }
        else
        {
            Debug.Log("Waiting on other players...");
        }
    }
}

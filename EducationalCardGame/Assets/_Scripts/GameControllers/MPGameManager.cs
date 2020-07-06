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
        }
    }
}

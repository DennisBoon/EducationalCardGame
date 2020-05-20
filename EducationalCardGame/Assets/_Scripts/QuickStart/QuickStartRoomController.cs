using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex; // Number for the build index to the multiplayer scene.

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() // Callback function for when we succesfully create or join a room
    {
        Debug.Log("JOINED ROOM");
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("STARTING GAME");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex); // Because of AutoSyncScene all players who join will have this scene.
        }
    }
}

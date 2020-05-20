using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject delayStartButton;
    [SerializeField]
    private GameObject delayCancelButton;
    [SerializeField]
    private int roomSize; // Manually set number of players in the room at one time.

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        delayStartButton.SetActive(true);
    }

    public void DelayStart()
    {
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("DELAY START");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("FAILED TO JOIN A ROOM");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("CREATING ROOM...");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log("ROOM NUMBER: " + randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("FAILED TO CREATE ROOM, TRYING AGAIN...");
        CreateRoom();
    }

    public void DelayCancel()
    {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}

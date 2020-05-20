using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
    // Photon view for sending rpc that updates the timer
    private PhotonView myPhotonView;

    // Scene navigation indexes
    [SerializeField]
    private int multiplayerSceneIndex;
    [SerializeField]
    private int menuSceneIndex;

    // Number of players in the room out of the total room size
    private int playerCount;
    private int roomSize;
    [SerializeField]
    private int minPlayersToStart;

    // Text variables for holding the displays for the countdown timer and player count
    [SerializeField]
    private TMP_Text roomCountDisplay;
    [SerializeField]
    private TMP_Text timerToStartDisplay;

    // Bool values for if the timer can count down
    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;

    // Countdown timer variables
    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    // Countdown timer reset variables
    [SerializeField]
    private float maxWaitTime;
    [SerializeField]
    private float maxFullGameWaitTime;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        // Updates player count when players join the room
        // Displays player count
        // Triggers countdown timer
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playerCount + ":" + roomSize;

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Called whenever a new player joins the room
        PlayerCountUpdate();
        // Send master clients countdown timer to all other players in order to sync time.
        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SENDTIMER", RpcTarget.Others, timerToStartGame);     
    }

    [PunRPC]
    private void RPC_SENDTIMER(float timeIn)
    {
        // RPC for syncing the countdown timer to those that join after it has started the countdown.
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Called whenever a player leaves the room.
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        // If there is only one player in the room the timer will stop and reset.
        if (playerCount <= 1)
        {
            ResetTimer();
        }

        // When there are enough players in the room the start timer will begin counting down.
        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        // Format and display countdown timer
        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        // If the countdown timer reaches 0 the game will start.
        if (timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        // Resets the countdown timer
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    public void StartGame()
    {
        // Multiplayer scene is loaded to start the game.
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancel()
    {
        // Public function paired to cancel button in waiting room scene
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}

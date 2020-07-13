using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // This script will be added to any multiplayer scene
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public GameObject playerParentObject;
    public GameObject canvas;

    public GameObject gameCardOne;
    public GameObject gameCardTwo;

    public List<GameObject> infoBoardsWithText = new List<GameObject>();
    public List<GameObject> infoBoardsWithoutText = new List<GameObject>();

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    private List<Transform> canvasSpawnPoints = new List<Transform>();
    [SerializeField]
    private List<Transform> infoBoardSpawnPoints = new List<Transform>();
    [SerializeField]
    private List<Transform> gameCardOneSpawnPoints = new List<Transform>();
    [SerializeField]
    private List<Transform> gameCardTwoSpawnPoints = new List<Transform>();

    private void Start()
    {
        Debug.Log("START GAME SETUP");

        // Create an int to make sure all objects are set at the right position
        int spawnPicker = Random.Range(0, spawnPoints.Count);

        // Set position and rotation for player object, remove spawn point object and listing afterwards
        playerParentObject.transform.position = spawnPoints[spawnPicker].position;
        playerParentObject.transform.rotation = spawnPoints[spawnPicker].rotation;
        Destroy(spawnPoints[spawnPicker].gameObject);
        spawnPoints.Remove(spawnPoints[spawnPicker]);

        // Set position and rotation for canvas object, remove spawn point object and listing afterwards
        canvas.transform.position = canvasSpawnPoints[spawnPicker].position;
        canvas.transform.rotation = canvasSpawnPoints[spawnPicker].rotation;
        Destroy(canvasSpawnPoints[spawnPicker].gameObject);
        canvasSpawnPoints.Remove(canvasSpawnPoints[spawnPicker]);

        // Set position and rotation for infoboard object, remove spawn point object and listing afterwards
        foreach (GameObject boardWithText in infoBoardsWithText)
        {
            boardWithText.transform.position = infoBoardSpawnPoints[spawnPicker].position;
            boardWithText.transform.rotation = infoBoardSpawnPoints[spawnPicker].rotation;
        }

        Destroy(infoBoardSpawnPoints[spawnPicker].gameObject);
        infoBoardSpawnPoints.Remove(infoBoardSpawnPoints[spawnPicker]);

        // Set position and rotation for game card one object, remove spawn point object and listing afterwards
        gameCardOne.transform.position = gameCardOneSpawnPoints[spawnPicker].position;
        gameCardOne.transform.rotation = gameCardOneSpawnPoints[spawnPicker].rotation;
        Destroy(gameCardOneSpawnPoints[spawnPicker].gameObject);
        gameCardOneSpawnPoints.Remove(gameCardOneSpawnPoints[spawnPicker]);

        // Set position and rotation for game card one object, remove spawn point object and listing afterwards
        gameCardTwo.transform.position = gameCardTwoSpawnPoints[spawnPicker].position;
        gameCardTwo.transform.rotation = gameCardTwoSpawnPoints[spawnPicker].rotation;
        Destroy(gameCardTwoSpawnPoints[spawnPicker].gameObject);
        gameCardTwoSpawnPoints.Remove(gameCardTwoSpawnPoints[spawnPicker]);

        for (int i = 0; i < infoBoardSpawnPoints.Count; i++)
        {
            infoBoardsWithoutText[i].transform.position = infoBoardSpawnPoints[i].position;
            infoBoardsWithoutText[i].transform.rotation = infoBoardSpawnPoints[i].rotation;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                GameObject playerObject = PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
                canvas.GetComponent<RoleManager>().player = playerObject;
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }

            Debug.Log("GAME SETUP COMPLETED");
        }
    }
}

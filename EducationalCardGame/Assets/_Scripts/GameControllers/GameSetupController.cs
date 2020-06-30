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

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    private List<Transform> canvasSpawnPoints = new List<Transform>();

    public GameObject canvas;

    private void Start()
    {
        Debug.Log("START GAME SETUP");

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }

            Debug.Log("GAME SETUP COMPLETED");
        }

        int spawnPicker = Random.Range(0, spawnPoints.Count);

        // Spawnpoints index will temporarely be set to 0 for testing.
        // And because for some reason the spawning sometimes does not work correctly...

        this.transform.position = spawnPoints[0].position;
        this.transform.rotation = spawnPoints[0].rotation;
        Destroy(spawnPoints[spawnPicker].gameObject);
        spawnPoints.Remove(spawnPoints[spawnPicker]);

        canvas.transform.position = canvasSpawnPoints[0].position;
        canvas.transform.rotation = canvasSpawnPoints[0].rotation;
        Destroy(canvasSpawnPoints[spawnPicker].gameObject);
        canvasSpawnPoints.Remove(canvasSpawnPoints[spawnPicker]);
    }
}

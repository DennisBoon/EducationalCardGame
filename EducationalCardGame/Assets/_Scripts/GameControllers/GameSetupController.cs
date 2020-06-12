using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // This script will be added to any multiplayer scene
    //public Transform[] spawnPoints;
    //private bool gameSetupCompleted = false;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

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
                //int spawnPicker = Random.Range(0, spawnPoints.Length);
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                //PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoints[spawnPicker].position,
                //spawnPoints[spawnPicker].rotation, 0);
                PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }

            //if (PhotonNetwork.IsMasterClient)
            //{
            //    CreateEnvironment(); // Create a networked environment object for each player that's in the multiplayerscene.
            //}

            Debug.Log("GAME SETUP COMPLETED");
        }

        //if (!gameSetupCompleted)
        //{
        //    CreatePlayer(); // Create a networked player object for each player that's in the multiplayerscene.
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        CreateEnvironment(); // Create a networked environment object for each player that's in the multiplayerscene.
        //    }
        //    gameSetupCompleted = true;
        //}
    }

    //private void CreatePlayer()
    //{
    //    int spawnPicker = Random.Range(0, spawnPoints.Length);
    //    Debug.Log("CREATING PLAYER CHECKER");
    //    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerChecker"), spawnPoints[spawnPicker].position, 
    //        spawnPoints[spawnPicker].rotation, 0);
    //}

    //private void CreateEnvironment()
    //{
    //    Debug.Log("CREATING ENVIRONMENT");
    //    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonEnvironment"), Vector3.zero, Quaternion.identity);
    //}
}

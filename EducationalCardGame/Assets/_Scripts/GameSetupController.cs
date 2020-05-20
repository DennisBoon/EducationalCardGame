using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // This script will be added to any multiplayer scene
    private void Start()
    {
        CreatePlayer(); // Create a networked player object for each player that's in the multiplayerscene.
        CreateEnvironment(); // Create a networked environment object for each player that's in the multiplayerscene.
    }

    private void CreatePlayer()
    {
        Debug.Log("CREATING PLAYER");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }

    private void CreateEnvironment()
    {
        Debug.Log("CREATING ENVIRONMENT");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonEnvironment"), Vector3.zero, Quaternion.identity);
    }
}

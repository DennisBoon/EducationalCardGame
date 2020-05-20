using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Function that will try to establish a connection with the Photon server
    }

    public override void OnConnectedToMaster() // Callback function for when the first connection is established.
    {
        Debug.Log("CONNECTED TO " + PhotonNetwork.CloudRegion + " SERVER");
    }
}

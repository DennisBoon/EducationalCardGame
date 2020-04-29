using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviour, IOnEventCallback
{
    public const byte InstantiateVrAvatarEventCode = 1; // example code, change to any value between 1 and 199

    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == InstantiateVrAvatarEventCode)
        {
            GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
            PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
            photonView.ViewID = (int)photonEvent.CustomData;
        }
    }
}

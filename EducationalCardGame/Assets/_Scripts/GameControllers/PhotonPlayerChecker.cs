using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayerChecker : MonoBehaviour
{
    private PhotonView PV;

    //[SerializeField]
    //Behaviour[] componentsToDisable;

    //[SerializeField]
    //CharacterController characterController;

    //[SerializeField]
    //string remoteLayerName = "RemotePlayer";

    public void OnEnable()
    {
        PV = GetComponent<PhotonView>();

        if (!PV.IsMine)
        {
            Debug.Log("THIS IS A REMOTE PLAYER");
            //CreateRemotePlayer();
            //DisableComponents();
            //AssignRemoteLayer();
        }
        else
        {
            Debug.Log("THIS IS MY PLAYER");
            CreateLocalPlayer();
        }
    }

    void CreateLocalPlayer()
    {
        Debug.Log("CREATING LOCAL PLAYER");
        var localPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonLocalPlayer"), gameObject.transform.position,
            gameObject.transform.rotation, 0);
        localPlayer.transform.parent = gameObject.transform;
        this.enabled = false;
    }

    void CreateRemotePlayer()
    {
        Debug.Log("CREATING REMOTE PLAYER");
        var remotePlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonRemotePlayer"), gameObject.transform.position,
            gameObject.transform.rotation, 0);
        remotePlayer.transform.parent = gameObject.transform;
        this.enabled = false;
    }

    //void DisableComponents()
    //{
    //    for (int i = 0; i < componentsToDisable.Length; i++)
    //    {
    //        componentsToDisable[i].enabled = false;
    //    }
    //    characterController.enabled = false;
    //}

    //void AssignRemoteLayer()
    //{
    //    gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    //}
}

using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : MonoBehaviour
{
    private RoleData roleData;

    public GameObject photonViewObject;

    private const int size = 4;
    public Text[] roleNameTexts = new Text[size];
    public Text[] playerNameTexts = new Text[size];
    public Button[] roleButtons = new Button[size];

    private void OnValidate()
    {
        if (roleNameTexts.Length != size)
        {
            Debug.LogWarning("This array's size cannot be changed");
            Array.Resize(ref roleNameTexts, size);
        }
        else if (playerNameTexts.Length != size)
        {
            Debug.LogWarning("This array's size cannot be changed");
            Array.Resize(ref playerNameTexts, size);
        }
        else if (roleButtons.Length != size)
        {
            Debug.LogWarning("This array's size cannot be changed");
            Array.Resize(ref roleButtons, size);
        }
    }

    private void Awake()
    {
        roleData = GetComponentInParent<RoleData>();

        for (int i = 0; i < roleNameTexts.Length; i++)
        {
            roleNameTexts[i].text = roleData.roleNames[i];
            playerNameTexts[i].text = roleData.playerNames[i];
        }
    }

    [PunRPC]
    public void SelectRole(int roleNumber)
    {
        photonViewObject.GetComponentInChildren<PlayerManager>().role = roleData.roleNames[roleNumber];
        playerNameTexts[roleNumber].text = photonViewObject.GetComponentInChildren<PhotonView>().Owner.NickName;
        roleButtons[roleNumber].interactable = false;
    }
}

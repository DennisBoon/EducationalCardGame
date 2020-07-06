using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : MonoBehaviour
{
    private RoleData roleData;
    [HideInInspector]
    public GameObject player;
    public GameObject laserPointer;

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

    public void SelectRole(int roleNumber)
    {
        player.GetComponent<PlayerManager>().role = roleData.roleNames[roleNumber];
        playerNameTexts[roleNumber].text = player.GetComponent<PhotonView>().Owner.NickName;
        roleButtons[roleNumber].interactable = false;
        laserPointer.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        player.GetComponent<PhotonView>().RPC("RPC_SelectRole", RpcTarget.AllBuffered, roleNumber, 
            playerNameTexts[roleNumber].text);
    }

    [PunRPC]
    void RPC_SelectRole(int roleNumber, string playerName)
    {
        playerNameTexts[roleNumber].text = playerName;
        roleButtons[roleNumber].interactable = false;
        MPGameManager.Instance.CheckIfReady();
    }
}

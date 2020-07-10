using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleData : MonoBehaviour
{
    private const int size = 4;
    public string[] roleNames = new string[size];
    public string[] playerNames = new string[size];

    public List<string> roleDescriptions = new List<string>();

    private void OnValidate()
    {
        if (roleNames.Length != size)
        {
            Debug.LogWarning("This array's size cannot be changed");
            Array.Resize(ref roleNames, size);
        }
        else if (playerNames.Length != size)
        {
            Debug.LogWarning("This array's size cannot be changed");
            Array.Resize(ref playerNames, size);
        }
    }
}

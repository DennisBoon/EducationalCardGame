using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        int spawnPicker = Random.Range(0, spawnPoints.Length);
        this.transform.position = spawnPoints[spawnPicker].position;
        this.transform.rotation = spawnPoints[spawnPicker].rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        int spawnPicker = Random.Range(0, spawnPoints.Count);
        this.transform.position = spawnPoints[spawnPicker].position;
        this.transform.rotation = spawnPoints[spawnPicker].rotation;
        Destroy(spawnPoints[spawnPicker].gameObject);
        spawnPoints.Remove(spawnPoints[spawnPicker]);
    }
}

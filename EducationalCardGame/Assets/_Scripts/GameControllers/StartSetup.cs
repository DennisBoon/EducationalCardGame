using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetup : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    private List<string> playerRoles = new List<string>();

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

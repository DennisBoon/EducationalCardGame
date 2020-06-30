using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetup : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    private List<Transform> canvasSpawnPoints = new List<Transform>();

    public GameObject canvas;

    void Start()
    {
        int spawnPicker = Random.Range(0, spawnPoints.Count);

        // Spawnpoints index will temporarely be set to 0 for testing.
        // And because for some reason the spawning sometimes does not work correctly...

        this.transform.position = spawnPoints[0].position;
        this.transform.rotation = spawnPoints[0].rotation;
        Destroy(spawnPoints[spawnPicker].gameObject);
        spawnPoints.Remove(spawnPoints[spawnPicker]);

        canvas.transform.position = canvasSpawnPoints[0].position;
        canvas.transform.rotation = canvasSpawnPoints[0].rotation;
        Destroy(canvasSpawnPoints[spawnPicker].gameObject);
        canvasSpawnPoints.Remove(canvasSpawnPoints[spawnPicker]);
    }
}

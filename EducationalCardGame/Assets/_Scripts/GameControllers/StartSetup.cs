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

        this.transform.position = spawnPoints[spawnPicker].position;
        this.transform.rotation = spawnPoints[spawnPicker].rotation;
        Destroy(spawnPoints[spawnPicker].gameObject);
        spawnPoints.Remove(spawnPoints[spawnPicker]);

        canvas.transform.position = canvasSpawnPoints[spawnPicker].position;
        canvas.transform.rotation = canvasSpawnPoints[spawnPicker].rotation;
        Destroy(canvasSpawnPoints[spawnPicker].gameObject);
        canvasSpawnPoints.Remove(canvasSpawnPoints[spawnPicker]);
    }
}

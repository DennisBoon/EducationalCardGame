using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject lookAtObject;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.LookAt(lookAtObject.transform);
    }
}

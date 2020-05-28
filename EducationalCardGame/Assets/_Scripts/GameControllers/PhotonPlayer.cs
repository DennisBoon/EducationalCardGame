using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    [SerializeField]
    private GameObject[] eyeAnchors;

    // Start is called before the first frame update
    public void OnEnable()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            Debug.Log("THIS IS MY PLAYER");
        }
        else
        {
            Debug.Log("THIS IS NOT MY PLAYER");
            foreach (GameObject obj in eyeAnchors)
            {
                Destroy(obj);
            }
            Debug.Log("DESTROYED TRACKING OF OTHER PLAYER");
        }
    }
}

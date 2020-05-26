using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public int FPS { get; private set; }
    public TextMeshPro displayCurrent;

    public void Update()
    {
        float current = (int)(1f / Time.deltaTime);
        if (Time.frameCount % 50 == 0)
            displayCurrent.text = current.ToString() + " FPS";
    }
}

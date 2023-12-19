using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPS_Debug : MonoBehaviour
{
    public TMP_Text fpsText;
    public float deltaTime;

    void Update()
    {
        updateFPS();
    }

    void updateFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}

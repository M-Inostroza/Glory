using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnims : MonoBehaviour
{
    private GameObject storeName;
    private GameObject playerCanvas;
    private void Start()
    {
        storeName = GameObject.Find("Store");
        playerCanvas = GameObject.Find("PlayerCanvas");
    }

    public void AfterCameraFadeIn()
    {
        storeName.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OnCameraFadeOut()
    {
        storeName.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void AfterProfileFadeIn()
    {
        playerCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void OnProfileFadeOut()
    {
        playerCanvas.transform.GetChild(1).gameObject.SetActive(false);
    }
}

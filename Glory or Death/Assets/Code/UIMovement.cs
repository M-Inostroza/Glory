using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMovement : MonoBehaviour
{
    private Animator cameraController;
    private bool shop = false;
    private bool profile = false;
    private void Awake()
    {
        if(GameObject.Find("Main Camera").GetComponent<Animator>() != null)
        {
            cameraController = GameObject.Find("Main Camera").GetComponent<Animator>();
        }
    }
    public void LoadSceneAsync(int sceneNumber)
    {
        StartCoroutine(LoadScene(sceneNumber));
    }

    private IEnumerator LoadScene(int sceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void ShopButton()
    {
        if (!shop)
        {
            cameraController.Play("StoreZoomIn");
            shop = true;
        }
        else
        {
            shop= false;
            cameraController.Play("StoreZoomOut");
        }
    }
    public void ProfileButton()
    {
        if(!profile)
        {
            cameraController.Play("ProfileZoomIn");
            profile = true;
        }
        else
        {
            cameraController.Play("ProfileZoomOut");
            profile= false;
        }
    }
}

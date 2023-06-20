using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterManager : MonoBehaviour
{
    public GameObject shieldImage;
    public GameObject counterBullet;
    public GameObject overlay;
    public Camera mainCamera;


    float rotationSpeed = 150;

    private void Start()
    {
        counterBullet.transform.DOLocalMoveX(-17, 16);
    }

    private void Update()
    {
        rotateOnKey();
        rotateOnStart();
    }

    private void OnEnable()
    {
        startMinigame();
    }
    private void OnDisable()
    {
        closeMinigame();
    }

    void rotateOnKey()
    {
        if (Input.GetKey(KeyCode.X))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            shieldImage.transform.Rotate(0f, 0f, rotationAmount);
        }
    }

    void rotateOnStart()
    {
        float rotationAmount = 20 * Time.deltaTime;
        shieldImage.transform.Rotate(0f, 0f, rotationAmount);
    }

    public void startMinigame()
    {
        mainCamera.DOFieldOfView(20, 1);
        mainCamera.transform.DOLocalMoveY(-2.5f, 1);
        overlay.GetComponent<Image>().DOFade(0.85f, 1);
    }

    public void closeMinigame()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
        overlay.GetComponent<Image>().DOFade(0, 0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterManager : MonoBehaviour
{
    [SerializeField] GameObject shieldImage, counterBullet, counterTarget, overlay;
    [SerializeField] Camera mainCamera;

    float rotationSpeed = 100;

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
        counterTarget.transform.DOPunchScale(new Vector3(0.05f,0.05f,0.05f), 1, 6, 3).SetLoops(-1, LoopType.Restart);
        mainCamera.DOFieldOfView(20, 1);
        mainCamera.transform.DOLocalMoveY(-2.5f, 1);
        overlay.GetComponent<Image>().DOFade(0.85f, 1);
        counterBullet.transform.DOLocalMoveX(-4, 6).OnComplete(()=> counterBullet.transform.DOLocalMoveX(12, 0));
    }

    public void closeMinigame()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
        overlay.GetComponent<Image>().DOFade(0, 0.5f);
    }
}

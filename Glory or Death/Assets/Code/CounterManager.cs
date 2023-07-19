using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterManager : MonoBehaviour
{
    [SerializeField] GameObject shieldImage, counterBullet, counterTarget, overlay;
    [SerializeField] Camera mainCamera;
    [SerializeField] Combat_UI combatUI;

    float rotationSpeed = 13;
    bool canRotate = false;

    [Header("Materials")]
    [SerializeField] Material heartMaterial;
    [SerializeField] Material swordMaterial;
    [SerializeField] Material shieldMaterial;

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
        if (Input.GetKeyUp(KeyCode.X) && canRotate)
        {
            shieldImage.transform.Rotate(0f, 0f, rotationSpeed);
        }
    }

    void rotateOnStart()
    {
        if (canRotate)
        {
            float rotationAmount = 10 * Time.deltaTime;
            shieldImage.transform.Rotate(0f, 0f, rotationAmount);
        }
    }

    public void startMinigame()
    {
        canRotate = true;
        setRandomRotation();
        moveCameraIn();

        heartMaterial.SetFloat("_FadeAmount", 0);
        swordMaterial.SetFloat("_FadeAmount", 0);
        shieldMaterial.SetFloat("_FadeAmount", 0);
        counterBullet.transform.DOLocalMoveX(-3.25f, 8.5f).SetEase(Ease.OutBack).OnComplete(()=> counterBullet.transform.DOLocalMoveX(12, 0));
        combatUI.activateX();
    }

    public void closeMinigame()
    {
        moveCameraOut();
        canRotate = false;
    }
    void setRandomRotation()
    {
        float randomRotation = Random.Range(66, 170);
        shieldImage.transform.DORotate(new Vector3(0, 0, randomRotation), 0);
        counterTarget.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 1, 6, 3).SetLoops(3, LoopType.Restart);
    }
    void moveCameraIn()
    {
        mainCamera.DOFieldOfView(20, 0.7f);
        mainCamera.transform.DOLocalMoveY(-2.5f, 0.7f);
        overlay.GetComponent<Image>().DOFade(0.85f, 0.5f);
    }
    void moveCameraOut()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
        overlay.GetComponent<Image>().DOFade(0, 0.6f);
    }

    public void canRotateBool(bool state)
    {
        canRotate = state;
    }
}

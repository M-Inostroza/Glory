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

    float rotationSpeed = 100;
    Material heartMaterial;

    private void Awake()
    {
        heartMaterial = counterTarget.GetComponent<Image>().GetComponent<Material>();
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
        float rotationAmount = 10 * Time.deltaTime;
        shieldImage.transform.Rotate(0f, 0f, rotationAmount);
    }

    public void startMinigame()
    {
        setRandomRotation();
        moveCameraIn();

        heartMaterial.SetFloat("_FadeAmount", 0);
        counterBullet.transform.DOLocalMoveX(-3.25f, 8.5f).SetEase(Ease.OutBack).OnComplete(()=> counterBullet.transform.DOLocalMoveX(12, 0));
        combatUI.activateX();
    }

    public void closeMinigame()
    {
        moveCameraOut();
    }
    void setRandomRotation()
    {
        float randomRotation = Random.Range(0, 360);
        shieldImage.transform.DORotate(new Vector3(0, 0, randomRotation), 0);
        counterTarget.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 1, 6, 3).SetLoops(-1, LoopType.Restart);
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
}

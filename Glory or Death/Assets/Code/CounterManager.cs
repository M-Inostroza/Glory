using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterManager : MonoBehaviour
{
    [SerializeField] GameObject shieldImage, _sword, counterTarget;
    [SerializeField] Camera mainCamera;

    [SerializeField] Image _overlay;
    Combat_UI combatUI;
    TutorialManager TutorialManager;
    cameraManager _cameraManager;

    static float rotationSpeed = 13;
    bool canRotate = false;

    [Header("Materials")]
    [SerializeField] Material heartMaterial;
    [SerializeField] Material swordMaterial;
    [SerializeField] Material shieldMaterial;

    private void Awake()
    {
        combatUI = FindObjectOfType<Combat_UI>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        _cameraManager = FindObjectOfType<cameraManager>();
    }
    private void Update()
    {
        rotateOnKey();
        rotateOnStart();
    }

    private void OnEnable()
    {
        startMinigame();
        fadeElements(0.1f, true);
    }
    private void OnDisable()
    {
        closeMinigame();
    }

    void rotateOnKey()
    {
        if (Input.GetKeyUp(KeyCode.X) && canRotate && !BattleSystem.IsPaused)
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
        _cameraManager.playChrome();
        canRotate = true;
        setRandomRotation();
        moveCameraIn();
        
        heartMaterial.SetFloat("_FadeAmount", 0);
        swordMaterial.SetFloat("_FadeAmount", 0);
        shieldMaterial.SetFloat("_FadeAmount", 0);

        if (!gameManager.isTutorial())
        {
            combatUI.activateX();
        } else
        {
            TutorialManager.hideUI();
            TutorialManager.activateX();
        }
        _sword.transform.DOLocalMoveX(-3.25f, 3).SetEase(Ease.Linear);
    }

    public void closeMinigame()
    {
        if (gameManager.isTutorial())
        {
            TutorialManager.showUI();
            TutorialManager.fadeTimer(1);
            if (TutorialManager._hasPlayedTutorial)
            {
                TutorialManager.showAllInput(1);
            }
        }
        _sword.transform.DOKill();
        _sword.transform.DOLocalMoveX(12, 0);
        moveCameraOut();
        canRotate = false;
    }
    void setRandomRotation()
    {
        float randomRotation = Random.Range(90, 170); // Default 75, 170 / Center -20 
        shieldImage.transform.DORotate(new Vector3(0, 0, randomRotation), 0);
        counterTarget.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 1, 6, 3).SetLoops(3, LoopType.Restart);
    }
    void moveCameraIn()
    {
        mainCamera.DOFieldOfView(20, 0.7f);
        mainCamera.transform.DOLocalMoveY(-2.5f, 0.7f);
        _overlay.DOFade(0.85f, 0.5f);
    }
    void moveCameraOut()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
        _overlay.DOFade(0, 0.6f);
    }

    public void fadeElements(float speed, bool In)
    {
        if (In)
        {
            shieldImage.GetComponent<Image>().DOFade(1, speed);
            _sword.GetComponent<Image>().DOFade(1, speed);
            counterTarget.GetComponent<Image>().DOFade(1, speed);
        } else
        {
            shieldImage.GetComponent<Image>().DOFade(0, speed);
            _sword.GetComponent<Image>().DOFade(0, speed);
            counterTarget.GetComponent<Image>().DOFade(0, speed);
        }
    }

    public void canRotateBool(bool state)
    {
        canRotate = state;
    }

    public static float GetRotationSpeed()
    {
        return rotationSpeed;
    }
    public static void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}

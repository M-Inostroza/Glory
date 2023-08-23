using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssetKits.ParticleImage;

public class restManager : MonoBehaviour
{
    [SerializeField] Slider restSlider;
    [SerializeField] Camera mainCamera;
    [SerializeField] ParticleImage stars;
    [SerializeField] Transform criticStar;
    bool hasAnimatedStar = false;

    Player player;
    timeManager timeManager;
    AudioManager audioManager;
    Tutorial_UI _tutorial_UI;

    bool canRest = false;
    private void Awake()
    {
        _tutorial_UI = FindObjectOfType<Tutorial_UI>();
        player = FindObjectOfType<Player>();
        timeManager = FindObjectOfType<timeManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Update()
    {
        reduceValueOverTime(24);
        keyStroke(4);
        animateStar();
    }

    private void OnEnable()
    {
        if (!gameManager.isTutorial())
        {
            FindObjectOfType<Combat_UI>().activateLeftRight();
            hasAnimatedStar = false;
        } else
        {
            _tutorial_UI.activateLeftRight();
        }
        restSlider.value = 0;
        cameraZoom();
        audioManager.Play("Rest_On");
        player.GetComponent<Animator>().Play("restSkill");
        canRest = true;
        StartCoroutine(setMinigameOff(4.5f));
    }

    void reduceValueOverTime(float timeFactor)
    {
        if (canRest)
        {
            restSlider.value -= Time.deltaTime * timeFactor;
        }
    }

    void keyStroke(float addedValue)
    {
        if (canRest && !BattleSystem.IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                restSlider.value += addedValue;
            }
        }
    }

    private void OnDisable()
    {
        if (!gameManager.isTutorial())
        {
            timeManager.continueUnitTimer();
            timeManager.fadeInUnitTimer();
        }
        criticStar.GetComponent<Image>().DOFade(0.25f, 0);
        canRest = false;
        player.SetCurrentStamina(restSlider.value);
        player.backToIdle();
    }

    // Utilities
    IEnumerator setMinigameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        returnCameraZoom();
        checkCritic();
        gameObject.SetActive(false);
    }
    void checkCritic()
    {
        if (restSlider.value > 85)
        {
            stars.Play();
            FindObjectOfType<Combat_UI>().showStars();
        }
    }
    private void cameraZoom()
    {
        mainCamera.DOFieldOfView(35, 1);
        mainCamera.transform.DOLocalMove(new Vector3(-1.7f, -1.5f, -10), 1);
    }
    private void returnCameraZoom()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 1);
    }
    void animateStar()
    {
        if (!hasAnimatedStar && restSlider.value > 85)
        {
            audioManager.Play("Star_Hit");
            criticStar.GetComponent<Image>().DOFade(1, 0.3f);
            criticStar.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.3f).OnComplete(()=> criticStar.DOScale(1,0));
            hasAnimatedStar = true;
        }
    }
}

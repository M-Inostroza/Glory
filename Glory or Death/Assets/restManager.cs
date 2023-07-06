using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class restManager : MonoBehaviour
{
    [SerializeField] Slider restSlider;
    [SerializeField] Camera mainCamera;

    Player player;
    timeManager timeManager;
    AudioManager audioManager;

    bool canRest = false;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
        timeManager = FindObjectOfType<timeManager>();
    }
    private void Update()
    {
        reduceValueOverTime(25);
        keyStroke(4);
    }

    private void OnEnable()
    {
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
        if (canRest)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                restSlider.value += addedValue;
            }
        }
    }

    private void OnDisable()
    {
        canRest = false;
        player.SetCurrentStamina(restSlider.value);
        timeManager.continueUnitTimer();
        timeManager.fadeInUnitTimer();
        player.backToIdle();
    }

    // Utilities
    IEnumerator setMinigameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        returnCameraZoom();
        gameObject.SetActive(false);
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
}

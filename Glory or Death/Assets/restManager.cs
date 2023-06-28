using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class restManager : MonoBehaviour
{
    [SerializeField] Slider restSlider;

    Player player;
    timeManager timeManager;

    bool canRest = false;
    private void Awake()
    {
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
        player.GetComponent<Animator>().Play("restSkill");
        canRest = true;
        StartCoroutine(setMinigameOff(4));
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
        player.currentStamina = restSlider.value;
        timeManager.continueUnitTimer();
        timeManager.fadeInUnitTimer();
        player.backToIdle();
    }

    IEnumerator setMinigameOff(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}

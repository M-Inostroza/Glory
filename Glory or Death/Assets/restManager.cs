using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class restManager : MonoBehaviour
{
    [SerializeField] Slider restSlider;

    Player player;

    bool canRest = false;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        reduceValueOverTime(35);
        keyStroke(4);
    }

    private void OnEnable()
    {
        canRest = true;
        StartCoroutine(setMinigameOff(5));
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
    }

    IEnumerator setMinigameOff(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}

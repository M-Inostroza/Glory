using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    private timeManager timeManager;
    private BattleSystem battleSystem;
    private AudioManager audioManager;

    public int turnCounter;

    private void Start()
    {
        DOTween.SetTweensCapacity(1750, 500);

        timeManager = FindObjectOfType<timeManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.Play("Combat_Theme");
    }

    private void Update()
    {
        quitGame();
        if (timeManager.timeOut)
        {
            timeManager.playerTimerControl = false;
            timeManager.enemyTimerControl = false;
        }
    }

    void quitGame()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}

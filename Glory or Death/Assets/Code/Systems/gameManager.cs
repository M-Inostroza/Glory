using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    private timeManager timeManager;
    private BattleSystem battleSystem;

    public int turnCounter;

    private void Start()
    {
        DOTween.SetTweensCapacity(500, 50);

        timeManager = FindObjectOfType<timeManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void Update()
    {
        if (timeManager.timeOut)
        {
            timeManager.playerTimerControl = false;
            timeManager.enemyTimerControl = false;
        }
    }

    public void showSummery()
    {
        Debug.Log("Showing summery");
    }
}

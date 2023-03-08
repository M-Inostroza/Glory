using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class gameManager : MonoBehaviour
{
    public int tween;

    private timeManager timeManager;
    private BattleSystem battleSystem;

    public int turnCounter;

    [SerializeField]
    private Transform endFightUI;

    private void Start()
    {
        DOTween.SetTweensCapacity(750, 50);

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
        endFightUI.DOMoveY(tween, 1f);
    }
}

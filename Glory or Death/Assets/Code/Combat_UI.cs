using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Combat_UI : MonoBehaviour
{
    [SerializeField]
    private Transform player_stats, enemy_stats, player_stamina, enemy_stamina, player_timer, enemy_timer;

    [SerializeField]
    private GameObject shieldManager, inputManager, globalTimer;

    //Unit sliders
    [SerializeField]
    private Player playerUnit;
    [SerializeField]
    private Enemy enemyUnit;

    private void OnEnable()
    {
        move_UI();
    }
    private void Update()
    {
        refillStamina();
    }

    void move_UI()
    {
        player_stats.DOLocalMoveX(player_stats.localPosition.x + 340, 1f).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x - 340, 1f).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x + 180, 1f).SetEase(Ease.InOutSine);
        enemy_stamina.DOLocalMoveX(enemy_stamina.localPosition.x - 180, 1f).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y - 160, 1f);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y - 160, 1f);

        inputManager.transform.DOLocalMoveX(-415, 1f).SetEase(Ease.InOutSine);
        globalTimer.transform.DOLocalMoveY(globalTimer.transform.localPosition.y - 80, 1f).SetEase(Ease.InOutSine);
    }

    void refillStamina()
    {
        playerUnit.currentStamina += 0.5f * Time.deltaTime;
        enemyUnit.currentStamina += 0.5f * Time.deltaTime;
    }
}

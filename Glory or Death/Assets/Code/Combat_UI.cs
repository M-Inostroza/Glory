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
        move_UI_in();
    }
    private void Update()
    {
        refillStamina();
    }

    float move_in_speed = 1;

    public void move_UI_in()
    {
        player_stats.DOLocalMoveX(player_stats.localPosition.x + 340, move_in_speed).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x - 340, move_in_speed).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x + 180, move_in_speed).SetEase(Ease.InOutSine);
        enemy_stamina.DOLocalMoveX(enemy_stamina.localPosition.x - 180, move_in_speed).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y - 160, move_in_speed);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y - 160, move_in_speed);

        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x + 80, move_in_speed).SetEase(Ease.InOutSine);
        globalTimer.transform.DOLocalMoveY(globalTimer.transform.localPosition.y - 80, move_in_speed).SetEase(Ease.InOutSine);
    }

    float move_out_speed = 0.6f;
    public void move_UI_out()
    {
        player_stats.DOLocalMoveX(player_stats.localPosition.x - 340, move_out_speed).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x + 340, move_out_speed).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x - 180, move_out_speed).SetEase(Ease.InOutSine);
        enemy_stamina.DOLocalMoveX(enemy_stamina.localPosition.x + 180, move_out_speed).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y + 160, move_out_speed);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y + 160, move_out_speed);

        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x - 80, move_out_speed).SetEase(Ease.InOutSine);
        globalTimer.transform.DOLocalMoveY(globalTimer.transform.localPosition.y + 80, move_out_speed).SetEase(Ease.InOutSine);
    }

    void refillStamina()
    {
        if (playerUnit.currentStamina < playerUnit.maxStamina)
        {
            playerUnit.currentStamina += 0.5f * Time.deltaTime; //Mejorable
        }
        if (enemyUnit.currentStamina < enemyUnit.maxStamina)
        {
            enemyUnit.currentStamina += 0.5f * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Combat_UI : MonoBehaviour
{
    [SerializeField]
    private Transform player_stats, enemy_stats;

    [SerializeField]
    private GameObject shieldManager, staminaContainer, inputManager;

    private void OnEnable()
    {
        move_UI();
    }

    void move_UI()
    {
        player_stats.DOLocalMoveX(player_stats.localPosition.x + 300, 1f).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x - 300, 1f).SetEase(Ease.InOutSine);

        //shieldManager.transform.DOLocalMoveX(shieldManager.transform.localPosition.x + 400, 1f).SetEase(Ease.InOutSine);
        staminaContainer.transform.DOLocalMoveY(-260, 1f).SetEase(Ease.InOutSine);
        inputManager.transform.DOLocalMoveX(-410, 1f).SetEase(Ease.InOutSine);
    }
}

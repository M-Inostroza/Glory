using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ui_anim_control : MonoBehaviour
{
    public Transform player_stats;
    public Transform enemy_stats;

    public GameObject shieldManager;

    public GameObject staminaContainer;

    public GameObject attackCommand;

    
    // Start is called before the first frame update
    private void OnEnable()
    {
        player_stats.DOLocalMoveX(player_stats.localPosition.x + 300, 1f).SetEase(Ease.InOutSine);
        shieldManager.transform.DOLocalMoveX(shieldManager.transform.localPosition.x + 400, 1f).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x - 300, 1f).SetEase(Ease.InOutSine);

        staminaContainer.transform.DOLocalMoveY(-220, 1f).SetEase(Ease.InOutSine);

        attackCommand.transform.DOMoveX(+160, 1f).SetEase(Ease.InOutSine);
    }
}

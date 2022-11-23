using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ui_anim_control : MonoBehaviour
{
    public Transform player_stats;
    public Transform enemy_stats;
    // Start is called before the first frame update
    private void OnEnable()
    {
        player_stats.DOMoveX(+340, 1.6f).SetEase(Ease.InOutElastic);
        enemy_stats.DOMoveX(+1580, 1.6f).SetEase(Ease.InOutElastic);
    }
}

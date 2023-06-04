using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class shieldPool : MonoBehaviour
{
    private Player Player;

    [SerializeField]
    private Slider shieldSlider;
    private void Start()
    {
        Player = FindObjectOfType<Player>();
        shieldSlider.maxValue = Player.GetMaxShield();
    }
    private void Update()
    {
        shieldSlider.value = Player.GetCurrentShield();
    }

    public void increaseShield()
    {
        shieldSlider.DOValue(Player.GetCurrentShield() + 1, 1);
    }
    public void decreaseShield()
    {
        shieldSlider.DOValue(Player.GetCurrentShield()  - 1, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EnemyHUD : MonoBehaviour
{
    public Slider hpSlider, adrenalineSlider, staminaSlider;
    public void setHP(int hp)
    {
        hpSlider.DOValue(hp, 0.5f);
    }

    private void Start()
    {
        // Set the stamina slider value to 100%.
        staminaSlider.value = staminaSlider.maxValue;
    }
}

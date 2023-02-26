using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Slider hpSlider, evadeSlider, adrenalineSlider, staminaSlider;
    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }

    private void Start()
    {
        // Set the stamina slider value to 100%.
        staminaSlider.value = staminaSlider.maxValue;
    }
}

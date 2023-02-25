using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;
    public Slider evadeSlider;
    public Slider adrenalineSlider;

    public Slider staminaSlider;
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

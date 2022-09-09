using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;

    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }

    
}

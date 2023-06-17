using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHUD : MonoBehaviour
{

    public Slider hpSlider, adrenalineSlider, staminaSlider;
    public void setHP(int hp)
    {
        hpSlider.DOValue(hp, 0.5f);
    }

    private void Start()
    {
        // Set the stamina slider value to 100%.
        staminaSlider.DOValue(staminaSlider.maxValue, 1.5f);
    }
}

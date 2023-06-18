using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHUD : MonoBehaviour
{
    public Slider hpSlider, adrenalineSlider;
    public void setHP(int hp)
    {
        hpSlider.DOValue(hp, 0.5f);
    }
}

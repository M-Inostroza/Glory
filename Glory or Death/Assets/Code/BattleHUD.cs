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
    public GameObject [] bricks;


    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }

    private void Start()
    {
        foreach(GameObject brick in bricks)
        {
            brick.SetActive(true);
        }
    }

    public void updateBricks(int stamina)
    {
        bricks[stamina].SetActive(false);
    }

    public void restoreBricks()
    {
        for (int i = 0; i < 5; i++)
        {
            bricks[i].SetActive(true);
        }
    }
}

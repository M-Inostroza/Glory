using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;

    public GameObject[] bricks;
    public Player playerUnit;

    public void setHP(int hp)
    {
        hpSlider.value = hp;
    }

    private void Start()
    {
        updateBricks();
    }

    void updateBricks()
    {
        foreach(GameObject brick in bricks)
        {
            brick.SetActive(true);
        }
    }

    public void takeBricks()
    {
        bricks[playerUnit.currentStamina].SetActive(false);
    }
}

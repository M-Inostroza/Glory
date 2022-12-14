using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_TImer : MonoBehaviour
{
    [SerializeField]
    private Slider leftSlider, rightSlider;


    private void Update()
    {
        leftSlider.value -= Time.deltaTime;
        rightSlider.value -= Time.deltaTime;
    }
}

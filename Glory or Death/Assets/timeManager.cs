using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class timeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image image;
    public float duration = 60f;

    public bool timeOut;


    // Player Timer
    public Image playerTimer;

    IEnumerator Start()
    {
        float elapsedTime = 0f;
        float originalFillAmount = image.fillAmount;
        int timer = 60;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(originalFillAmount, 0, elapsedTime / duration);
            timer = Mathf.RoundToInt(Mathf.Lerp(60, 0, elapsedTime / duration));
            timerText.text = timer.ToString();
            yield return null;
        }
        image.fillAmount = 0;
        timerText.text = "0";

        timeOut = true;
    }
}

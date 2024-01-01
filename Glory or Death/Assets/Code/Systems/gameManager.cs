using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class gameManager : MonoBehaviour
{
    [SerializeField] int turnCounter;
    [SerializeField] Image overlay;
    static string currentSceneName;


    public TMP_Text tsText;

    bool isPaused = false;
    private void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        DOTween.SetTweensCapacity(7500, 150);
    }
    private void Update()
    {
        pauseGame();
        //updateTimeScaleText();
        quitGame();
    }
    public void quitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void pauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                overlay.gameObject.SetActive(true);
                Time.timeScale = 0f;
                BattleSystem.IsPaused = true;
            }
            else
            {
                overlay.gameObject.SetActive(false);
                Time.timeScale = 1f;
                BattleSystem.IsPaused = false;
            }
        }
    }
    public static bool isTutorial()
    {
        if (currentSceneName == "Tutorial")
        {
            return true;
        } else
        {
            return false;
        }
    }

    void updateTimeScaleText()
    {
        if (tsText.text != null)
        {
            tsText.text = Time.timeScale.ToString();
        }
    }
}

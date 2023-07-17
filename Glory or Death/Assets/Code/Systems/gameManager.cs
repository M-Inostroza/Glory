using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    timeManager timeManager;
    BattleSystem battleSystem;
    AudioManager audioManager;

    [SerializeField] int turnCounter;
    [SerializeField] Image overlay;

    bool isPaused = false;
    private void Start()
    {
        DOTween.SetTweensCapacity(6000, 500);

        timeManager = FindObjectOfType<timeManager>();
        battleSystem = FindObjectOfType<BattleSystem>();
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.Play("Combat_Theme");
    }
    private void Update()
    {
        pauseGame();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitGame();
        }
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void pauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                overlay.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                overlay.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}

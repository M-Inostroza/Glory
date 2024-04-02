using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static int _dayCounter;
    private static int _dayRecord;

    [SerializeField] Image overlay;
    [SerializeField] Transform dayContainer;

    static string currentSceneName;

    AudioManager AudioManager;

    public TMP_Text tsText;

    bool isPaused = false;
    private void Awake()
    {
        AudioManager = FindObjectOfType<AudioManager>();
        currentSceneName = SceneManager.GetActiveScene().name;
        DOTween.SetTweensCapacity(7500, 150);
    }
    private void Update()
    {
        pauseGame();
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                AudioManager.Pause("Combat_Theme");
                overlay.gameObject.SetActive(true);
                Time.timeScale = 0f;
                BattleSystem.IsPaused = true;
            }
            else
            {
                AudioManager.Resume("Combat_Theme");
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

    public IEnumerator DayShow(float time)
    {
        dayContainer.GetChild(0).GetComponent<TMP_Text>().text = "Day " + _dayCounter.ToString();
        dayContainer.DOLocalMoveY(-175, 1);
        yield return new WaitForSeconds(time);
        dayContainer.DOLocalMoveY(-320, 1);
    }

    void updateTimeScaleText()
    {
        if (tsText.text != null)
        {
            tsText.text = Time.timeScale.ToString();
        }
    }

    // Day counters
    public void SetDayCounter(int day)
    {
        _dayCounter = day;
    }
    public void IncrementTurnCounter()
    {
        _dayCounter++;
    }
    public static int GetDayCounter()
    {
        return _dayCounter;
    }

    // Day record
    public static int GetRecordDays()
    {
        return _dayRecord;
    }
    public static void SetRecordDays(int newRecord)
    {
        _dayRecord = newRecord;
    }

    public static bool UpdateNewRecord()
    {
        if (_dayCounter >= _dayRecord)
        {
            _dayRecord = _dayCounter;
            return true;
        } else
        {
            return false;
        }
    }
}

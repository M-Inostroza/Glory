using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class timeManager : MonoBehaviour
{
    /*
     * 0: Sword
     * 1: Hourglass
     * 2: Boot
     * 3: Shield
     */
    //Array of sprites for the icons & active Icon
    public Sprite[] iconSprites;

    public Image actionIcon;
    public Image actionEnemyIcon;

    public Image playerRing;
    public Image EnemyRing;


    private BattleSystem BS;
    private Input_Manager Input_Manager;
    private gameManager GM;
    private Combat_UI combarUI;

    //-----------------------------------------------------------------------------dev-----
    private bool devMode = false;
    //-----------------------------------------------------------------------------dev-----

    //Global time
    public TextMeshProUGUI timerText;
    public float battleTimer = 90f;
    private bool timerIsRunning = false;
    public bool timeOut;

    //Cooldowns
    private float Shield_CD = 10;
    private float Attack_CD = 10;
    private float Dodge_CD = 10;
    private float Focus_CD = 10;

    // Player Timer
    public bool playerTimerControl = true;
    public Image playerTimer;
    public Image playerActionIcon;

    // Enemy Timer
    public bool enemyTimerControl = true;
    public Image enemyTimer;
    public Image enemyActionIcon;

    bool dirtPrevious = false;
    private float dirtChance = 20;

    //Generic wait time for turns
    private float mainWaitTime = 20;

    // Units
    private Player player;
    private Enemy enemy;


    //Manages general timer
    private void Start()
    {
        BS = FindObjectOfType<BattleSystem>();
        GM = FindObjectOfType<gameManager>();
        Input_Manager = FindObjectOfType<Input_Manager>();
        combarUI = FindObjectOfType<Combat_UI>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        // timerIsRunning = true;

        Input_Manager.SetPlayerAction("none");
    }

    private void Update()
    {
        playerAction();
        enemyAction();
        //runTimer();
    }

    void playerAction()
    {
        if (playerTimerControl)
        {
            playerTimer.fillAmount -= Time.deltaTime / (mainWaitTime - player.GetBaseSpeed());
        }

        //Execute selected action
        if (playerTimer.fillAmount == 0 && playerTimerControl)
        {
            stopUnitTimer(); 
            playerTimer.fillAmount = 1;
            switch (Input_Manager.GetPlayerAction())
            {
                case "ATK1":
                    if (player.currentStamina > 25)
                    {
                        BS.PlayerAttack();
                        Input_Manager.GetAttackCD().fillAmount = 1;
                        player.currentStamina -= 25;
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "DF":
                    if (player.currentStamina > 20)
                    {
                        BS.PlayerDefend();
                        Input_Manager.GetDefendCD().fillAmount = 1;
                        player.currentStamina -= 20;
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "DG":
                    if (player.currentStamina > 30)
                    {
                        BS.PlayDodge();
                        Input_Manager.GetDodgeCD().fillAmount = 1;
                        player.currentStamina -= 30;
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "FC":
                    if (player.currentStamina > 35)
                    {
                        BS.PlayFocus();
                        Input_Manager.GetFocusCD().fillAmount = 1;
                        player.currentStamina -= 35;
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "RST":
                    BS.PlayRest();
                    stopUnitTimer();
                    fadeOutUnitTimer();
                    break;

                case "none":
                    continueUnitTimer();
                    break;
            }
        }
    }
    public void continueUnitTimer()
    {
        playerTimerControl = true;
        enemyTimerControl = true;
    }

    public void stopUnitTimer()
    {
        playerTimerControl = false;
        enemyTimerControl = false;
    }

    public void showEnemyAction()
    {
        enemyActionIcon.sprite = iconSprites[0];
    }

    
    void enemyAction()
    {
        if (enemyTimerControl)
        {
            enemyTimer.fillAmount -= Time.deltaTime / (mainWaitTime - enemy.baseSpeed);
        }

        if (enemyTimer.fillAmount == 0 && enemyTimerControl)
        {
            stopUnitTimer();
            enemyTimer.fillAmount = 1;
            if (enemyActionIcon.color.a == 1)
            {
                fadeOutUnitTimer();
            }
            
            // Select action
            if (enemy.currentHP < (enemy.maxHP / 2) && enemy.getAngryState() == false)
            {
                Input_Manager.SetEnemyAction("RAGE");
                dirtPrevious = false;
                dirtChance += 5;
                enemy.setAngryState(true);
            } 
            else
            {
                float attackRandom = Random.Range(0, 99);
                if (attackRandom > dirtChance || dirtPrevious)
                {
                    Input_Manager.SetEnemyAction("ATK1");
                    dirtPrevious = false;
                    dirtChance += 5;
                }
                else
                {
                    Input_Manager.SetEnemyAction("DIRT");
                    dirtPrevious = true;
                    dirtChance = 10;
                }
            }


            // Execute action
            switch (Input_Manager.GetEnemyAction())
            {
                case "ATK1":
                    BS.EnemyTurn_attack();
                    break;

                case "DIRT":
                    BS.EnemyTurn_dirt();
                    break;

                case "RAGE":
                    BS.EnemyTurn_rage();
                    break;
            }
        }
    }


    // Cooldown timer (Mejorable!!)
    public void ReduceCooldown(Image timer)
    {
        if (timer.fillAmount > 0)
        {
            switch (timer.gameObject.tag)
            {
                case "AttackCD":
                    if (devMode)
                    {
                        timer.fillAmount -= Time.deltaTime * 1;
                    } else
                    {
                        timer.fillAmount -= Time.deltaTime / (Attack_CD * 2);
                    }
                    break;
                case "DefendCD":
                    if (devMode)
                    {
                        timer.fillAmount -= Time.deltaTime * 1;
                    }
                    else
                    {
                        timer.fillAmount -= Time.deltaTime / (Shield_CD * 4.5f);
                    }
                    break;
                case "DodgeCD":
                    if (devMode)
                    {
                        timer.fillAmount -= Time.deltaTime * 1;
                    }
                    else
                    {
                        timer.fillAmount -= Time.deltaTime / (Dodge_CD * 4.5f);
                    }
                    break;
                case "FocusCD":
                    if (devMode)
                    {
                        timer.fillAmount -= Time.deltaTime * 1;
                    }
                    else
                    {
                        timer.fillAmount -= Time.deltaTime / (Focus_CD * 4);
                    }
                    break;
            }
        }
    }

    void animateIcon(Transform icon)
    {
        Vector2 scale = new Vector2(0.2f, 0.2f);
        int vrb = 8;
        float duration = 0.3f;
        float elastic = 1f;
        Tween punch = icon.transform.DOPunchScale(scale, duration, vrb, elastic).Play().OnComplete(() => Input_Manager.SetCanClick(true));
    }

    public void selectIcon(string icon)
    {
        switch (icon)
        {
            case "ATK1":
                actionIcon.sprite = iconSprites[0];
                animateIcon(actionIcon.transform);
                break;
            case "DF":
                actionIcon.sprite = iconSprites[3];
                animateIcon(actionIcon.transform);
                break;
            case "DG":
                actionIcon.sprite = iconSprites[2];
                animateIcon(actionIcon.transform);
                break;
            case "FC":
                actionIcon.sprite = iconSprites[4];
                animateIcon(actionIcon.transform);
                break;
            case "RST":
                actionIcon.sprite = iconSprites[5];
                animateIcon(actionIcon.transform);
                break;
            case "Default":
                actionIcon.sprite = iconSprites[1];
                animateIcon(actionIcon.transform);
                break;
        }
    }

    public void defaultAction()
    {
        Input_Manager.SetPlayerAction("none");
        actionIcon.sprite = iconSprites[1];

        fadeInUnitTimer();
    }

    private void runTimer()
    {
        if (timerIsRunning)
        {
            battleTimer -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(battleTimer).ToString("D2");

            
            if (battleTimer <= 0)
            {
                battleTimer = 0;

                timerIsRunning = false;
                playerTimerControl = false;
                enemyTimerControl = false;
            }
        }
    }

    // Utilities
    public void fadeOutUnitTimer()
    {
        float fadeTime = 0.05f;

        actionIcon.DOFade(0, fadeTime);
        actionEnemyIcon.DOFade(0, fadeTime);

        playerRing.DOFade(0, fadeTime);
        EnemyRing.DOFade(0, fadeTime);
    }
    public void fadeInUnitTimer()
    {
        float fadeTime = 0.05f;

        actionEnemyIcon.DOFade(1, fadeTime);
        actionIcon.DOFade(1, fadeTime);

        playerRing.DOFade(1, fadeTime);
        EnemyRing.DOFade(1, fadeTime);
    }

    public void executeSlowMotion(float seconds, float scale)
    {
        StartCoroutine(slowMotion(seconds, scale));
    }

    IEnumerator slowMotion(float seconds, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 1;
    }
}

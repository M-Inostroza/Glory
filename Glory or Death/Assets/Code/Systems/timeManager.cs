using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;


public class TimeManager : MonoBehaviour
{
    public Sprite[] iconSprites;

    public Image playerRing;
    public Image EnemyRing;

    private BattleSystem BS;
    private Input_Manager Input_Manager;
    private CombatManager combarUI;
    private EndManager EndManager;
    private AudioManager audioManager;
    private cameraManager _cameraManager;

    //Global time
    public TextMeshProUGUI timerText;
    public float battleTimer = 94f;
    public float defaultFightTime = 94f;
    private static bool timerIsRunning = false;
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
    [SerializeField] GameObject _rushEffect;

    // Enemy Timer
    public bool enemyTimerControl = true;
    public Image enemyTimer;
    public Image enemyActionIcon;

    bool dirtPrevious = false;
    private float dirtChance = 15;

    //Generic wait time for turns
    private float mainWaitTime = 20;

    // Units
    private Player player;
    private Enemy enemy;

    // Stamina Costs
    int costATK = 25;
    int costATK2 = 60;
    int costDF = 20;
    int costDG = 30;
    int costFC = 35;
    public int CostFC
    {
        get { return costFC; }
        set { costFC = value; }
    }

    [SerializeField] TMP_Text costATKText;
    [SerializeField] TMP_Text costDFText;
    [SerializeField] TMP_Text costDGText;
    [SerializeField] TMP_Text costFCText;
    [SerializeField] TMP_Text costATK2Text;

    public bool enemyIconVisible = false;

    //Manages general timer
    private void Start()
    {
        SetActionCost();

        BS = FindObjectOfType<BattleSystem>();
        Input_Manager = FindObjectOfType<Input_Manager>();
        combarUI = FindObjectOfType<CombatManager>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        EndManager = FindObjectOfType<EndManager>();
        audioManager = FindObjectOfType<AudioManager>();
        _cameraManager = FindObjectOfType<cameraManager>();
        timerIsRunning = false;

        selectEnemyAction();
    }

    private void Update()
    {
        playerAction();
        enemyAction();
        runTimer();
    }


    public void SetActionCost()
    {
        costATKText.text = costATK.ToString();
        costDFText.text = costDF.ToString();
        costDGText.text = costDG.ToString();
        costFCText.text = costFC.ToString();
        costATK2Text.text = costATK2.ToString();
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
                    if (player.GetCurrentStamina() > costATK)
                    {
                        enemyTimer.fillAmount += 0.02f;
                        BS.PlayerAttack();
                        Input_Manager.GetAttackCD().fillAmount = 1;
                        player.DecrementCurrentStamina(costATK);
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;
                case "ATK2":
                    if (player.GetCurrentStamina() > costATK2)
                    {
                        enemyTimer.fillAmount += 0.02f;
                        BS.PlayerSuperAttack();
                        player.DecrementCurrentStamina(costATK2);
                        fadeOutUnitTimer();
                    }
                    else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "DF":
                    if (player.GetCurrentStamina() > costDF)
                    {
                        enemyTimer.fillAmount += 0.02f;
                        BS.PlayerDefend();
                        Input_Manager.GetDefendCD().fillAmount = 1;
                        player.DecrementCurrentStamina(costDF);
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "DG":
                    if (player.GetCurrentStamina() > costDG)
                    {
                        enemyTimer.fillAmount += 0.02f;
                        BS.PlayDodge();
                        Input_Manager.GetDodgeCD().fillAmount = 1;
                        player.DecrementCurrentStamina(costDG);
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "FC":
                    if (player.GetCurrentStamina() > costFC)
                    {
                        enemyTimer.fillAmount += 0.02f;
                        BS.PlayFocus();
                        Input_Manager.GetFocusCD().fillAmount = 1;
                        player.DecrementCurrentStamina(costFC);
                        fadeOutUnitTimer();
                    } else
                    {
                        continueUnitTimer();
                        combarUI.alarmStamina();
                    }
                    break;

                case "RST":
                    enemyTimer.fillAmount += 0.02f;
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
        timerIsRunning = true;
        BattleSystem.OnSkill = false;
    }

    public void stopUnitTimer()
    {
        playerTimerControl = false;
        enemyTimerControl = false;
        BattleSystem.OnSkill = true;
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
            playerTimer.fillAmount += 0.05f;
            if (enemyActionIcon.color.a == 1)
            {
                fadeOutUnitTimer();
            }

            // Execute action
            switch (Input_Manager.GetEnemyAction())
            {
                case "ATK1":
                    BS.EnemyTurn_attack();
                    break;

                case "ATK2":
                    BS.EnemyTurn_SuperAttack();
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

    public void selectEnemyAction()
    {
        if (enemy.currentHP < (enemy.maxHP / 2) && enemy.getAngryState() == false)
        {
            dirtPrevious = false;
            dirtChance += 3;
            enemy.setAngryState(true);
            Input_Manager.SetEnemyAction("RAGE");
            enemyActionIcon.sprite = iconSprites[7];
        }
        else
        {
            if (enemy.GetCurrentAdrenaline() >= 20)
            {
                Input_Manager.SetEnemyAction("ATK2");
                enemyActionIcon.sprite = iconSprites[8];
            }
            else
            {
                float attackRandom = UnityEngine.Random.Range(0, 99);
                if (attackRandom > dirtChance || dirtPrevious)
                {
                    Input_Manager.SetEnemyAction("ATK1");
                    enemyActionIcon.sprite = iconSprites[0];
                    dirtPrevious = false;
                    dirtChance += 5;
                }
                else
                {
                    Input_Manager.SetEnemyAction("DIRT");
                    enemyActionIcon.sprite = iconSprites[6];
                    dirtPrevious = true;
                    dirtChance = 5;
                }
            }
        }
    }
    // Cooldown timer (Mejorable!!)
    public float attackFactorCD = 2;
    public float defendFactorCD = 4.5f;
    public float dodgeFactorCD = 4.5f;
    public float focusFactorCD = 4;
    public void ReduceCooldown(Image timer)
    {
        if (timer.fillAmount > 0)
        {
            switch (timer.gameObject.tag)
            {
                case "AttackCD":
                    timer.fillAmount -= Time.deltaTime / (Attack_CD * attackFactorCD);
                    break;
                case "DefendCD":
                    timer.fillAmount -= Time.deltaTime / (Shield_CD * defendFactorCD);
                    break;
                case "DodgeCD":
                    timer.fillAmount -= Time.deltaTime / (Dodge_CD * dodgeFactorCD);
                    break;
                case "FocusCD":
                    timer.fillAmount -= Time.deltaTime / (Focus_CD * focusFactorCD);
                    break;
            }
        }
    }

    public static void animateIcon(Transform icon)
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
                playerActionIcon.sprite = iconSprites[0];
                animateIcon(playerActionIcon.transform);
                break;
            case "ATK2":
                playerActionIcon.sprite = iconSprites[8];
                animateIcon(playerActionIcon.transform);
                break;
            case "DF":
                playerActionIcon.sprite = iconSprites[3];
                animateIcon(playerActionIcon.transform);
                break;
            case "DG":
                playerActionIcon.sprite = iconSprites[2];
                animateIcon(playerActionIcon.transform);
                break;
            case "FC":
                playerActionIcon.sprite = iconSprites[4];
                animateIcon(playerActionIcon.transform);
                break;
            case "RST":
                playerActionIcon.sprite = iconSprites[5];
                animateIcon(playerActionIcon.transform);
                break;
            case "Default":
                playerActionIcon.sprite = iconSprites[1];
                animateIcon(playerActionIcon.transform);
                break;
        }
    }

    public void defaultAction()
    {
        Input_Manager.SetPlayerAction("none");
        playerActionIcon.sprite = iconSprites[1];
        fadeInUnitTimer();
    }

    private void runTimer()
    {
        if (timerIsRunning)
        {
            battleTimer -= Time.unscaledDeltaTime;
            timerText.text = Mathf.RoundToInt(battleTimer).ToString("D2");
            if (battleTimer <= 0)
            {
                battleTimer = 0;
            }
            // Time out
            if (battleTimer <= 0 && !BattleSystem.OnSkill)
            {
                audioManager.Stop("Combat_Theme");
                CombatManager.move_UI_out();
                timerIsRunning = false;
                stopUnitTimer();
                battleTimer = 0;
                _cameraManager.playChrome();
                slowMotion(2, 0.1f);
                EndManager.StartCoroutine(EndManager.showEndScreen(1));
            }
        }
    }

    public void ResetTimers() // Resets all timers
    {
        resetFightTimer((int)defaultFightTime);
        ActivateFightTimer();

        resetPlayerTimer();
        resetEnemyTimer();
        continueUnitTimer();
    }

    // Utilities

    public void RushEffect(bool activate)
    {
        _rushEffect.SetActive(activate);
    }
    public void fadeOutUnitTimer()
    {
        float fadeTime = 0.05f;

        playerActionIcon.DOFade(0, fadeTime);
        enemyActionIcon.DOFade(0, fadeTime);

        playerRing.DOFade(0, fadeTime);
        EnemyRing.DOFade(0, fadeTime);
    }
    public void fadeInUnitTimer()
    {
        float fadeTime = 0.05f;

        enemyActionIcon.DOFade(1, fadeTime);
        playerActionIcon.DOFade(1, fadeTime);

        playerRing.DOFade(1, fadeTime);
        EnemyRing.DOFade(1, fadeTime);
    }

    public static IEnumerator slowMotion(float seconds, float timeScale, Action afterWait = null)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 1;
        if (afterWait != null)
        {
            afterWait.Invoke();
        }
    }

    // G & S
    public void ActivateFightTimer()
    {
        timerIsRunning = true;
    }
    public static void DeactivateFightTimer()
    {
        timerIsRunning = false;
    }
    public void resetFightTimer(int seconds)
    {
        battleTimer = seconds;
    }
    public void resetPlayerTimer()
    {
        playerTimer.fillAmount = 1;
    }
    public void resetEnemyTimer()
    {
        enemyTimer.fillAmount = 1;
    }
}

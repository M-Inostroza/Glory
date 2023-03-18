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

    //Battlesystem
    private BattleSystem BS;

    //Game Manager
    private gameManager GM;

    //-----------------------------------------------------------------------------dev-----
    private bool devMode = false;

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

    //Generic wait time for turns
    private float mainWaitTime = 20;

    // Fighting units
    private Player player;
    private Enemy enemy;


    //Manages general timer
    private void Start()
    {
        BS = FindObjectOfType<BattleSystem>();
        GM = FindObjectOfType<gameManager>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        timerIsRunning = true;
    }

    private void Update()
    {
        playerAction();
        enemyAction();
        runTimer();
    }


    //Manages action execution
    void playerAction()
    {
        if (playerTimerControl)
        {
            if (devMode)
            {
                playerTimer.fillAmount -= Time.deltaTime * 1;
            } 
            else
            {
                playerTimer.fillAmount -= Time.deltaTime / (mainWaitTime - player.baseSpeed);
            }
        }

        //Execute selected action
        if (playerTimer.fillAmount == 0 && playerTimerControl)
        {
            switch (BS.GetPlayerAction())
            {
                case "ATK1":
                    if (player.currentStamina > 25)
                    {
                        BS.PlayerAttack();
                        actionIcon.DOFade(0, 0.3f);
                        BS.GetAttackCD().fillAmount = 1;
                        playerTimerControl = false;
                        enemyTimerControl = false;
                        player.currentStamina -= 25;
                    } else
                    {
                        setDefaultAction();
                    }
                    break;

                case "DF":
                    if (player.currentStamina > 20)
                    {
                        BS.PlayerDefend();
                        actionIcon.DOFade(0, 0.3f);
                        BS.GetDefendCD().fillAmount = 1;
                        playerTimerControl = false;
                        enemyTimerControl = false;
                        player.currentStamina -= 20;
                    } else
                    {
                        setDefaultAction();
                    }
                    break;

                case "DG":
                    if (player.currentStamina > 30)
                    {
                        BS.PlayDodge();
                        actionIcon.DOFade(0, 0.3f);
                        BS.GetDodgeCD().fillAmount = 1;
                        playerTimerControl = false;
                        enemyTimerControl = false;
                        player.currentStamina -= 30;
                    } else
                    {
                        setDefaultAction();
                    }
                    break;

                case "FC":
                    if (player.currentStamina > 25)
                    {
                        BS.PlayFocus();
                        actionIcon.DOFade(0, 0.3f);
                        BS.GetFocusCD().fillAmount = 1;
                        playerTimerControl = false;
                        enemyTimerControl = false;
                        player.currentStamina -= 25;
                    } else
                    {
                        setDefaultAction();
                    }
                    break;

                default:
                    setDefaultAction();
                    break;
            }
        }
    }
    void setDefaultAction()
    {
        playerTimer.fillAmount = 1;
        playerTimerControl = true;
        enemyTimerControl = true;
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
        //Execute selected action
        if (enemyTimer.fillAmount == 0 && enemyTimerControl)
        {
            playerTimerControl = false;
            enemyTimerControl = false;

            // Select action
            BS.SetEnemyAction("ATK1");

            // Execute action
            switch (BS.GetEnemyAction())
            {
                case "ATK1":
                    BS.EnemyTurn_attack();
                    break;

                case "CHR":
                    Debug.Log("charging");
                    break;

                default:
                    enemyTimer.fillAmount = 1;
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
                        timer.fillAmount -= Time.deltaTime / (Focus_CD * 3);
                    }
                    break;
            }
        }
    }

    void animateIcon(Transform icon)
    {
        Vector2 scale = new Vector2(0.1f, 0.1f);
        int vrb = 8;
        float duration = 0.4f;
        float elastic = 1f;
        Tween punch = icon.transform.DOPunchScale(scale, duration, vrb, elastic).Play().OnComplete(() => BS.SetCanClick(true));
    }

    public void selectIcon(string icon)
    {
        //Changes icon for selected action
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
        }
    }

    public void defaultAction()
    {
        BS.SetPlayerAction("none");
        actionIcon.sprite = iconSprites[1];
        actionIcon.DOFade(1, 0.3f);
    }

    private void runTimer()
    {
        if (timerIsRunning)
        {
            // Update the time remaining
            battleTimer -= Time.deltaTime;
            // Update the timer text
            timerText.text = Mathf.RoundToInt(battleTimer).ToString("D2");
            // Check if the timer has reached 0
            if (battleTimer <= 0)
            {
                // Stop the timer
                battleTimer = 0;
                timerIsRunning = false;
                playerTimerControl = false;
                enemyTimerControl = false;

                GM.showSummery();
            }
        }
    }
}

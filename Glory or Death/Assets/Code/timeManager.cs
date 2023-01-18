using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
    public BattleSystem BS;

    //Global time
    public TextMeshProUGUI timerText;
    public float duration = 60f;
    public bool timeOut;

    //Cooldowns
    public float Shield_CD = 10;
    public float Attack_CD = 10;
    public float Dodge_CD = 10;
    public float Focus_CD = 10;

    // Player Timer
    private bool playerTimerControl = true;
    public Image playerTimer;
    public Image playerActionIcon;

    private bool enemyTimerControl = true;
    public Image enemyTimer;

    //Generic wait time for turns
    private float mainWaitTime = 20;

    // Fighting units
    private Player player;
    public bool can_perform_player = true;

    private Enemy enemy;
    public bool can_perform_enemy = true;




    //Manages general timer
    IEnumerator Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();

        float elapsedTime = 0f;
        int timer = 60;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            timer = Mathf.RoundToInt(Mathf.Lerp(60, 0, elapsedTime / duration));
            timerText.text = timer.ToString();
            yield return null;
        }
        timerText.text = "0";

        timeOut = true;
    }

    private void Update()
    {
        playerAction();
        enemyAction();
    }


    //Manages action execution
    void playerAction()
    {
        if (playerTimerControl)
        {
            playerTimer.fillAmount -= Time.deltaTime / (mainWaitTime - player.baseSpeed);
        }

        //Changes icon for selected action
        switch (BS.selectedPlayerAction)
        {
            case "ATK1":
                actionIcon.sprite = iconSprites[0];
                break;
            case "DF":
                actionIcon.sprite = iconSprites[3];
                break;
            case "DG":
                actionIcon.sprite = iconSprites[2];
                break;
            case "FC":
                actionIcon.sprite = iconSprites[4];
                break;

        }

        //Execute selected action
        if (playerTimer.fillAmount == 0 && can_perform_player)
        {
            enemyTimerControl = false;
            switch (BS.selectedPlayerAction)
            {
                case "ATK1":
                    BS.PlayerAttack();
                    BS.AttackButtonCD.fillAmount = 1;
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;

                case "DF":
                    BS.PlayerDefend();
                    BS.DefendButtonCD.fillAmount = 1;
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;

                case "DG":
                    BS.PlayDodge();
                    BS.DodgeButtonCD.fillAmount = 1;
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;

                case "FC":
                    BS.PlayFocus();
                    BS.FocusButtonCD.fillAmount = 1;
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;


                default:
                    playerTimer.fillAmount = 1;
                    break;
            }
        }
    }

    void enemyAction()
    {
        if (enemyTimerControl)
        {
            enemyTimer.fillAmount -= Time.deltaTime / (mainWaitTime - enemy.baseSpeed);
        }
        //Execute selected action
        if (enemyTimer.fillAmount == 0)
        {
            playerTimerControl = false;
            switch (BS.selectedEnemyAction)
            {
                case "ATK1":
                    BS.EnemyTurn_attack();
                    break;

                default:
                    playerTimer.fillAmount = 1;
                    break;
            }
        }
    }


    // Cooldown timer
    public void ReduceCooldown(Image timer)
    {
        if (timer.fillAmount > 0)
        {
            switch (timer.gameObject.tag)
            {
                case "ATK Counter":
                    timer.fillAmount -= Time.deltaTime / (Attack_CD * 3);
                    break;
                case "DF Counter":
                    timer.fillAmount -= Time.deltaTime / (Shield_CD * 3.5f);
                    break;
                case "DG Counter":
                    timer.fillAmount -= Time.deltaTime / (Dodge_CD * 3.5f);
                    break;
                case "FC Counter":
                    timer.fillAmount -= Time.deltaTime / (Focus_CD * 4);
                    break;
            }
        }
    }
}

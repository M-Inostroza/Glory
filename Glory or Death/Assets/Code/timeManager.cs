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
    [SerializeField] private float DefendButtonCD;
    public float Shield_CD = 7;
    public float Attack_CD = 7;
    public float Dodge_CD = 7;
    public float Focus_CD = 7;

    // Player Timer
    public Image playerTimer;
    public Image playerActionIcon;

    public Image enemyTimer;

    //Generic wait time for turns
    private float mainWaitTime = 6;

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
    }


    //Manages action execution
    void playerAction()
    {
        if (BS.state == BattleState.PLAYERTURN)
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
            switch (BS.selectedPlayerAction)
            {
                case "ATK1":
                    BS.PlayerAttack();
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;

                case "DF":
                    BS.PlayerDefend();
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;

                case "DG":
                    BS.PlayDodge();
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;

                case "FC":
                    BS.PlayFocus();
                    BS.selectedPlayerAction = "None";
                    can_perform_player = false;
                    actionIcon.sprite = iconSprites[1];
                    break;


                default:
                    playerTimer.fillAmount = 1;
                    BS.switchToEnemy();
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
                    timer.fillAmount -= Time.deltaTime / (Attack_CD * 2);
                    break;
                case "DF Counter":
                    timer.fillAmount -= Time.deltaTime / (Shield_CD * 3);
                    break;
                case "DG Counter":
                    timer.fillAmount -= Time.deltaTime / (Dodge_CD * 3);
                    break;
                case "FC Counter":
                    timer.fillAmount -= Time.deltaTime / (Focus_CD * 3);
                    break;
            }
        }
    }
}

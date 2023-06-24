using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Combat_UI : MonoBehaviour
{
    [SerializeField]
    private Transform player_stats, enemy_stats, player_stamina, player_timer, enemy_timer;

    [SerializeField]
    private GameObject enemy_DMG_Feedback, enemy_SPEED_Feedback; // Enemy
    [SerializeField]
    private GameObject player_DMG_Feedback, player_SPEED_Feedback; // Player

    [SerializeField] 
    private Slider playerHpSlider, playerAdrenalineSlider, staminaSlider, shieldBar;

    [SerializeField]
    private GameObject inputManager, globalTimer, staminaAlarm, shieldFeedback;

    //Unit sliders
    [SerializeField]
    private Player playerUnit;
    [SerializeField]
    private Enemy enemyUnit;

    private void Start()
    {
        staminaSlider.DOValue(staminaSlider.maxValue, 1.5f);
    }
    private void OnEnable()
    {
        move_UI_in();
    }
    private void Update()
    {
        updateShieldBar();
        refillStamina();
    }
    public void setPlayerHP(int hp)
    {
        playerHpSlider.DOValue(hp, 0.5f);
    }

    public void move_UI_in()
    {
        float move_in_speed = 0.7f;

        player_stats.DOLocalMoveX(player_stats.localPosition.x + 350, move_in_speed).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x - 350, move_in_speed).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x + 200, move_in_speed).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y - 160, move_in_speed);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y - 160, move_in_speed);

        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x + 80, move_in_speed).SetEase(Ease.InOutSine);
        globalTimer.transform.DOLocalMoveY(globalTimer.transform.localPosition.y - 80, move_in_speed).SetEase(Ease.InOutSine);
    }
    public void move_UI_out()
    {
        float move_out_speed = 0.7f;

        player_stats.DOLocalMoveX(player_stats.localPosition.x - 340, move_out_speed).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x + 340, move_out_speed).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x - 180, move_out_speed).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y + 160, move_out_speed);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y + 160, move_out_speed);

        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x - 80, move_out_speed).SetEase(Ease.InOutSine);
        globalTimer.transform.DOLocalMoveY(globalTimer.transform.localPosition.y + 80, move_out_speed).SetEase(Ease.InOutSine);
    }

    public void move_Inputs_in()
    {
        float move_in_speed = 0.7f;
        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x + 80, move_in_speed);
    }
    public void move_Inputs_out()
    {
        float move_out_speed = 0.7f;
        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x - 80, move_out_speed);
    }

    // Stamina
    void refillStamina()
    {
        if (playerUnit.currentStamina < playerUnit.maxStamina)
        {
            playerUnit.currentStamina += 0.5f * Time.deltaTime; //Mejorable
        }

        if (playerUnit.currentStamina < 30)
        {
            inputManager.GetComponent<Input_Manager>().GetRestButton().transform.DOLocalMoveX(55, 0.7f);
        }
        else
        {
            inputManager.GetComponent<Input_Manager>().GetRestButton().transform.DOLocalMoveX(-40, 0.7f);
        }
    }

    private bool hasPlayed = false;
    public void alarmStamina()
    {
        if (!hasPlayed)
        {
            fadeON();
            player_stamina.transform.DOShakePosition(0.6f, 4, 50);
            staminaAlarm.transform.DOShakePosition(0.6f, 4, 50).OnComplete(() => fadeOFF());
            hasPlayed = true;
        }
        
        void fadeON()
        {
            foreach (Transform child in staminaAlarm.transform)
            {
                child.GetComponent<Image>().DOFade(1, 0.2f);
            }
        }
        void fadeOFF()
        {
            foreach (Transform child in staminaAlarm.transform)
            {
                child.GetComponent<Image>().DOFade(0, 0.2f);
            }
            hasPlayed = false;
        }
    }
    public Slider GetStaminaSlider()
    {
        return staminaSlider;
    }

    // Shield
    private bool shieldFeedControl = false;
    public void shieldFeed()
    {
        if (!shieldFeedControl)
        {
            fadeON();
            shieldFeedControl = true;
            shieldFeedback.transform.DOLocalMoveY(140, 0.8f).OnComplete(()=> fadeOFF());
        }

        void fadeON()
        {
            foreach (Transform child in shieldFeedback.transform)
            {
                child.GetComponent<Image>().DOFade(1, 0.2f);
            }
        }
        void fadeOFF()
        {
            foreach (Transform child in shieldFeedback.transform)
            {
                child.GetComponent<Image>().DOFade(0, 0.2f).OnComplete(()=> shieldFeedback.transform.DOLocalMoveY(110, 0));
            }
            shieldFeedControl = false;
        }
    }
    void updateShieldBar()
    {
        shieldBar.DOValue(playerUnit.getCurrentShield(), .5f);
    }
    public void shakeShieldBar()
    {
        shieldBar.transform.DOShakePosition(0.5f, 2.5f, 50, 90);
    }

    // Buffs
    public void damageBuff(string unit)
    {
        if (unit == "player")
        {
            player_DMG_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => player_DMG_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
            player_DMG_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

            player_DMG_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => player_DMG_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
            player_DMG_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
        } else if (unit == "enemy")
        {
            enemy_DMG_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => enemy_DMG_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
            enemy_DMG_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

            enemy_DMG_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => enemy_DMG_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
            enemy_DMG_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
        }
    }
    public void speedBuff(string unit)
    {
        if (unit == "player")
        {
            player_SPEED_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => player_SPEED_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
            player_SPEED_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

            player_SPEED_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => player_SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
            player_SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
        } else if (unit == "enemy")
        {
            enemy_SPEED_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => enemy_SPEED_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
            enemy_SPEED_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

            enemy_SPEED_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => enemy_SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
            enemy_SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
        }
        
    }

    // Stamina
    public Slider GetPlayerAdrenalineSlider()
    {
        return playerAdrenalineSlider;
    }

    // Test
    public void reduceStamina()
    {
        playerUnit.currentStamina = 10;
    }
}

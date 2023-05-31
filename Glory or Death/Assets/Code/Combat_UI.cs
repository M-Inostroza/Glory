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
    private GameObject DMG_Feedback, SPEED_Feedback;

    [SerializeField]
    private GameObject shieldManager, inputManager, globalTimer, staminaAlarm;

    //Unit sliders
    [SerializeField]
    private Player playerUnit;
    [SerializeField]
    private Enemy enemyUnit;

    private void OnEnable()
    {
        move_UI_in();
    }
    private void Update()
    {
        refillStamina();
    }

    public void move_UI_in()
    {
        float move_in_speed = 0.7f;

        player_stats.DOLocalMoveX(player_stats.localPosition.x + 350, move_in_speed).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x - 350, move_in_speed).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x + 200, move_in_speed).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y - 160, move_in_speed);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y - 160, move_in_speed);

        inputManager.transform.DOLocalMoveX(inputManager.transform.localPosition.x + 70, move_in_speed).SetEase(Ease.InOutSine);
        globalTimer.transform.DOLocalMoveY(globalTimer.transform.localPosition.y - 70, move_in_speed).SetEase(Ease.InOutSine);
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

    public void damageBuff()
    {
        DMG_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => DMG_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
        DMG_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

        DMG_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => DMG_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
        DMG_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
    }
    public void speedBuff()
    {
        SPEED_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => SPEED_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
        SPEED_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

        SPEED_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
        SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
    }

    // Test

    public void reduceStamina()
    {
        playerUnit.currentStamina = 10;
    }
}

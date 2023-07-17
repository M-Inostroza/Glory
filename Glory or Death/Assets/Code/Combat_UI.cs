using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using AssetKits.ParticleImage;

public class Combat_UI : MonoBehaviour
{
    [Header("--Stats--")]
    [SerializeField] Transform player_stats;
    [SerializeField] Transform enemy_stats;
    [SerializeField] Transform player_stamina;
    [SerializeField] Transform star_counter;

    [Header("--Stars--")]
    [SerializeField] int stars;
    [SerializeField] Transform starsTransformSide;
    [SerializeField] TMP_Text starsText;

    [Header("--Timers--")]
    [SerializeField] Transform player_timer;
    [SerializeField] Transform enemy_timer;
    [SerializeField] Transform fightTimer;

    [Header("--Feedbacks--")]
    [SerializeField] GameObject enemy_DMG_Feedback;
    [SerializeField] GameObject enemy_SPEED_Feedback;
    [SerializeField] GameObject player_DMG_Feedback;
    [SerializeField] GameObject player_SPEED_Feedback;
    [SerializeField] GameObject staminaAlarm;
    [SerializeField] GameObject shieldFeedback;

    [Header("--Sliders--")]
    [SerializeField] Slider playerHpSlider;
    [SerializeField] Slider playerAdrenalineSlider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Slider shieldBar;

    [Header("--Input--")]
    [SerializeField] Transform inputManager;

    [Header("--Input keys--")]
    [SerializeField] Transform xKey;
    [SerializeField] Transform sKey;
    [SerializeField] Transform aKey;

    [Header("--Sliders--")]
    [SerializeField] Player playerUnit;
    [SerializeField] Enemy enemyUnit;

    [Header("--Materials--")]
    [SerializeField] Material swordBuffMaterial;
    [SerializeField] Material arrowBuffMaterial;

    [Header("--Debug--")]
    [SerializeField] TMP_Text hpPlayerDebug;


    [Header("--End--")]
    [SerializeField] Transform endScreen;
    [SerializeField] Image endOverlay;
    [SerializeField] TMP_Text endStarCount;
    [SerializeField] ParticleImage starParticle;
    [SerializeField] Transform endStarSymbol;

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
        hpPlayerDebug.text = "HP: " + playerUnit.GetCurrentHP();
        starsText.text = "= " + stars.ToString();
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
        fightTimer.DOLocalMoveY(202, move_in_speed).SetEase(Ease.InOutSine);

        inputManager.transform.DOLocalMoveX(-435, move_in_speed).SetEase(Ease.InOutSine);
    }
    public void move_UI_out()
    {
        float move_out_speed = 0.7f;

        player_stats.DOLocalMoveX(player_stats.localPosition.x - 350, move_out_speed).SetEase(Ease.InOutSine);
        enemy_stats.DOLocalMoveX(enemy_stats.localPosition.x + 350, move_out_speed).SetEase(Ease.InOutSine);

        player_stamina.DOLocalMoveX(player_stamina.localPosition.x - 200, move_out_speed).SetEase(Ease.InOutSine);

        player_timer.DOLocalMoveY(player_timer.localPosition.y + 160, move_out_speed);
        enemy_timer.DOLocalMoveY(enemy_timer.localPosition.y + 160, move_out_speed);
        fightTimer.DOLocalMoveY(240, move_out_speed).SetEase(Ease.InOutSine);

        inputManager.transform.DOLocalMoveX(-500, move_out_speed).SetEase(Ease.InOutSine);
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
        if (playerUnit.GetCurrentStamina() < playerUnit.GetMaxStamina())
        {
            playerUnit.IncrementCurrentStamina(0.5f * Time.deltaTime);  //Mejorable
        }

        if (playerUnit.GetCurrentStamina() < 30)
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
            shineBuffs();
            shieldFeedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => fadeOFF());
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
                child.GetComponent<Image>().DOFade(0, 0.2f).OnComplete(() => shieldFeedback.transform.DOLocalMoveY(65, 0));
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
        var playerImgDmg = player_DMG_Feedback.GetComponent<Image>();
        var child = player_DMG_Feedback.transform.GetChild(0);
        if (unit == "player")
        {
            playerImgDmg.DOFade(1, 0);
            player_DMG_Feedback.transform.DOLocalMoveY(65, 0);
            shineBuffs();
            player_DMG_Feedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => playerImgDmg.DOFade(0, 0.5f));

            child.transform.DOLocalMoveY(0, 0.8f).OnComplete(() => child.GetComponent<Image>().DOFade(0, 0.5f));
            child.GetComponent<Image>().DOFade(1, 0.5f);
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
        var playerImgSpeed = player_SPEED_Feedback.GetComponent<Image>();
        var child = player_SPEED_Feedback.transform.GetChild(0);
        if (unit == "player")
        {
            playerImgSpeed.DOFade(1, 0);
            player_SPEED_Feedback.transform.DOLocalMoveY(65, 0);
            shineBuffs();
            player_SPEED_Feedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => playerImgSpeed.DOFade(0, 0.5f));

            child.transform.DOLocalMoveY(0, 0.8f).OnComplete(() => child.GetComponent<Image>().DOFade(0, 0.5f));
            child.GetComponent<Image>().DOFade(1, 0.5f);
        } else if (unit == "enemy")
        {
            enemy_SPEED_Feedback.transform.DOLocalMoveY(150, 0.8f).OnComplete(() => enemy_SPEED_Feedback.GetComponent<Image>().DOFade(0, 0.5f));
            enemy_SPEED_Feedback.GetComponent<Image>().DOFade(1, 0.3f);

            enemy_SPEED_Feedback.transform.GetChild(0).transform.DOLocalMoveY(0, 1f).OnComplete(() => enemy_SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f));
            enemy_SPEED_Feedback.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
        }
    }

    // Keys
    public void activateX()
    {
        if (xKey.gameObject.activeInHierarchy)
        {
            xKey.DOScale(0.8f, 0.1f).SetDelay(1f);
            xKey.DOScale(1, 0.1f).SetDelay(2f).OnComplete(() => activateX());
        }
    }
    public void activateS()
    {
        if (sKey.gameObject.activeInHierarchy)
        {
            sKey.DOScale(0.8f, 0.1f).SetDelay(0.3f);
            sKey.DOScale(1, 0.1f).SetDelay(1f).OnComplete(() => activateS());
        }
    }
    public void activateA()
    {
        if (aKey.gameObject.activeInHierarchy)
        {
            aKey.DOScale(0.8f, 0.1f).SetDelay(0.3f);
            aKey.DOScale(1, 0.1f).SetDelay(1f).OnComplete(() => activateA());
        }
    }

    // Critic
    public void incrementStars()
    {
        stars++;
        Invoke("hideStars", 1);
    }
    public void showStars()
    {
        star_counter.DOLocalMoveX(-350, 1);
    }
    public void hideStars()
    {
        star_counter.DOLocalMoveX(-500, 1);
    }
    public void starPunchSide()
    {
        starsTransformSide.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => starsTransformSide.DOScale(1, 0));
    }

    // Stamina
    public Slider GetPlayerAdrenalineSlider()
    {
        return playerAdrenalineSlider;
    }

    // Test
    public void reduceStamina()
    {
        playerUnit.SetCurrentStamina(80);
    }

    // Shader UI
    public void shineBuffs()
    {
        DOTween.To(() => swordBuffMaterial.GetFloat("_ShineLocation"), x => swordBuffMaterial.SetFloat("_ShineLocation", x), 0, 0.7f).OnComplete(() => swordBuffMaterial.SetFloat("_ShineLocation", 1)).SetDelay(0.3f);
        DOTween.To(() => arrowBuffMaterial.GetFloat("_ShineLocation"), x => arrowBuffMaterial.SetFloat("_ShineLocation", x), 0, 0.7f).OnComplete(() => arrowBuffMaterial.SetFloat("_ShineLocation", 1)).SetDelay(0.3f);
    }

    // End
    public int starCounter = 0;
    public void showEndScreen()
    {
        endScreen.gameObject.SetActive(true);
        endOverlay.gameObject.SetActive(true);
        showStars();

        endOverlay.DOFade(0.5f, 1.4f).OnComplete(() => starParticle.Play());
        endScreen.DOLocalMoveY(0, 1.4f).SetEase(Ease.OutBounce);
    }
    public void addToStarCounter()
    {
        if (starCounter != stars)
        {
            starCounter++;
            endStarCount.text = starCounter.ToString();
        }
    }
    public void starPunchEnd()
    {
        endStarSymbol.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => endStarSymbol.DOScale(1, 0));
    }
}

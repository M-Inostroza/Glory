using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Combat_UI : MonoBehaviour
{
    // Stats
    private static Transform _playerStats;
    private static Transform _enemyStats;

    [Header("--Stamina--")]
    private static Transform _playerStamina;
    [SerializeField] TMP_Text staminaText;
    [SerializeField] Transform star_counter;

    [Header("--Stars--")]
    [SerializeField] int stars;
    [SerializeField] Transform starsTransformSide;
    [SerializeField] TMP_Text starsText;

    // Timers
    private static Transform _playerTimer;
    private static Transform _enemyTimer;
    private static Transform _fightTimer;

    [Header("--Feedbacks--")]
    [SerializeField] GameObject enemy_DMG_Feedback;
    [SerializeField] GameObject enemy_SPEED_Feedback;
    [SerializeField] GameObject player_DMG_Feedback;
    [SerializeField] GameObject player_SPEED_Feedback;
    [SerializeField] GameObject staminaAlarm;
    [SerializeField] GameObject shieldFeedback;
    [SerializeField] TMP_Text shieldNumber;

    [Header("--Sliders--")]
    [SerializeField] Slider playerHpSlider;
    [SerializeField] Slider playerAdrenalineSlider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Slider shieldBar;

    // Input
    private static Transform _inputManager;

    [Header("--Input keys--")]
    [SerializeField] Transform xKey;
    [SerializeField] Transform sKey;
    [SerializeField] Transform aKey;
    [SerializeField] Transform leftKey;
    [SerializeField] Transform rightKey;

    [Header("--Materials--")]
    [SerializeField] Material swordBuffMaterial;
    [SerializeField] Material arrowBuffMaterial;

    [Header("--HP--")]
    [SerializeField] TMP_Text hpPlayerDebug;
    [SerializeField] TMP_Text hpEnemyDebug;

    Player playerUnit;
    Enemy enemyUnit;
    AudioManager audioManager;

    private void Awake()
    {
        _playerStats = transform.GetChild(0).Find("Player stat HUD");
        _playerStamina = transform.GetChild(0).Find("Stamina");
        _playerTimer = transform.GetChild(0).Find("Player Timer");

        _enemyStats = transform.GetChild(1).Find("Enemy stat HUD");
        _enemyTimer = transform.GetChild(1).Find("Enemy Timer");

        _fightTimer = transform.Find("Fight Timer");
        _inputManager = transform.Find("Input Manager");
    }
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        playerUnit = FindObjectOfType<Player>();
        enemyUnit = FindObjectOfType<Enemy>();
        staminaSlider.DOValue(staminaSlider.maxValue, 1.5f);
    }
    private void OnEnable()
    {
        move_UI_in();
    }
    private void Update()
    {
        showSuperATKButton();
        updateShieldBar();
        refillStamina();
        hpTextUpdate();
        starsText.text = "= " + stars.ToString();
    }
    public void setPlayerHP(int hp)
    {
        playerHpSlider.DOValue(hp, 0.5f);
    }

    // UI
    public static void move_UI_in(bool includeInput = true)
    {
        if (!gameManager.isTutorial())
        {
            float move_in_speed = 0.3f;

            _playerStats.DOLocalMoveX(_playerStats.localPosition.x + 350, move_in_speed).SetEase(Ease.InOutSine);
            _enemyStats.DOLocalMoveX(_enemyStats.localPosition.x - 350, move_in_speed).SetEase(Ease.InOutSine);

            _playerStamina.DOLocalMoveX(_playerStamina.localPosition.x + 200, move_in_speed).SetEase(Ease.InOutSine);

            _playerTimer.DOLocalMoveY(_playerTimer.localPosition.y - 160, move_in_speed);
            _enemyTimer.DOLocalMoveY(_enemyTimer.localPosition.y - 160, move_in_speed);
            _fightTimer.DOLocalMoveY(202, move_in_speed).SetEase(Ease.InOutSine);

            if (includeInput)
            {
                _inputManager.transform.DOLocalMoveX(-435, move_in_speed).SetEase(Ease.InOutSine);
            }
        }
    }
    public static void move_UI_out()
    {
        float move_out_speed = 0.3f;

        _playerStats.DOLocalMoveX(_playerStats.localPosition.x - 350, move_out_speed).SetEase(Ease.InOutSine);
        _enemyStats.DOLocalMoveX(_enemyStats.localPosition.x + 350, move_out_speed).SetEase(Ease.InOutSine);

        _playerStamina.DOLocalMoveX(_playerStamina.localPosition.x - 200, move_out_speed).SetEase(Ease.InOutSine);

        _playerTimer.DOLocalMoveY(_playerTimer.localPosition.y + 160, move_out_speed);
        _enemyTimer.DOLocalMoveY(_enemyTimer.localPosition.y + 160, move_out_speed);
        _fightTimer.DOLocalMoveY(270, move_out_speed).SetEase(Ease.InOutSine);

        _inputManager.transform.DOLocalMoveX(-500, move_out_speed).SetEase(Ease.InOutSine);
    }
    public static void move_Inputs_in()
    {
        float move_in_speed = 0.5f;
        _inputManager.transform.DOLocalMoveX(_inputManager.transform.localPosition.x + 80, move_in_speed);
    }
    public static void move_Inputs_out()
    {
        float move_out_speed = 0.5f;
        _inputManager.transform.DOLocalMoveX(_inputManager.transform.localPosition.x - 80, move_out_speed);
    }

    // Stamina
    public void refillStamina()
    {
        staminaText.text = ((int)playerUnit.GetCurrentStamina()).ToString() + " / " + ((int)playerUnit.GetMaxStamina()).ToString();
        if (playerUnit.GetCurrentStamina() < playerUnit.GetMaxStamina())
        {
            playerUnit.IncrementCurrentStamina(0.5f * Time.deltaTime);  //Mejorable
        }

        if (playerUnit.GetCurrentStamina() < 50)
        {
            _inputManager.GetComponent<Input_Manager>().GetRestButton().transform.DOLocalMoveX(55, 0.7f);
        }
        else
        {
            _inputManager.GetComponent<Input_Manager>().GetRestButton().transform.DOLocalMoveX(-40, 0.7f);
        }
    }
    private bool hasPlayed = false;
    public void alarmStamina()
    {
        if (!hasPlayed)
        {
            fadeON();
            _playerStamina.transform.DOShakePosition(0.6f, 4, 50);
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

    // Adrenaline
    void showSuperATKButton()
    {
        if (playerUnit.GetAdrenaline() == 20)
        {
            _inputManager.GetComponent<Input_Manager>().GetSATKButton().transform.DOLocalMoveX(55, 0.7f);
        }
        else
        {
            _inputManager.GetComponent<Input_Manager>().GetSATKButton().transform.DOLocalMoveX(-40, 0.7f);
        }
    }

    // Shield
    bool shieldFeedControl = false;
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
        shieldBar.DOValue(playerUnit.getCurrentShield(), .3f);
        shieldNumber.text = playerUnit.getCurrentShield().ToString();
    }
    public void shakeShieldBar()
    {
        shieldBar.transform.DOShakePosition(0.5f, 2.5f, 50, 90);
    }

    //HP
    void hpTextUpdate()
    {
        hpPlayerDebug.text = playerUnit.GetCurrentHP().ToString() + " / " + playerUnit.GetMaxHP().ToString();
        hpEnemyDebug.text = enemyUnit.currentHP + " / " + enemyUnit.maxHP;
    }

    // Buffs
    public void damageBuff(string unit)
    {
        if (unit == "player")
        {
            var playerImgDmg = player_DMG_Feedback.GetComponent<Image>();
            var child = player_DMG_Feedback.transform.GetChild(0);

            playerImgDmg.DOFade(1, 0);
            player_DMG_Feedback.transform.DOLocalMoveY(65, 0);
            shineBuffs();
            player_DMG_Feedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => playerImgDmg.DOFade(0, 0.5f));

            child.transform.DOLocalMoveY(0, 0.8f).OnComplete(() => child.GetComponent<Image>().DOFade(0, 0.5f));
            child.GetComponent<Image>().DOFade(1, 0.5f);
        } else if (unit == "enemy")
        {
            var enemyImgDmg =  enemy_DMG_Feedback.GetComponent<Image>();
            var child = enemy_DMG_Feedback.transform.GetChild(0);

            enemyImgDmg.DOFade(1, 0);
            enemy_DMG_Feedback.transform.DOLocalMoveY(65, 0);
            shineBuffs();
            enemy_DMG_Feedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => enemyImgDmg.DOFade(0, 0.5f));

            child.transform.DOLocalMoveY(0, 0.8f).OnComplete(() => child.GetComponent<Image>().DOFade(0, 0.5f));
            child.GetComponent<Image>().DOFade(1, 0.5f);
        }
    }
    public void speedBuff(string unit)
    {
        if (unit == "player")
        {
            var playerImgSpeed = player_SPEED_Feedback.GetComponent<Image>();
            var child = player_SPEED_Feedback.transform.GetChild(0);
            playerImgSpeed.DOFade(1, 0);

            player_SPEED_Feedback.transform.DOLocalMoveY(65, 0);
            shineBuffs();
            player_SPEED_Feedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => playerImgSpeed.DOFade(0, 0.5f));

            child.transform.DOLocalMoveY(0, 0.8f).OnComplete(() => child.GetComponent<Image>().DOFade(0, 0.5f));
            child.GetComponent<Image>().DOFade(1, 0.5f);
        } else if (unit == "enemy")
        {
            var enemyImgSpeed = enemy_SPEED_Feedback.GetComponent<Image>();
            var child = enemy_SPEED_Feedback.transform.GetChild(0);

            enemyImgSpeed.DOFade(1, 0);
            enemy_SPEED_Feedback.transform.DOLocalMoveY(65, 0);
            shineBuffs();
            enemy_SPEED_Feedback.transform.DOLocalMoveY(120, 0.8f).OnComplete(() => enemyImgSpeed.DOFade(0, 0.5f));

            child.transform.DOLocalMoveY(0, 0.8f).OnComplete(() => child.GetComponent<Image>().DOFade(0, 0.5f));
            child.GetComponent<Image>().DOFade(1, 0.5f);
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
    public void activateLeftRight()
    {
        if (leftKey.gameObject.activeInHierarchy && rightKey.gameObject.activeInHierarchy)
        {
            animateLeft();
            animateRight();
            void animateLeft()
            {
                leftKey.DOScale(0.7f, 0.1f).OnComplete(animateRight);
                rightKey.DOScale(0.8f, 0.1f);
            }
            void animateRight()
            {
                rightKey.DOScale(0.7f, 0.1f).OnComplete(animateLeft);
                leftKey.DOScale(0.8f, 0.1f);
            }
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
        audioManager.Play("Star_Shimes_3");
        star_counter.DOLocalMoveX(-350, 0.5f);
    }
    public void hideStars()
    {
        star_counter.DOLocalMoveX(-500, 0.5f);
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

    // G & S 
    public int GetStars()
    {
        return stars;
    }
    public void removeStar()
    {
        stars--;
    }
}

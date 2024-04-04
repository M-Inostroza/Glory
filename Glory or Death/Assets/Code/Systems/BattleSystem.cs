using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    GameManager GameManager;
    TargetManager targetManager;
    DefendManager DefendManager;
    TimeManager TimeManager;
    InputManager InputManager;
    CombatManager CombatManager;
    EndManager EndManager;
    cameraManager cameraManager;
    AudioManager AudioManager;
    LoadingScreen LoadingScreen;

    [SerializeField] GameObject _loadingScreen;
    [SerializeField] Transform playerPanel;
    [SerializeField] Transform enemyPanel;

    [Header("Mechanics")]
    [SerializeField] GameObject dodgeManager;
    public GameObject _focusManager;
    public GameObject restManager;
    public GameObject superAttackManager;

    [Header("Scores")]
    public int targetHit;
    public bool canBloomAttack = true;

    [Header("Units")]
    public GameObject enemyPrefab;
    private Player playerUnit;
    private Enemy enemyUnit;
    bool deadPlayer = false;
    bool deadEnemy = false;


    // Animators
    private Animator _playerAnimator;

    // UI
    public EnemyHUD enemyHUD;

    [Header("Text elements")]
    public GameObject missText;
    public GameObject hitText_Enemy;
    public GameObject hitText_Player;
    public GameObject missText_Player;

    //Info Hud
    public GameObject infoHud;
    public GameObject infoHud_EN;

    // Pause control
    public static bool IsPaused { get; set; } = false;
    public static bool OnSkill { get; set; } = false;

    private void Awake()
    {
        GetScripts();

        TimeManager.stopUnitTimer();
        OpenPanels();
    }

    private void Update()
    {
        checkBloom();
        updateUI();
        CheckEndFight();
    }

    // ----------------- Actions -----------------

    public void PlayerAttack()
    {
        targetManager.attack();
        _playerAnimator.Play("ATK_jump");
    }
    public void PlayerSuperAttack()
    {
        superAttackManager.SetActive(true);
        playerUnit.reduceAdrenaline(20);
    }

    public void PlayerDefend()
    {
        DefendManager.activateShieldMinigame();
    }
    public void PlayDodge()
    {
        dodgeManager.SetActive(true);
    }
    public void PlayFocus()
    {
        _focusManager.SetActive(true);
    }

    public void PlayRest()
    {
        restManager.SetActive(true);
        InputManager.SetPlayerAction("none");
        TimeManager.selectIcon("Default");
    }


    // ------------------------Enemy turn------------------------
    public void EnemyTurn_attack()
    {
        enemyUnit.executeAttack();
    }
    public void EnemyTurn_SuperAttack()
    {
        enemyUnit.executeSuperAttack();
    }
    public void EnemyTurn_dirt()
    {
        enemyUnit.GetComponent<Animator>().Play("dirt_toss");
    }
    public void EnemyTurn_rage()
    {
        enemyUnit.ExecuteRage(3, 2);
    }

    // UI
    public void showHit(int dmg, Transform jumper)
    {
        jumper.gameObject.SetActive(true);
        jumper.transform.DOLocalMove(new Vector3(0, 0, 0), 0, true);
        jumper.GetComponent<TMP_Text>().DOFade(1, 0);

        Tween fadeTween = jumper.GetComponent<TMP_Text>().DOFade(0, 1.5f);
        Tween jumpTween = jumper.transform.DOLocalJump(new Vector2(40, 30), 30, 1, 1);

        fadeTween.OnComplete(() => jumper.gameObject.SetActive(false));
        
        // Show dmg
        if (jumper.name == "Hit Text enemy")
        {
            hitText_Enemy.GetComponent<TMP_Text>().text = dmg.ToString();
            hitText_Enemy.SetActive(true);
            fadeTween.Play();
            jumpTween.Play();
        } 
        else if (jumper.name == "Hit Text player") 
        {
            hitText_Player.GetComponent<TMP_Text>().text = dmg.ToString();
            hitText_Player.SetActive(true);
            fadeTween.Play();
            jumpTween.Play();
        } else if (jumper.name == "Miss Text player")
        {
            missText_Player.SetActive(true);
            fadeTween.Play();
            jumpTween.Play();
        }
    }
    public void updateUI()
    {
        // Update health
        enemyHUD.setHP(enemyUnit.currentHP);
        CombatManager.setPlayerHP(playerUnit.GetCurrentHP());

        // Update stamina
        CombatManager.GetStaminaSlider().DOValue(playerUnit.GetCurrentStamina(), 0.3f);

        // Update adrenaline
        CombatManager.GetPlayerAdrenalineSlider().DOValue(playerUnit.GetAdrenaline(), 0.3f);
        if (playerUnit.GetAdrenaline() >= playerUnit.GetMaxAdrenaline())
        {
            playerUnit.SetAdrenaline(playerUnit.GetMaxAdrenaline());
        }

        enemyHUD.adrenalineSlider.DOValue(enemyUnit.adrenaline, 0.3f);
        if (enemyUnit.adrenaline >= 20)
        {
            enemyUnit.adrenaline = 20;
        }
    }
    /*-----------------------------------END FIGHT---------------------------------------------------------*/
    public void CheckEndFight()
    {
        // Defeat
        if (playerUnit.GetCurrentHP() <= 0 && deadPlayer == false)
        {
            CombatManager.move_UI_out();
            cameraManager.playChrome();
            AudioManager.Stop("Combat_Theme");
            AudioManager.Play("Defeat_Sound");
            TimeManager.StopFightTimers();
            enemyUnit.GetComponent<Animator>().SetBool("Victory", true);
            
            EndManager.activateEndElements(true, 1);
            EndManager.DefeatScreen();

            deadPlayer = true;
        }
        // Victory
        else if (enemyUnit.currentHP <= 0 && deadEnemy == false)
        {
            CombatManager.move_UI_out();
            cameraManager.playChrome();
            AudioManager.Stop("Combat_Theme");
            AudioManager.Play("Last_Hit");
            TimeManager.StopFightTimers();

            EndManager.activateEndElements(true, 2);
            EndManager.VictoryScreen();
            StartCoroutine(EndManager.AnimatePlayerAvatarIn("Thanks for playing!", 3, true));

            deadEnemy = true;
        }
    }
    /*-----------------------------------END FIGHT---------------------------------------------------------*/

    public void resetBattle()
    {
        cameraManager.playChrome();
        EndManager.HideUpgradeScreen(true);
        EndManager.hideUpgradeButton();
        LoadingScreen.toggleLoadingScreen(1, 0.3f);
        StartCoroutine(LoadingScreen.fillLoadingSlider(1, 0.3f));

        setPlayerStats();
        setEnemyStats();
        GameManager.IncrementTurnCounter();
        EndManager.resetFight();
    }
    void setPlayerStats()
    {
        playerUnit.NativeDamage -= focusManager.GetTotalATKBuff();
        focusManager.ResetATKBuff();
        playerUnit.SetAdrenaline(0);
        playerUnit.SetCurrentStamina(playerUnit.GetMaxStamina());
        InputManager.SetPlayerAction("none");
        TimeManager.selectIcon("Default");
    }
    void setEnemyStats()
    {
        enemyUnit.nativeDamage = 4; // Default
        enemyUnit.baseSpeed = 13; // Default
        enemyUnit.setAngryState(false);
        enemyUnit.currentHP += (int)(enemyUnit.maxHP * 0.3f);
        enemyUnit.adrenaline = 0;
    }
    

    public void OpenPanels() // Deals with the panels showing at the begining of the fight
    {
        enemyPanel.DOLocalMoveX(900, 1).SetDelay(2.5f);
        playerPanel.DOLocalMoveX(-900, 1).SetDelay(2.5f).OnComplete(complete);
        void complete()
        {
            GameManager.SetDayCounter(1);
            StartCoroutine(GameManager.DayShow(3));
            TimeManager.continueUnitTimer();
            //audioManager.Play("Combat_Theme"); REMOVE
            _loadingScreen.SetActive(false);
        }
    } // Global Start
    public void ClosePanels()
    {
        playerPanel.DOLocalMoveX(-308, 1);
        enemyPanel.DOLocalMoveX(308, 1);
    }

    void checkBloom()
    {
        if (targetHit == 3 && canBloomAttack)
        {
            // Fix add sound
            StartCoroutine(TimeManager.slowMotion(.1f, .3f));
            cameraManager.PlayBloom(1);
            canBloomAttack = false;
        }
    }

    // G & S
    public bool GetDeadPlayer()
    {
        return deadPlayer;
    }
    public bool GetDeadEnemy()
    {
        return deadEnemy;
    }

    void GetScripts()
    {
        _playerAnimator = FindObjectOfType<Player>().GetComponent<Animator>();
        AudioManager = FindObjectOfType<AudioManager>();
        CombatManager = FindObjectOfType<CombatManager>();
        targetManager = FindObjectOfType<TargetManager>();
        DefendManager = FindObjectOfType<DefendManager>();
        TimeManager = FindObjectOfType<TimeManager>();
        EndManager = FindObjectOfType<EndManager>();
        cameraManager = FindObjectOfType<cameraManager>();
        LoadingScreen = FindObjectOfType<LoadingScreen>();
        GameManager = FindObjectOfType<GameManager>();

        playerUnit = FindObjectOfType<Player>();
        enemyUnit = FindObjectOfType<Enemy>();

        InputManager = FindObjectOfType<InputManager>();
    }
}



using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    TargetManager targetManager;
    defendManager defendManager;
    timeManager timeManager;
    Input_Manager Input_Manager;
    Combat_UI combat_UI;
    endManager endManager;
    cameraManager cameraManager;

    [SerializeField] Toggle testMode;

    [Header ("Mechanics")]
    public GameObject dodgeManager;
    public GameObject focusManager;
    public GameObject restManager;

    [Header("Scores")]
    public int targetHit;

    [Header("Units")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private Player playerUnit;
    private Enemy enemyUnit;
    bool deadPlayer = false;
    bool deadEnemy = false;

    // Animators
    private Animator playerAnimator;

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

    private void Start()
    {
        playerAnimator = playerPrefab.GetComponent<Animator>();
        combat_UI = FindObjectOfType<Combat_UI>();
        targetManager = FindObjectOfType<TargetManager>();
        defendManager = FindObjectOfType<defendManager>();
        timeManager = FindObjectOfType<timeManager>();
        endManager = FindObjectOfType<endManager>();
        cameraManager = FindObjectOfType<cameraManager>();

        playerUnit = FindObjectOfType<Player>();
        enemyUnit = FindObjectOfType<Enemy>();

        Input_Manager = FindObjectOfType<Input_Manager>();

        SetupBattle();
    }

    private void Update()
    {
        updateUI();
        checkEndFight();
        testModeActivation();
    }

    void SetupBattle()
    {
        playerUnit.SetCurrentHP(playerUnit.GetMaxHP());
        enemyUnit.currentHP = enemyUnit.maxHP;

        playerUnit.setCurrentShield(playerUnit.GetMaxShield());
        playerUnit.SetCurrentStamina(playerUnit.GetMaxStamina());
    }

    // ----------------- Actions -----------------

    public void PlayerAttack()
    {
        targetManager.attack();
        playerAnimator.Play("ATK_jump");
        playerUnit.incrementAdrenaline(1);
    }

    public void PlayerDefend()
    {
        if (playerUnit.GetCurrentStamina() >= 20)
        {
            defendManager.activateShieldMinigame();
        }
    }
    public void PlayDodge()
    {
        if (playerUnit.GetCurrentStamina() >= 20)
        {
            dodgeManager.SetActive(true);
        } 
    }
    public void PlayFocus()
    {
        focusManager.SetActive(true);
    }

    public void PlayRest()
    {
        restManager.SetActive(true);
        Input_Manager.SetPlayerAction("none");
        timeManager.selectIcon("Default");
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
        enemyUnit.executeRage(3, 2);
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
            hitText_Enemy.GetComponent<TMP_Text>().text = "- " + dmg;
            hitText_Enemy.SetActive(true);
            fadeTween.Play();
            jumpTween.Play();
        } 
        else if (jumper.name == "Hit Text player") 
        {
            hitText_Player.GetComponent<TMP_Text>().text = "- " + (dmg);
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
        combat_UI.setPlayerHP(playerUnit.GetCurrentHP());

        // Update stamina
        combat_UI.GetStaminaSlider().DOValue(playerUnit.GetCurrentStamina(), 0.5f);

        // Update adrenaline
        combat_UI.GetPlayerAdrenalineSlider().DOValue(playerUnit.GetAdrenaline(), 0.5f);
        if (playerUnit.GetAdrenaline() >= 20)
        {
            playerUnit.SetAdrenaline(20);
        }

        enemyHUD.adrenalineSlider.DOValue(enemyUnit.adrenaline, 0.5f);
        if (enemyUnit.adrenaline >= 20)
        {
            enemyUnit.adrenaline = 20;
        }
    }

    public void checkEndFight()
    {
        if (playerUnit.GetCurrentHP() <= 0 && deadPlayer == false) 
        {
            combat_UI.move_UI_out();
            cameraManager.playChrome();
            FindObjectOfType<AudioManager>().Play("Defeat_Sound");
            timeControlDefeatVictory();
            
            endManager.activateEndElements(true, 1);
            endManager.defeatScreen();

            deadPlayer = true;
        } 
        else if (enemyUnit.currentHP <= 0 && deadEnemy == false)
        {
            combat_UI.move_UI_out();
            cameraManager.playChrome();
            FindObjectOfType<AudioManager>().Play("Victory_Sound");
            timeControlDefeatVictory();

            endManager.activateEndElements(true, 2);
            endManager.victoryScreen();

            deadEnemy = true;
        }
    }

    public void testModeActivation()
    {
        if (testMode.isOn)
        {
            timeManager.stopUnitTimer();
        }
    }

    public void resetBattle()
    {
        combat_UI.move_UI_in();
        timeManager.resetFightTimer();
        timeManager.activateFightTimer();
        timeManager.resetPlayerTimer();
        timeManager.resetEnemyTimer();
        timeManager.continueUnitTimer();
        endManager.resetFight();
    }

    void timeControlDefeatVictory()
    {
        timeManager.executeSlowMotion(0.7f, 0.2f);
        timeManager.stopUnitTimer();
        timeManager.fadeOutUnitTimer();
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
}



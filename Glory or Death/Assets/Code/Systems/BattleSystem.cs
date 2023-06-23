using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BattleSystem : MonoBehaviour
{
    TargetManager targetManager;
    defendManager defendManager;
    Input_Manager Input_Manager;
    timeManager timeManager;
    Combat_UI combat_UI;

    //private bool canEvade = false;
    private float evadeTimer;

    // Systems
    public GameObject dodgeManager;
    public GameObject focusManager;

    // Scores
    public int targetHit;

    // Units
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private Player playerUnit;
    private Enemy enemyUnit;

    // Animators
    private Animator playerAnimator;

    // UI
    public EnemyHUD enemyHUD;

    // Text elements
    public GameObject missText;
    public GameObject hitText_Enemy;
    public GameObject hitText_Player;
    public GameObject thankYou;

    //Info Hud
    public GameObject infoHud;
    public GameObject infoHud_EN;

    private void Start()
    {
        playerAnimator = playerPrefab.GetComponent<Animator>();

        targetManager = FindObjectOfType<TargetManager>();
        defendManager = FindObjectOfType<defendManager>();
        timeManager = FindObjectOfType<timeManager>();
        combat_UI = FindObjectOfType<Combat_UI>();

        playerUnit = FindObjectOfType<Player>();
        enemyUnit = FindObjectOfType<Enemy>();

        Input_Manager = FindObjectOfType<Input_Manager>();

        SetupBattle();
    }

    private void Update()
    {
        updateUI();
        checkEndFight();
    }

    void SetupBattle()
    {
        playerUnit.SetCurrentHP(playerUnit.GetMaxHP());
        enemyUnit.currentHP = enemyUnit.maxHP;

        playerUnit.setCurrentShield(playerUnit.GetMaxShield());

        playerUnit.currentStamina = playerUnit.maxStamina;
    }

    // ----------------- Actions -----------------

    public void PlayerAttack()
    {
        targetManager.attack();
        playerAnimator.Play("ATK_jump");
        playerUnit.incrementAdrenaline(1);
        StartCoroutine(waitForDamage(3.6f));
    }

    public void PlayerDefend()
    {
        if (playerUnit.currentStamina >= 20)
        {
            defendManager.activateShieldMinigame();
        }
    }
    public void PlayDodge()
    {
        if (playerUnit.currentStamina >= 20)
        {
            dodgeManager.SetActive(true);
        } 
    }
    public void PlayFocus()
    {
        if (playerUnit.currentStamina >= 15)
        {
            focusManager.SetActive(true);
        }
    }

    public void PlayRest()
    {
        playerUnit.GetComponent<Animator>().Play("Cheer");
        playerUnit.currentStamina += 50; // Mejorable
        Input_Manager.SetPlayerAction("none");
        timeManager.selectIcon("Default");
    }




    // ------------------------Enemy turn------------------------
    public void EnemyTurn_attack()
    {
        enemyUnit.executeAttack();
    }
    public void EnemyTurn_dirt()
    {
        enemyUnit.GetComponent<Animator>().Play("dirt_toss");
    }
    public void EnemyTurn_rage()
    {
        enemyUnit.executeRage();
    }

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
            if (!playerUnit.missed)
            {
                hitText_Player.GetComponent<TMP_Text>().text = "- " + (dmg);
            } else
            {
                hitText_Player.GetComponent<TMP_Text>().text = "Missed!";
            }
            
            hitText_Player.SetActive(true);
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
        combat_UI.GetStaminaSlider().DOValue(playerUnit.currentStamina, 0.5f);

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

    IEnumerator waitForDamage(float delay)
    {
        //Delay
        yield return new WaitForSeconds(delay);

        //Do DMG
        enemyUnit.TakeDamage(targetHit);
        enemyHUD.setHP(enemyUnit.currentHP);
    }
    
    public void checkCritics()
    {
        if (targetHit == 3)
        {
            playerUnit.increaseCritHits(1);
        }
    }

    public void checkEndFight()
    {
        if (playerUnit.GetCurrentHP() <= 0)
        {
            Debug.Log("Defeat!");
        } else if (enemyUnit.currentHP <= 0)
        {
            Debug.Log("Win!");
        }
    }
}



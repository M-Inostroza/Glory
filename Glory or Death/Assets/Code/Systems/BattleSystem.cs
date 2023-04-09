using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    //Selected actions
    private string selectedPlayerAction;
    private string selectedEnemyAction;

    // Avoid click spam
    private bool canClick = true;

    //Target Manager
    private TargetManager targetManager;

    //Audio manager
    private AudioManager audioManager;

    // Cooldown Commands
    //private bool canEvade = false;
    private float evadeTimer;

    // Cooldown images (radial fill)
    private Image AttackButtonCD;
    private Image DefendButtonCD;
    private Image DodgeButtonCD;
    private Image FocusButtonCD;

    // Dodge System
    public GameObject dodgeManager;

    // Defend mechanic
    public GameObject defendManager;
    public defendManager defendManagerScript;

    // Focus manager
    public GameObject focusManager;

    //Scores
    public int targetHit;

    //Get player and enemy GO
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //Anims
    private Animator playerAnimator;

    //Gets the UI for both
    public PlayerHUD playerHUD;
    public EnemyHUD enemyHUD;

    //Miss & Hit text Player
    public GameObject missText;
    public GameObject hitText_Enemy;

    public GameObject hitText_Player;

    public GameObject thankYou;

    //Info Hud
    public GameObject infoHud;
    public GameObject infoHud_EN;
   
    //Gets the scripts for both
    private Player playerUnit;
    private Enemy enemyUnit;
    timeManager timeManager;

    private void Start()
    {
        playerAnimator = playerPrefab.GetComponent<Animator>();

        targetManager = FindObjectOfType<TargetManager>();
        timeManager = FindObjectOfType<timeManager>();

        playerUnit = FindObjectOfType<Player>();
        enemyUnit = FindObjectOfType<Enemy>();

        audioManager = FindObjectOfType<AudioManager>();

        AttackButtonCD = GameObject.FindWithTag("AttackCD").GetComponent<Image>();
        DefendButtonCD = GameObject.FindWithTag("DefendCD").GetComponent<Image>();
        DodgeButtonCD = GameObject.FindWithTag("DodgeCD").GetComponent<Image>();
        FocusButtonCD = GameObject.FindWithTag("FocusCD").GetComponent<Image>();

        evadeTimer = 5f;
        SetupBattle();
    }

    private void Update()
    {
        PlayerEvade();
        updateUI();

        timeManager.ReduceCooldown(DefendButtonCD);
        timeManager.ReduceCooldown(AttackButtonCD);
        timeManager.ReduceCooldown(DodgeButtonCD);
        timeManager.ReduceCooldown(FocusButtonCD);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void SetupBattle()
    {
        //Set stats to max

        // Shield
        playerUnit.currentShield = playerUnit.maxShield;
        enemyUnit.currentShield = enemyUnit.maxShield;

        // HP
        playerUnit.currentHP = playerUnit.maxHP;
        enemyUnit.currentHP = enemyUnit.maxHP;

        // Stamina
        playerUnit.currentStamina = playerUnit.maxStamina;
        enemyUnit.currentStamina = enemyUnit.maxStamina;

        playerUnit.currentAgility = playerUnit.maxAgility;
        enemyUnit.currentAgility = enemyUnit.maxAgility;
    }

    // ----------------- Buttons -----------------

    public void OnAttackButton()
    {
        if (AttackButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "ATK1";
            timeManager.selectIcon("ATK1");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnDefendButton()
    {
        if (DefendButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "DF";
            timeManager.selectIcon("DF");
        } else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnDodgeButton()
    {
        if (DodgeButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "DG";
            timeManager.selectIcon("DG");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnFocusButton()
    {
        if (FocusButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "FC";
            timeManager.selectIcon("FC");
        } else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    // Getters and Setters 
    public void SetCanClick(bool newValue)
    {
        canClick = newValue;
    }

    public void SetPlayerAction(string newAction)
    {
        selectedPlayerAction = newAction;
    }
    public string GetPlayerAction()
    {
        return selectedPlayerAction;
    }
    public void SetEnemyAction(string newAction)
    {
        selectedEnemyAction = newAction;
    }
    public string GetEnemyAction()
    {
        return selectedEnemyAction;
    }

    public Image GetAttackCD()
    {
        return AttackButtonCD;
    }
    public Image GetDefendCD()
    {
        return DefendButtonCD;
    }
    public Image GetDodgeCD()
    {
        return DodgeButtonCD;
    }
    public Image GetFocusCD()
    {
        return FocusButtonCD;
    }

    //----------------TO DO--------------------------

    public void OnSuperAttackButton()
    {
        PlayerSuperAttack();
    }

    public void OnRestButton()
    {
        PlayerRest();
    }

    public void OnEvadeButton()
    {
        isOnEvade = true;
        PlayerEvade();
    }

    // ----------------- Actions -----------------

    public void PlayerAttack()
    {
        targetManager.attack();
        //Play Animation
        playerAnimator.Play("ATK_jump");
        playerUnit.adrenaline += 1;
        //Enemy takes damage
        StartCoroutine(waitForDamage(3.6f));
    }

    public void PlayerDefend()
    {
        if (playerUnit.currentStamina >= 20)
        {
            defendManager.SetActive(true);
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

    // TO DO
    void PlayerSuperAttack()
    {
        //Check stamina
        if (playerUnit.currentStamina >= 1)
        {
            targetManager.attackHard();
            //Play Animation
            playerAnimator.SetBool("ATK2", true);

            //Enemy takes damage
            StartCoroutine(waitForDamage(4f));

            playerUnit.adrenaline = 0;
        }
    }

    void PlayerRest()
    {
        playerAnimator.SetBool("Resting", true);
        playerUnit.currentStamina += 50; //Mejorable
    }

    void PlayerCharge()
    {
        playerUnit.currentShield++;
    }


    // Evade bools
    bool canRight = true;
    bool canLeft = true;
    bool isOnEvade = false;

    void PlayerEvade()
    {
        // Checks if is on evade mode
        if (isOnEvade)
        {
            // Sets evade mechanic and timer (default 5f)
            playerHUD.evadeSlider.gameObject.SetActive(true);
            evadeTimer -= Time.deltaTime;
            if (evadeTimer <= 0)
            {
                evadeTimer = 0;
            }
            Debug.Log(evadeTimer);

            // Fill bar resistance & UI update
            playerUnit.evade -= playerUnit.evade * Time.deltaTime * 0.5f;
            playerHUD.evadeSlider.value = playerUnit.evade;

            // Direction arrows mechanic & evade buff
            if (Input.GetKeyDown("left") && canLeft)
            {
                canLeft = false;
                canRight = true;
                playerUnit.evade++;
            }
            else if (Input.GetKeyDown("right") && canRight)
            {
                canRight = false;
                canLeft = true;
                playerUnit.evade++;
            }

            // Triggers miss and evade anim
            if (playerUnit.evade >= 20 )
            {
                playerUnit.missed = true;
                playerAnimator.SetBool("Evade", true);

                // Deactivates evade mode, deactivates evade manager, resets timer, resets challenge, changes to enemy turn.
                resetEvades();
            }
            else if (evadeTimer == 0)
            {
                playerUnit.missed = false;
                resetEvades();
            }               
        }    
    }

    // Enemy turn
    public void EnemyTurn_attack()
    {
        enemyUnit.GetComponent<Animator>().SetBool("attack", true);
        enemyUnit.currentStamina -= 25;
    }
    public void EnemyTurn_dirt()
    {
        enemyUnit.GetComponent<Animator>().Play("dirt_toss");
        enemyUnit.currentStamina -= 20;
    }

    public void EndBattle()
    {
        //TO DO

        thankYou.transform.DOLocalMoveY(-110, 1f);
    }

    public Vector2 jumpVector;
    public float jumpPower;
    public float jumpTime;
    public void showHit(int dmg, GameObject unit)
    {
        hitText_Enemy.transform.localPosition = new Vector3(0, 0, 0);
        Tween fadeTween = hitText_Enemy.GetComponent<TMP_Text>().DOFade(0, 1.5f);
        Tween jumpTween = hitText_Enemy.transform.DOLocalJump(jumpVector, jumpPower, 1, jumpTime);
        fadeTween.Restart();
        jumpTween.Restart();
        fadeTween.OnComplete(() => unit.SetActive(false));
        Debug.Log("not restarting");
        
        //Show dmg
        if (unit == hitText_Enemy)
        {
            hitText_Enemy.GetComponent<TMP_Text>().text = "- " + dmg;
            hitText_Enemy.SetActive(true);
            fadeTween.Play();
            jumpTween.Play();
        } 
        else if (unit == hitText_Player) 
        {
            hitText_Player.GetComponent<TMP_Text>().text = "- " + (dmg);
            hitText_Player.SetActive(true);
            fadeTween.Play();
            jumpTween.Play();
        }
    }
    
    public void missHit()
    {
        GameObject missNotif = Instantiate(missText, infoHud.transform.position, Quaternion.identity);
        missNotif.transform.SetParent(infoHud.transform);

        missNotif.GetComponent<TMP_Text>().DOFade(0, 1.5f);
        missNotif.transform.DOJump(new Vector2(infoHud.transform.position.x + 1, infoHud.transform.position.y + 1), 1, 1, 1f).OnComplete(() => Destroy(missNotif));
    }

    // Deactivates evade mode, deactivates evade manager, resets timer, resets challenge, changes to enemy turn.
    private void resetEvades()
    {
        isOnEvade = false;
        playerHUD.evadeSlider.gameObject.SetActive(false);
        evadeTimer = 5f;
        playerUnit.evade = 0;
    }

    public void updateUI()
    {
        // Update health
        enemyHUD.setHP(enemyUnit.currentHP);
        playerHUD.setHP(playerUnit.currentHP);


        // Update stamina
        playerHUD.staminaSlider.DOValue(playerUnit.currentStamina, 0.5f);
        enemyHUD.staminaSlider.DOValue(enemyUnit.currentStamina, 0.5f);

        playerHUD.adrenalineSlider.DOValue(playerUnit.adrenaline, 0.5f);
        if (playerUnit.adrenaline >= 20)
        {
            playerUnit.adrenaline = 20;
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
        bool isDead = enemyUnit.TakeDamage(targetHit);
        enemyHUD.setHP(enemyUnit.currentHP);
    }
}



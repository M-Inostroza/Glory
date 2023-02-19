using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    //Selected actions
    public string selectedPlayerAction;
    public string selectedEnemyAction;
    // Avoid click spam
    public bool canClick = true;

    //Target Manager
    private TargetManager targetManager;

    //Audio manager
    [SerializeField]
    private AudioManager audioManager;

    // Cooldown Commands
    [SerializeField]
    public bool canEvade = false;
    private float evadeTimer;

    public Image DefendButtonCD;
    public Image AttackButtonCD;
    public Image DodgeButtonCD;
    public Image FocusButtonCD;

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
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    //Miss & Hit text Player
    public GameObject missText;
    public GameObject hitText;

    //Debug UI player
    public TMP_Text debugPLAYER_HP;
    public TMP_Text debugPLAYER_Shield;
    public TMP_Text debugPLAYER_Stamina;
    public TMP_Text debugPLAYER_Agility;

    //Debug UI enemy
    public TMP_Text debugENEMY_HP;
    public TMP_Text debugENEMY_Shield;
    public TMP_Text debugENEMY_Stamina;
    public TMP_Text debugENEMY_Agility;

    //Endgame 
    public GameObject victoryText;
    public GameObject defeatText;

    public GameObject thankYou;

    //Info Hud
    public GameObject infoHud;
    public GameObject infoHud_EN;
   
    //Gets the scripts for both
    Player playerUnit;
    Enemy enemyUnit;
    timeManager timeManager;

    private void Awake()
    {
        evadeTimer = 5f;
        SetupBattle();
    }

    private void Update()
    {
        debugScreen();
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
        //Get scripts for every unit
        playerUnit = playerPrefab.GetComponent<Player>();
        enemyUnit = enemyPrefab.GetComponent<Enemy>();

        //Get Animator
        playerAnimator = playerPrefab.GetComponent<Animator>();

        targetManager = FindObjectOfType<TargetManager>();
        timeManager = FindObjectOfType<timeManager>();


        //Set stats to max
        playerUnit.currentHP = playerUnit.maxHP;
        enemyUnit.currentHP = enemyUnit.maxHP;
        playerUnit.currentShield = playerUnit.maxShield;
        enemyUnit.currentShield = enemyUnit.maxShield;
        playerUnit.currentStamina = playerUnit.maxStamina;
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

    /*public void PlayerAttack()
    {
        //Check stamina
        if (playerUnit.currentStamina >= 1)
        {
            targetManager.attack();
            //Play Animation
            playerAnimator.SetBool("ATK1", true);
            playerUnit.adrenaline += 2;
            //Enemy takes damage
            StartCoroutine(waitForDamage(3.6f));

            //Reduce Stamina
            playerUnit.currentStamina -= 1;
            playerHUD.updateBricks(playerUnit.currentStamina);

            timeManager.defaultAction();
        }
            
    }*/
    public void PlayerAttack()
    {
        //Check stamina
        if (playerUnit.currentStamina >= 1)
        {
            targetManager.attack();
            //Play Animation
            playerAnimator.Play("ATK_jump");
            playerUnit.adrenaline += 1;
            //Enemy takes damage
            StartCoroutine(waitForDamage(3.6f));

            //Reduce Stamina
            playerUnit.currentStamina -= 1;
            playerHUD.updateBricks(playerUnit.currentStamina);

            timeManager.defaultAction();
        }
    }

    public void PlayerDefend()
    {
        if (playerUnit.currentStamina >= 1)
        {
            defendManager.SetActive(true);
            timeManager.defaultAction();
        }
    }
    public void PlayDodge()
    {
        if (playerUnit.currentStamina >= 1)
        {
            dodgeManager.SetActive(true);
            timeManager.defaultAction();
        } 
    }
    public void PlayFocus()
    {
        if (playerUnit.currentStamina >= 1)
        {
            focusManager.SetActive(true);
            timeManager.defaultAction();
        }
    }

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

            //Reduce Stamina
            playerUnit.currentStamina -= 1;
            playerHUD.updateBricks(playerUnit.currentStamina);

            playerUnit.adrenaline = 0;
        }
    }

    void PlayerRest()
    {
        playerAnimator.SetBool("Resting", true);
        playerUnit.currentStamina = 5;
        playerHUD.restoreBricks();
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
        // Attack Strong
        if (enemyUnit.adrenaline == 12)
        {
            int totalDMG = enemyUnit.native_damage + 3;
            enemyUnit.GetComponent<Animator>().SetBool("ATK2", true);
            playerAnimator.SetBool("DF2", true);

            //Deals damage to Player
            if (playerUnit.currentShield > 0)
            {
                bool isDead = playerUnit.TakeDamage(totalDMG);
                showHit(totalDMG);
                if (isDead)
                {
                    EndBattle();
                }
            }
            else
            {
                bool isDead = playerUnit.TakeDamage(totalDMG);
                showHit(totalDMG);
                if (isDead)
                {
                    EndBattle();
                }
            }
        }
        else
        {
            // ---------- ATTACK BASIC ----------
            enemyUnit.GetComponent<Animator>().SetBool("attack", true);
            
        }
    }

    public void EndBattle()
    {
        //TO DO

        thankYou.transform.DOLocalMoveY(-110, 1f);
    }

    void debugScreen()
    {
        debugPLAYER_HP.text = "HP: " + playerUnit.currentHP;
        debugPLAYER_Shield.text = "Shield: " + playerUnit.currentShield;
        debugPLAYER_Stamina.text = "Stamina: " + playerUnit.currentStamina;
        debugPLAYER_Agility.text = "Agility: " + playerUnit.currentAgility;

        debugENEMY_HP.text = "HP: " + enemyUnit.currentHP;
        debugENEMY_Shield.text = "Shield: " + enemyUnit.currentShield;
        debugENEMY_Agility.text = "Agility: " + enemyUnit.currentAgility;
    }

    public void showHit(int dmg)
    {
        //Show dmg
        GameObject hitNotif = Instantiate(hitText, infoHud.transform.position, Quaternion.identity);
        hitNotif.transform.SetParent(infoHud.transform);

        //check shield
        hitNotif.GetComponent<TMP_Text>().text = "- " + (dmg);

        hitNotif.GetComponent<TMP_Text>().DOFade(0, 1.5f);
        hitNotif.transform.DOJump(new Vector2(infoHud.transform.position.x +1, infoHud.transform.position.y + 1), 1, 1, 1f).OnComplete(() => Destroy(hitNotif));
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
        enemyHUD.setHP(enemyUnit.currentHP);
        playerHUD.setHP(playerUnit.currentHP);
        playerHUD.adrenalineSlider.value = playerUnit.adrenaline;
        if (playerUnit.adrenaline >= 20)
        {
            playerUnit.adrenaline = 20;
        }

        enemyHUD.adrenalineSlider.value = enemyUnit.adrenaline;
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
        bool isDead = enemyUnit.TakeDamage(playerUnit.native_damage + targetHit);
        enemyHUD.setHP(enemyUnit.currentHP);

        //Show dmg on enemy
        GameObject hitNotif = Instantiate(hitText, infoHud_EN.transform.position, Quaternion.identity);
        hitNotif.transform.SetParent(infoHud_EN.transform);

        //Check defense
        if (enemyUnit.currentShield > 0)
            hitNotif.GetComponent<TMP_Text>().text = "- " + (playerUnit.native_damage - 2);
        else
            hitNotif.GetComponent<TMP_Text>().text = "- " + (playerUnit.native_damage + targetHit);

        //Eliminate
        hitNotif.GetComponent<TMP_Text>().DOFade(0, 1.5f).OnComplete(() => Destroy(hitNotif));
        hitNotif.transform.DOJump(new Vector2(infoHud_EN.transform.position.x + 1, infoHud_EN.transform.position.y + 1), 1, 1, 1f).OnComplete(() => Destroy(hitNotif));
    }
}



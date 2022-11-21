using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

//Manages the battle states
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }
    //Cooldown Commands
    [SerializeField]
    private bool canDefend = true;
    public bool canEvade = false;
    private float evadeTimer;
     
    // Defend system
    public GameObject defendManager;

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

    //Info Hud
    public GameObject infoHud;
    public GameObject infoHud_EN;
   
    //Gets the scripts for both
    Player playerUnit;
    Enemy enemyUnit;

    public BattleState state;

    private void Start()
    {
        evadeTimer = 5f;
        Instance = this;
        state = BattleState.START;
        SetupBattle();
    }

    private void Update()
    {
        debugScreen();
        PlayerEvade();
        updateUI();
    }

    void SetupBattle()
    {
        //Get scripts for every unit
        playerUnit = playerPrefab.GetComponent<Player>();
        enemyUnit = enemyPrefab.GetComponent<Enemy>();

        //Get Animator
        playerAnimator = playerPrefab.GetComponent<Animator>();

        //Set HP to max
        playerUnit.currentHP = playerUnit.maxHP;
        enemyUnit.currentHP = enemyUnit.maxHP;
        
        //Set shield to max
        playerUnit.currentShield = playerUnit.maxShield;
        enemyUnit.currentShield = enemyUnit.maxShield;

        //Set Stamina to max
        playerUnit.currentStamina = playerUnit.maxStamina;
        enemyUnit.currentStamina = enemyUnit.maxStamina;

        //Set Agility to max
        playerUnit.currentAgility = playerUnit.maxAgility;
        enemyUnit.currentAgility = enemyUnit.maxAgility;

        state = BattleState.PLAYERTURN;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        PlayerAttack();
    }

    public void OnSuperAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        PlayerSuperAttack();
    }

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        PlayerDefend();
    }

    public void OnEvadeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        isOnEvade = true;
        PlayerEvade();
    }

    void PlayerAttack()
    {
        //Check stamina
        if (playerUnit.currentStamina >= 1)
        {
            //Play Animation
            playerAnimator.SetBool("ATK1", true);

            //Enemy takes damage
            StartCoroutine(waitForDamage(3.6f));

            //Reduce Stamina
            playerUnit.currentStamina -= 1;
            playerHUD.updateBricks(playerUnit.currentStamina);

            //Resets defend counter
            canDefend = true;
        }
            
    }

    void PlayerSuperAttack()
    {
        //Check stamina
        if (playerUnit.currentStamina >= 1)
        {
            //Play Animation
            playerAnimator.SetBool("ATK2", true);

            //Enemy takes damage
            StartCoroutine(waitForDamage(4f));

            //Reduce Stamina
            playerUnit.currentStamina -= 1;
            playerHUD.updateBricks(playerUnit.currentStamina);

            //Resets defend counter
            canDefend = true;
        }
    }

    void PlayerDefend()
    {
        if (canDefend)
        {
            playerUnit.currentShield++;
            canDefend = false;
            switchToEnemy();
        } 
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
    IEnumerator EnemyTurn()
    {
        //Check stamina
        if (enemyUnit.currentStamina >= 1)
        {
            // Starts defend system
            yield return new WaitForSeconds(1f);
            defendManager.SetActive(true);

            enemyUnit.GetComponent<Animator>().SetBool("ATK1", true);

            //Delay
            yield return new WaitForSeconds(3.4f);

            
            //HITS!!
            if (!playerUnit.missed)
            {
                //Deals damage to Player
                bool isDead = playerUnit.TakeDamage(enemyUnit.native_damage);
                enemyUnit.currentStamina -= 1;
                hits();

                if (isDead)
                    state = BattleState.LOST;
                else
                    state = BattleState.PLAYERTURN;
            } else
            {
                missHit();
                state = BattleState.PLAYERTURN;
            }
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            Debug.Log("You won!");
            playerAnimator.SetBool("WIN", true);
            victoryText.transform.DOLocalJump(new Vector2(0, 0), 10f, 1, .5f);
            enemyUnit.GetComponent<Animator>().SetBool("Defeat", true);
        } else if (state == BattleState.LOST)
        {
            Debug.Log("you lost");
            defeatText.transform.DOLocalJump(new Vector2(0, 0), 10f, 1, .5f);
        }
    }

    void debugScreen()
    {
        debugPLAYER_HP.text = "HP: " + playerUnit.currentHP;
        debugPLAYER_Shield.text = "Shield: " + playerUnit.currentShield;
        debugPLAYER_Stamina.text = "Stamina: " + playerUnit.currentStamina;
        debugPLAYER_Agility.text = "Agility: " + playerUnit.currentAgility;

        debugENEMY_Stamina.text = "Stamina: " + enemyUnit.currentStamina;
        debugENEMY_HP.text = "HP: " + enemyUnit.currentHP;
        debugENEMY_Shield.text = "Shield: " + enemyUnit.currentShield;
        debugENEMY_Agility.text = "Agility: " + enemyUnit.currentAgility;
    }

    public void hits()
    {
        //Show dmg
        GameObject hitNotif = Instantiate(hitText, infoHud.transform.position, Quaternion.identity);
        hitNotif.transform.SetParent(infoHud.transform);

        if (playerUnit.currentShield > 0)
            hitNotif.GetComponent<TMP_Text>().text = "- " + (enemyUnit.native_damage - 2);
        else if (playerUnit.currentShield == 0)
            hitNotif.GetComponent<TMP_Text>().text = "- " + enemyUnit.native_damage;

        hitNotif.GetComponent<TMP_Text>().DOFade(0, 1f);
        hitNotif.transform.DOJump(new Vector2(infoHud.transform.position.x +40, infoHud.transform.position.y + 50), 1, 1, 1f).OnComplete(() => Destroy(hitNotif));
    }
    public void missHit()
    {
        GameObject missNotif = Instantiate(missText, infoHud.transform.position, Quaternion.identity);
        missNotif.transform.SetParent(infoHud.transform);

        missNotif.GetComponent<TMP_Text>().DOFade(0, 1f);
        missNotif.transform.DOJump(new Vector2(infoHud.transform.position.x + 40, infoHud.transform.position.y + 50), 1, 1, 1f).OnComplete(() => Destroy(missNotif));
    }

    public void switchToEnemy()
    {
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // Deactivates evade mode, deactivates evade manager, resets timer, resets challenge, changes to enemy turn.
    private void resetEvades()
    {
        isOnEvade = false;
        playerHUD.evadeSlider.gameObject.SetActive(false);
        evadeTimer = 5f;
        playerUnit.evade = 0;
        switchToEnemy();
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
        hitNotif.transform.DOJump(new Vector2(infoHud_EN.transform.position.x + 40, infoHud_EN.transform.position.y + 50), 1, 1, 1f).OnComplete(() => Destroy(hitNotif));

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            switchToEnemy();
        }
    }
}



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

    //Debug UI player
    public TMP_Text debugPLAYER_HP;
    public TMP_Text debugPLAYER_Shield;
    public TMP_Text debugPLAYER_Stamina;
    public TMP_Text debugPLAYER_Agility;

    //Miss text Player
    public GameObject missText;
    public Vector3 missPositionInit;
    public Vector3 missPositionEnd;
    //Hit text Player
    public GameObject hitText;
    public Vector3 jumpPlayerPos;
    public Vector3 originalPlayerPos;


    public Vector3 jumpEnemyPos;
    public Vector3 originalEnemyPos;

    //Debug UI enemy
    public TMP_Text debugENEMY_HP;
    public TMP_Text debugENEMY_Shield;
    public TMP_Text debugENEMY_Stamina;
    public TMP_Text debugENEMY_Agility;

   
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
    }

    void SetupBattle()
    {
        //Get scripts
        playerUnit = playerPrefab.GetComponent<Player>();
        enemyUnit = enemyPrefab.GetComponent<Enemy>();

        //Get Animator
        playerAnimator = playerPrefab.GetComponent<Animator>();

        //Set HP to max
        playerUnit.currentHP = playerUnit.maxHP;
        playerHUD.setHP(playerUnit.maxHP);

        enemyUnit.currentHP = enemyUnit.maxHP;
        enemyHUD.setHP(enemyUnit.maxHP);

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
            StartCoroutine(stopAttack());

            //Enemy takes damage
            StartCoroutine(waitForDamage(3.2f));

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
        if (isOnEvade)
        {
            playerHUD.evadeSlider.gameObject.SetActive(true);
            evadeTimer -= Time.deltaTime;
            if (evadeTimer <= 0)
            {
                evadeTimer = 0;
            }
            Debug.Log(evadeTimer);

            playerUnit.evade -= playerUnit.evade * Time.deltaTime * 0.5f;
            playerHUD.evadeSlider.value = playerUnit.evade;

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

            if (playerUnit.evade >= 20 )
            {
                playerUnit.missed = true;
                resetEvades();
            }
            else if (evadeTimer == 0)
            {
                playerUnit.missed = false;
                resetEvades();
            }               
        }    
    }

    // Enemy
    IEnumerator EnemyTurn()
    {
        //Check stamina
        if (enemyUnit.currentStamina >= 1)
        {
            //Delay
            yield return new WaitForSeconds(3f);

            //Does damage to Player
            bool isDead = playerUnit.TakeDamage(enemyUnit.native_damage);
            playerHUD.setHP(playerUnit.currentHP);
            enemyUnit.currentStamina -= 2;

            //Animate Shield
            playerAnimator.SetBool("DF", true);
            StartCoroutine(stopDefend());

            //HITS!!
            if (!playerUnit.missed)
            {
                hits();
            } else
            {
                missHit();
            }
            
   
            if (isDead)
                state = BattleState.LOST;
            else
                state = BattleState.PLAYERTURN;
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            Debug.Log("You won!");
        } else if (state == BattleState.LOST)
        {
            Debug.Log("you lost");
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
        GameObject hitNotif = Instantiate(hitText, originalPlayerPos, Quaternion.identity);
        hitNotif.transform.SetParent(playerHUD.transform);

        if (playerUnit.currentShield > 0)
            hitNotif.GetComponent<TMP_Text>().text = "- " + (enemyUnit.native_damage - 2);
        else if (playerUnit.currentShield == 0)
            hitNotif.GetComponent<TMP_Text>().text = "- " + enemyUnit.native_damage;

        hitNotif.GetComponent<TMP_Text>().DOFade(0, 1f);
        hitNotif.transform.DOLocalMove(jumpPlayerPos, 1f).OnComplete(() => Destroy(hitNotif));
    }
    public void missHit()
    {
        GameObject missNotif = Instantiate(missText, missPositionInit, Quaternion.identity);
        missNotif.transform.SetParent(playerHUD.transform);

        missNotif.GetComponent<TMP_Text>().DOFade(0, 1f);
        missNotif.transform.DOLocalMove(missPositionEnd, 1f).OnComplete(() => Destroy(missNotif));
    }

    public void switchToEnemy()
    {
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    private void resetEvades()
    {
        isOnEvade = false;
        playerHUD.evadeSlider.gameObject.SetActive(false);
        evadeTimer = 5f;
        playerUnit.evade = 0;
        switchToEnemy();
    }

    IEnumerator stopAttack()
    {
        //Delay
        yield return new WaitForSeconds(4f);
        playerAnimator.SetBool("ATK1", false);
    }

    IEnumerator stopDefend()
    {
        //Delay
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetBool("DF", false);
    }

    IEnumerator waitForDamage(float delay)
    {
        //Delay
        yield return new WaitForSeconds(delay);

        //Do DMG
        bool isDead = enemyUnit.TakeDamage(playerUnit.native_damage + targetHit);
        enemyHUD.setHP(enemyUnit.currentHP);

        //Show dmg on enemy
        GameObject hitNotif = Instantiate(hitText, originalEnemyPos, Quaternion.identity);
        hitNotif.transform.SetParent(enemyHUD.transform);

        if (enemyUnit.currentShield > 0)
            hitNotif.GetComponent<TMP_Text>().text = "- " + (playerUnit.native_damage - 2);
        else
            hitNotif.GetComponent<TMP_Text>().text = "- " + (playerUnit.native_damage + targetHit);

        hitNotif.GetComponent<TMP_Text>().DOFade(0, 1.5f).OnComplete(() => Destroy(hitNotif));

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



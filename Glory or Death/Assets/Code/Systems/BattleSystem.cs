using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BattleSystem : MonoBehaviour
{
    private TargetManager targetManager;
    private AudioManager audioManager;
    private Input_Manager Input_Manager;

    // Cooldown Commands
    //private bool canEvade = false;
    private float evadeTimer;

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
        Input_Manager = FindObjectOfType<Input_Manager>();

        evadeTimer = 5f;
        SetupBattle();
    }

    private void Update()
    {
        PlayerEvade();
        updateUI();

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void SetupBattle()
    {
        playerUnit.SetCurrentShield(playerUnit.GetMaxShield());

        playerUnit.SetCurrentHP(playerUnit.GetMaxHP());
        enemyUnit.currentHP = enemyUnit.maxHP;

        // Stamina
        playerUnit.currentStamina = playerUnit.maxStamina;

        playerUnit.currentAgility = playerUnit.maxAgility;
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
        Debug.Log("Executing from BattleSystem");
        targetManager.attack();
        playerAnimator.Play("ATK_jump");
        playerUnit.incrementAdrenaline(1);
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

    public void PlayRest()
    {
        playerUnit.GetComponent<Animator>().Play("Cheer");
        playerUnit.currentStamina += 50; // Mejorable
        Input_Manager.SetPlayerAction("none");
        timeManager.selectIcon("Default");
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
        }
    }

    void PlayerRest()
    {
        playerAnimator.SetBool("Resting", true);
        playerUnit.currentStamina += 50; //Mejorable
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
                playerAnimator.SetBool("evadeJump", true);

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
    }
    public void EnemyTurn_dirt()
    {
        enemyUnit.GetComponent<Animator>().Play("dirt_toss");
    }

    public void EnemyTurn_rage()
    {
        enemyUnit.executeRage();
    }

    public void EndBattle()
    {
        //TO DO

        thankYou.transform.DOLocalMoveY(-110, 1f);
    }

    public Vector2 jumpVector;
    public float jumpPower;
    public float jumpTime;
    public void showHit(int dmg, Transform jumper)
    {
        jumper.gameObject.SetActive(true);

        jumper.transform.DOLocalMove(new Vector3(0, 0, 0), 0, true);
        jumper.GetComponent<TMP_Text>().DOFade(1, 0);

        Tween fadeTween = jumper.GetComponent<TMP_Text>().DOFade(0, 1.5f);
        Tween jumpTween = jumper.transform.DOLocalJump(jumpVector, jumpPower, 1, jumpTime);

        fadeTween.OnComplete(() => jumper.gameObject.SetActive(false));
        
        //Show dmg
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
        playerHUD.setHP(playerUnit.GetCurrentHP());
        


        // Update stamina
        playerHUD.staminaSlider.DOValue(playerUnit.currentStamina, 0.5f);

        playerHUD.adrenalineSlider.DOValue(playerUnit.GetAdrenaline(), 0.5f);
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
        bool isDead = enemyUnit.TakeDamage(targetHit);
        enemyHUD.setHP(enemyUnit.currentHP);
    }
    
    public void checkCritics()
    {
        if (targetHit == 3)
        {
            playerUnit.increaseCritHits(1);
        }
    }
}



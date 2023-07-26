using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private timeManager timeManager;
    private TargetManager targetManager; 
    private BattleSystem BS;
    
    [Header("Stats")]
    [SerializeField] private float maxSpeed, baseSpeed;
    [SerializeField] private float maxStamina, currentStamina; 
    [SerializeField] private int maxHP, currentHP;
    [SerializeField] private int adrenaline;
    [SerializeField] private int nativeDamage;
    [SerializeField] private int maxShield, currentShield;
    [SerializeField] private int critHits;

    [SerializeField] private DodgeManager dodgeManager;

    //Agility (Dodging)
    public bool missed = false;

    public Camera MainCamera;

    Animator myAnim;

    // Current Enemy
    public GameObject enemy_unit;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        BS = FindObjectOfType<BattleSystem>();
        targetManager = FindObjectOfType<TargetManager>();
        timeManager = FindObjectOfType<timeManager>();
    }

    private void Update()
    {
        capShield();
        capHP();
        checkVictoryCondition();
    }

    public void TakeDamage(int dmg)
    {
        if (!missed)
        {
            missed = false;
            currentHP -= dmg;
        }
        else
        {
            missed = true;
        }
    }

    // ------- Anim Methods -------
    
    public void blockAttack()
    {
        myAnim.Play("blockAttack");
    }
    public void backToIdle()
    {
        myAnim.Play("Idle");
    }
    public void nextAttack()
    {
        if (targetManager.attackOrder.Count > 0)
        {
            switch (targetManager.attackOrder.First())
            {
                case 0:
                    myAnim.Play("ATK_head");
                    targetManager.attackOrder.Remove(targetManager.attackOrder.First());
                    break;
                case 1:
                    myAnim.Play("ATK_mid");
                    targetManager.attackOrder.Remove(targetManager.attackOrder.First());
                    break;
                case 2:
                    myAnim.Play("ATK_bottom");
                    targetManager.attackOrder.Remove(targetManager.attackOrder.First());
                    break;
            }
        } else
        {
            myAnim.Play("ATK_back");
        }
    }
    public void stopAttack()
    {
        if (BS.GetDeadEnemy() == false)
        {
            timeManager.playerActionIcon.sprite = timeManager.iconSprites[1];
            timeManager.playerTimer.fillAmount = 1;
            FindObjectOfType<Input_Manager>().SetPlayerAction("none");
            timeManager.fadeInUnitTimer();
            timeManager.continueUnitTimer();
            myAnim.Play("Idle");
        } else
        {
            backToIdle();
        }
    }
    public void returnCamera()
    {
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);
    }
    public void stopHurt()
    {
        myAnim.SetBool("HURT", false);
    }
    public void stopDefendSkill()
    {
        myAnim.SetBool("DF_Skill", false);
        timeManager.continueUnitTimer();
        timeManager.defaultAction();
    }
    public void stopDodgeSkill()
    {
        myAnim.SetBool("evadeSuccess", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.continueUnitTimer();
        timeManager.defaultAction();
    }
    public void stopDodgeSkillFail()
    {
        myAnim.SetBool("skillFail", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopEvadeJump()
    {
        missed = false;
        myAnim.SetBool("evadeJump", false);
        timeManager.fadeInUnitTimer();
    }
    public void stopDodgeIcon()
    {
        dodgeManager.deactivateDodgeBuff();
    }
    public void stopFocusSkill()
    {
        myAnim.SetBool("focusSuccess", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopShieldSuccess()
    {
        myAnim.SetBool("skillShieldSuccess", false);
        FindObjectOfType<Combat_UI>().shieldFeed();
        timeManager.playerTimer.fillAmount = 1;
        timeManager.continueUnitTimer();
        timeManager.defaultAction();
    }
    public void stopRest()
    {
        myAnim.SetBool("Resting", false);
    }
    public void stopSuperDefend()
    {
        myAnim.SetBool("DF2", false);
    }
    public void startEnemyDefense()
    {
        enemy_unit.GetComponent<Animator>().SetBool("hurt_basic", true);
    }
    public void hurtParts(int part)
    {
        switch (part)
        {
            case 0:
                enemy_unit.GetComponent<Animator>().Play("head_hurt");
                break;
            case 1:
                enemy_unit.GetComponent<Animator>().Play("mid_hurt");
                break;
            case 2:
                enemy_unit.GetComponent<Animator>().Play("bottom_hurt");
                break;
        }
    }

    public void checkVictoryCondition()
    {
        myAnim.SetInteger("Victory", enemy_unit.GetComponent<Enemy>().currentHP);
    }
    public void checkDefeatCondition()
    {
        if (BS.GetDeadPlayer())
        {
            myAnim.Play("DefeatSuper");
        }
    }
    public void showEnemyDamage()
    {
        BS.showHit(nativeDamage, BS.hitText_Enemy.transform);
    }


    // ---------------- Getters and Setters ----------------

    // HP
    public int GetMaxHP()
    {
        return maxHP;
    }
    public void SetMaxHP(int MaxHP)
    {
        maxHP = MaxHP;
    }
    public int GetCurrentHP()
    {
        return currentHP;
    }
    public void SetCurrentHP(int CurrentHP)
    {
        currentHP = CurrentHP;
    }

    // Stamina
    public float GetMaxStamina()
    {
        return maxStamina;
    }
    public float GetCurrentStamina()
    {
        return currentStamina;
    }
    public void SetCurrentStamina(float newStamina)
    {
        currentStamina = newStamina;
    }
    public float IncrementCurrentStamina(float plusStamina)
    {
        return currentStamina += plusStamina;
    }
    public float DecrementCurrentStamina(float plusStamina)
    {
        return currentStamina -= plusStamina;
    }

    // DMG
    public int getDamage()
    {
        return nativeDamage;
    }
    public void increaseDamage(int newDamage)
    {
        nativeDamage += newDamage;
    }
    public void decreaseDamage(int newDamage)
    {
        nativeDamage -= newDamage;
    }
    public void deactivateAttackFeed()
    {
        GameObject attackFeedback = targetManager.GetAttackFeedback();
        foreach (Transform child in attackFeedback.GetComponentsInChildren<Transform>(true))
        {
            if (child == transform)
                continue;
            child.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(()=> attackFeedback.SetActive(false));
        }
        targetManager.checkCritic();
    }
    public void doDMG()
    {
        enemy_unit.GetComponent<Enemy>().TakeDamage(nativeDamage);
    }

    // Shield
    public int GetMaxShield()
    {
        return maxShield;
    }
    public void setMaxShield(int newShield)
    {
        maxShield = newShield;
    }
    public int getCurrentShield()
    {
        return currentShield;
    }
    public void setCurrentShield(int newShield)
    {
        currentShield = newShield;
    }
    public void increaseCurrentShield()
    {
        currentShield++;
    }
    public void decreaseCurrentShield()
    {
        currentShield--;
    }

    // Speed
    public float GetBaseSpeed()
    {
        return baseSpeed;
    }
    public void incrementBaseSpeed(float speedBuff)
    {
        baseSpeed += speedBuff;
    }
    public void reduceBaseSpeed(float speedDebuff)
    {
        baseSpeed -= speedDebuff;
    }

    // Adrenaline
    public int GetAdrenaline()
    {
        return adrenaline;
    }
    public void SetAdrenaline(int newAdrenaline)
    {
        adrenaline = newAdrenaline;
    }
    public void incrementAdrenaline(int newAdrenaline)
    {
       adrenaline  += newAdrenaline;
    }
    public void reduceAdrenaline(int newAdrenaline)
    {
        adrenaline -= newAdrenaline;
    }

    // Critic Hits
    public int GetCritHits()
    {
        return critHits;
    }
    public void SetCritHits(int newHit)
    {
        critHits = newHit;
    }
    public void increaseCritHits(int newHit)
    {
        critHits += newHit;
    }
    public void reduceHits(int newHit)
    {
        critHits -= newHit;
    }

    // Caps
    void capShield()
    {
        if (currentShield <= 0)
        {
            currentShield = 0;
        }
        else if (currentShield >= maxShield)
        {
            currentShield = maxShield;
        }
    }
    void capHP()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
        }
        myAnim.SetInteger("Defeat", currentHP);
    }

    // Buffs
    public void doDamageBuff()
    {
        FindObjectOfType<Combat_UI>().damageBuff("player");
    }
    public void doSpeedBuff()
    {
        FindObjectOfType<Combat_UI>().speedBuff("player");
    }
    public IEnumerator boostSpeed()
    {
        baseSpeed += 3f;
        yield return new WaitForSeconds(3.5f);
        baseSpeed -= 3f;
    }

    // Effects
    public void shakeHit()
    {
        transform.DOShakePosition(0.3f, 0.2f, 22, 10, false, true);
    }
    public void blockHit()
    {
        FindObjectOfType<AudioManager>().Play("Shield_metal");
        Time.timeScale = 0.2f;
    }
    public void returnTime()
    {
        Time.timeScale = 1;
    }
}

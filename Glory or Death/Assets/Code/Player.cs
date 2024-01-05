using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private timeManager timeManager;
    private TargetManager targetManager; 
    private BattleSystem BS;
    private AudioManager audioManager;
    private Tutorial_UI _tutorial_UI;
    private Combat_UI _combat_UI;
    private cameraManager _cameraManager;
    
    [Header("Stats")]
    [SerializeField] private float maxSpeed, baseSpeed;
    [SerializeField] private float maxStamina, currentStamina; 
    [SerializeField] private int maxHP, currentHP;
    [SerializeField] private int adrenaline;
    [SerializeField] private int _nativeDamage;

    [SerializeField] Animator _dummyAnimator;

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
        _tutorial_UI = FindObjectOfType<Tutorial_UI>();
        _combat_UI = FindObjectOfType<Combat_UI>();

        myAnim = GetComponent<Animator>();
        BS = FindObjectOfType<BattleSystem>();
        targetManager = FindObjectOfType<TargetManager>();
        timeManager = FindObjectOfType<timeManager>();
        audioManager = FindObjectOfType<AudioManager>();
        _cameraManager = FindObjectOfType<cameraManager>();

        setStats();
    }

    private void Update()
    {
        capHP();
        capStamina();
        checkVictoryCondition();
    }

    void setStats()
    {
        currentStamina = maxStamina;
        currentShield = 0;
        currentHP = maxHP;
        if (gameManager.isTutorial())
        {
            currentShield = 0;
        } else
        {
            currentShield = maxShield / 2;
        }
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
        if (!gameManager.isTutorial())
        {
            if (BS.GetDeadEnemy() == false)
            {
                timeManager.playerActionIcon.sprite = timeManager.iconSprites[1];
                timeManager.playerTimer.fillAmount = 1;
                FindObjectOfType<Input_Manager>().SetPlayerAction("none");
                timeManager.fadeInUnitTimer();
                timeManager.continueUnitTimer();
                myAnim.Play("Idle");
            }
            else
            {
                backToIdle();
            }
        } else
        {
            _tutorial_UI.fadeTimer(1);
            _tutorial_UI.selectIcon("Default");
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
        if (!gameManager.isTutorial())
        {
            timeManager.playerTimer.fillAmount = 1;
            timeManager.continueUnitTimer();
            timeManager.defaultAction();
        } else
        {
            _tutorial_UI.fadeTimer(1);
            _tutorial_UI.selectIcon("Default");
        }
    }
    public void stopDodgeSkillFail()
    {
        myAnim.SetBool("skillFail", false);
        if (gameManager.isTutorial())
        {
            _tutorial_UI.fadeTimer(1);
            _tutorial_UI.selectIcon("Default");
        } else
        {
            timeManager.playerTimer.fillAmount = 1;
            timeManager.continueUnitTimer();
            timeManager.defaultAction();
        }
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
        if (!gameManager.isTutorial())
        {
            timeManager.playerTimer.fillAmount = 1;
            timeManager.continueUnitTimer();
            timeManager.defaultAction();
        } else
        {
            _tutorial_UI.fadeTimer(1);
            _tutorial_UI.selectIcon("Default");
        }
    }
    public void stopShieldSuccess()
    {
        myAnim.SetBool("skillShieldSuccess", false);
        if (gameManager.isTutorial())
        {
            _tutorial_UI.fadeTimer(1);
            _tutorial_UI.selectIcon("Default");
        } else
        {
            Combat_UI.shieldFeed();
            timeManager.playerTimer.fillAmount = 1;
            timeManager.continueUnitTimer();
            timeManager.defaultAction();
        }
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
        if (!gameManager.isTutorial())
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
        } else
        {
            switch (part)
            {
                case 0:
                    _dummyAnimator.Play("dummy_hurt_up");
                    break;
                case 1:
                    _dummyAnimator.Play("dummy_hurt_mid");
                    break;
                case 2:
                    _dummyAnimator.Play("dummy_hurt_down");
                    break;
            }
        }
    }

    public void checkVictoryCondition()
    {
        if (!gameManager.isTutorial() && enemy_unit != null)
        {
            myAnim.SetInteger("Victory", enemy_unit.GetComponent<Enemy>().currentHP);
        }
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
        if (!gameManager.isTutorial())
        {
            BS.showHit(_nativeDamage, BS.hitText_Enemy.transform);
        }
    }
    public void showEnemySuperDamage()
    {
        BS.showHit(superATKManager.GetHits(), BS.hitText_Enemy.transform);
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
        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
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
    public int NativeDamage
    {
        get => _nativeDamage;
        set => _nativeDamage = value;
    }
    public void resetDamage()
    {
        NativeDamage = 1;
    }
    public void increaseDamage(int newDamage)
    {
        _nativeDamage += newDamage;
    }
    public void decreaseDamage(int newDamage)
    {
        _nativeDamage -= newDamage;
    }

    // Anim ref
    public void deactivateAttackFeed()
    {
        GameObject attackFeedback = targetManager.GetAttackFeedback();
        foreach (Transform child in attackFeedback.GetComponentsInChildren<Transform>(true))
        {
            if (child == transform)
                continue;
            child.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(()=> attackFeedback.SetActive(false));
        }
        if (!gameManager.isTutorial())
        {
            targetManager.checkCritic();
        }
    }
    public void doDMG()
    {
        if (!gameManager.isTutorial())
        {
            enemy_unit.GetComponent<Enemy>().TakeDamage(NativeDamage);
        }
    }
    public void doSuperDMG()
    {
        enemy_unit.GetComponent<Enemy>().TakeDamage(superATKManager.GetHits());
    }

    // Shield
    private int shieldFactor = 1;
    public int GetShieldFactor()
    {
        return shieldFactor;
    }
    public void SetShieldFactor(int newShieldFactor)
    {
        shieldFactor = newShieldFactor;
    }
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
        currentShield += shieldFactor;
        capShield();
    }
    public void decreaseCurrentShield()
    {
        currentShield--;
        capShield();
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
    private int adrenalineFactor = 1;
    public int GetAdrenalineFactor()
    {
        return adrenalineFactor;
    }
    public void SetAdrenalineFactor(int newAdrenalineFactor)
    {
        adrenalineFactor = newAdrenalineFactor;
    }
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
    void capStamina()
    {
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        } else if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
    // Buffs
    public void doDamageBuff()
    {
        if (!gameManager.isTutorial())
        {
            _combat_UI.damageBuff("player");
        }
    }
    public void doSpeedBuff()
    {
        if (!gameManager.isTutorial())
        {
            _combat_UI.speedBuff("player");
        }
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
    public void ATK_SlowmoHit(float slowMo)
    {
        _cameraManager.playChrome();
        StartCoroutine(timeManager.slowMotion(slowMo, .2f));
    }
    public void blockHit()
    {
        audioManager.Play("Shield_metal");
        Time.timeScale = 0.2f;
    }
    public void returnTime()
    {
        Time.timeScale = 1;
    }
    public void Do_UI_in()
    {
        Combat_UI.move_UI_in();
    }
}

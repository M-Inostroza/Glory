using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private TimeManager TimeManager;
    private TargetManager targetManager; 
    private BattleSystem BS;
    private AudioManager audioManager;
    private TutorialManager TutorialManager;
    private CombatManager CombatManager;
    private cameraManager _cameraManager;
    private GoalManager _goalManager;
    
    [Header("Stats")]
    [SerializeField] private float maxSpeed, baseSpeed;
    private static float _maxStamina, currentStamina; 
    [SerializeField] private static int _maxHP, _currentHP;
    [SerializeField] private static int _adrenaline;
    [SerializeField] private int maxAdrenaline;
    [SerializeField] private int _nativeDamage;

    [Header("Effects")]
    [SerializeField] private ParticleSystem attackBlood;
    [SerializeField] private ParticleSystem attackSpark;

    [SerializeField] Animator _dummyAnimator;

    [SerializeField] private static int _maxShield, _currentShield;
    [SerializeField] private int critHits;

    [SerializeField] private DodgeManager dodgeManager;

    //Agility (Dodging)
    public bool missed = false;

    public Camera MainCamera;

    static Animator myAnim;

    // Current Enemy
    public GameObject enemy_unit;

    private void Start()
    {
        TutorialManager = FindObjectOfType<TutorialManager>();
        CombatManager = FindObjectOfType<CombatManager>();

        myAnim = GetComponent<Animator>();
        BS = FindObjectOfType<BattleSystem>();
        targetManager = FindObjectOfType<TargetManager>();
        TimeManager = FindObjectOfType<TimeManager>();
        audioManager = FindObjectOfType<AudioManager>();
        _cameraManager = FindObjectOfType<cameraManager>();
        _goalManager = FindObjectOfType<GoalManager>();

        ResetStats();
    }

    private void Update()
    {
        capHP();
        capStamina();
        CheckVictoryCondition();
    }

    public static void ResetStats()
    {
        _maxStamina = 100;
        _adrenaline = 0;
        _maxHP = 30;
        _maxShield = 4;

        currentStamina = _maxStamina;
        _currentHP = _maxHP;

        if (GameManager.isTutorial())
        {
            _currentShield = 0;
        } else
        {
            _currentShield = _maxShield / 2;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!missed)
        {
            missed = false;
            _currentHP -= dmg;
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
    public void BackToIdle()
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
        if (!GameManager.isTutorial())
        {
            if (BS.GetDeadEnemy() == false)
            {
                TimeManager.playerActionIcon.sprite = TimeManager.iconSprites[1];
                TimeManager.playerTimer.fillAmount = 1;
                FindObjectOfType<InputManager>().SetPlayerAction("none");
                TimeManager.fadeInUnitTimer();
                TimeManager.continueUnitTimer();
                myAnim.Play("Idle");
            }
            else
            {
                BackToIdle();
            }
        } else
        {
            TutorialManager.fadeTimer(1);
            TutorialManager.selectIcon("Default");
            BackToIdle();
        }
    }
    public void returnCamera()
    {
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .3f);
        MainCamera.DOFieldOfView(50, 0.3f);
    }
    public void stopHurt()
    {
        myAnim.SetBool("HURT", false);
    }
    public void stopDefendSkill()
    {
        myAnim.SetBool("DF_Skill", false);
        TimeManager.continueUnitTimer();
        TimeManager.defaultAction();
    }
    public void stopDodgeSkill()
    {
        myAnim.SetBool("evadeSuccess", false);
        if (!GameManager.isTutorial())
        {
            TimeManager.playerTimer.fillAmount = 1;
            TimeManager.continueUnitTimer();
            TimeManager.defaultAction();
        } else
        {
            TutorialManager.fadeTimer(1);
            TutorialManager.selectIcon("Default");
        }
    }
    public void stopDodgeSkillFail()
    {
        myAnim.SetBool("skillFail", false);
        if (GameManager.isTutorial())
        {
            TutorialManager.fadeTimer(1);
            TutorialManager.selectIcon("Default");
        } else
        {
            TimeManager.playerTimer.fillAmount = 1;
            TimeManager.continueUnitTimer();
            TimeManager.defaultAction();
        }
    }
    public void stopEvadeJump()
    {
        missed = false;
        myAnim.SetBool("evadeJump", false);
        TimeManager.fadeInUnitTimer();
    }
    public void stopDodgeIcon()
    {
        dodgeManager.deactivateDodgeBuff();
    }
    public void stopFocusSkill()
    {
        myAnim.SetBool("focusSuccess", false);
        if (!GameManager.isTutorial())
        {
            TimeManager.playerTimer.fillAmount = 1;
            TimeManager.continueUnitTimer();
            TimeManager.defaultAction();
        } else
        {
            TutorialManager.fadeTimer(1);
            TutorialManager.selectIcon("Default");
        }
    }
    public void stopShieldSuccess()
    {
        myAnim.SetBool("skillShieldSuccess", false);
        if (GameManager.isTutorial())
        {
            TutorialManager.fadeTimer(1);
            TutorialManager.selectIcon("Default");
        } else
        {
            CombatManager.shieldFeed();
            TimeManager.playerTimer.fillAmount = 1;
            TimeManager.continueUnitTimer();
            TimeManager.defaultAction();
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
        if (!GameManager.isTutorial())
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

    public void CheckVictoryCondition()
    {
        if (!GameManager.isTutorial() && enemy_unit != null)
        {
            myAnim.SetInteger("Victory", Enemy.GetCurrentHP());
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
        if (!GameManager.isTutorial())
        {
            BS.showHit(_nativeDamage, BS.hitText_Enemy.transform);
        }
    }
    public void showEnemySuperDamage()
    {
        BS.showHit(SuperATKManager.GetHits(), BS.hitText_Enemy.transform);
    }

    // ---------------- Getters and Setters ----------------

    // HP
    public int GetMaxHP()
    {
        return _maxHP;
    }
    public void SetMaxHP(int MaxHP)
    {
        _maxHP = MaxHP;
    }
    public int GetCurrentHP()
    {
        return _currentHP;
    }
    public void SetCurrentHP(int CurrentHP)
    {
        _currentHP = CurrentHP;
        if (_currentHP >= _maxHP)
        {
            _currentHP = _maxHP;
        }
    }

    // Stamina
    public float GetMaxStamina()
    {
        return _maxStamina;
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

    // Animation references
    public void deactivateAttackFeed()
    {
        GameObject attackFeedback = targetManager.GetAttackFeedback();
        foreach (Transform child in attackFeedback.GetComponentsInChildren<Transform>(true))
        {
            if (child == transform)
                continue;
            child.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(()=> attackFeedback.SetActive(false));
        }
        if (!GameManager.isTutorial())
        {
            targetManager.checkCritic();
        }
    }
    public void doDMG()
    {
        if (!GameManager.isTutorial())
        {
            enemy_unit.GetComponent<Enemy>().TakeDamage(NativeDamage);
        }
    }
    public void doSuperDMG()
    {
        enemy_unit.GetComponent<Enemy>().TakeDamage(SuperATKManager.GetHits());
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
        return _maxShield;
    }
    public void setMaxShield(int newShield)
    {
        _maxShield = newShield;
    }
    public int getCurrentShield()
    {
        return _currentShield;
    }
    public void setCurrentShield(int newShield)
    {
        _currentShield = newShield;
    }
    public void increaseCurrentShield()
    {
        if (DefendManager.GetShieldCritic())
        {
            _currentShield += shieldFactor + 1;
            DefendManager.SetShieldCritic(false);
        } else
        {
            _currentShield += shieldFactor;
        }
        capShield();
    }
    public void decreaseCurrentShield()
    {
        _currentShield--;
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
    private int adrenalineFactor = 2;
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
        return _adrenaline;
    }
    public void SetAdrenaline(int newAdrenaline)
    {
        _adrenaline = newAdrenaline;
    }

    public int GetMaxAdrenaline()
    {
        return maxAdrenaline;
    }
    public void incrementAdrenaline(int newAdrenaline)
    {
       _adrenaline  += newAdrenaline;
    }
    public void reduceAdrenaline(int newAdrenaline)
    {
        _adrenaline -= newAdrenaline;
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
        if (_currentShield <= 0)
        {
            _currentShield = 0;
        }
        else if (_currentShield >= _maxShield)
        {
            _currentShield = _maxShield;
        }
    }
    void capHP()
    {
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
        myAnim.SetInteger("Defeat", _currentHP);
    }
    void capStamina()
    {
        if (currentStamina <= 0)
        {
            currentStamina = 0;
        } else if (currentStamina >= _maxStamina)
        {
            currentStamina = _maxStamina;
        }
    }
    // Buffs
    public void doDamageBuff()
    {
        if (!GameManager.isTutorial())
        {
            CombatManager.damageBuff("player");
        }
    }
    public void doSpeedBuff()
    {
        if (!GameManager.isTutorial())
        {
            CombatManager.speedBuff("player");
        }
    }
    public IEnumerator boostSpeed()
    {
        baseSpeed += 3f;
        yield return new WaitForSeconds(3.5f);
        baseSpeed -= 3f;
    }

    // --------------------- Anim Effects --------------------- //
    public void PlayBloodMid()
    {
        if (!GameManager.isTutorial())
        {
            attackBlood.gameObject.SetActive(true);
        } else
        {
            attackSpark.gameObject.SetActive(true);
        }
    }
    public void shakeHit()
    {
        transform.DOShakePosition(0.3f, 0.2f, 22, 10, false, true);
    }
    public void ATK_SlowmoHit(float slowMo)
    {
        StartCoroutine(TimeManager.slowMotion(slowMo, .2f));
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
        CombatManager.move_UI_in();
    }

    public void PlayChrome()
    {
        _cameraManager.playChrome(3, 0.3f);
    }

    public void PlayBloom()
    {
        _cameraManager.PlayBloom(2, 0.5f);
    }
    // --------------------- Anim Effects --------------------- //
}

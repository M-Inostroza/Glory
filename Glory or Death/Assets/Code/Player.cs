using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private timeManager timeManager;
    private TargetManager targetManager;
    private BattleSystem BS;
    
    [SerializeField] private int maxHP, currentHP;
    [SerializeField] private float maxSpeed, baseSpeed;
    [SerializeField] private int adrenaline;
    [SerializeField] private int nativeDamage;
    [SerializeField] private int maxShield, currentShield;
    [SerializeField] private int critHits;

    //Stamina
    public float maxStamina;
    public float currentStamina;

    //Agility (Dodging)
    public bool missed = false;
    public GameObject dodgeBuffIcon;


    public Camera MainCamera;

    // Current Enemy
    public GameObject enemy_unit;

    
    private void Start()
    {
        BS = FindObjectOfType<BattleSystem>();
        targetManager = FindObjectOfType<TargetManager>();
        timeManager = FindObjectOfType<timeManager>();
    }

    public void TakeDamage(int dmg)
    {
        if (!missed)
        {
            missed = false;
            currentHP -= dmg;
            if (currentShield > 0)
            {
                decreaseCurrentShield();
                if (currentShield <= 0)
                {
                    currentShield = 0;
                }
            }
        }
        else
        {
            missed = true;
        }
    }

    // ------- Anim Methods -------
    public void nextAttack()
    {
        if (targetManager.attackOrder.Count > 0)
        {
            switch (targetManager.attackOrder.First())
            {
                case 0:
                    GetComponent<Animator>().Play("ATK_head");
                    targetManager.attackOrder.Remove(targetManager.attackOrder.First());
                    break;
                case 1:
                    GetComponent<Animator>().Play("ATK_mid");
                    targetManager.attackOrder.Remove(targetManager.attackOrder.First());
                    break;
                case 2:
                    GetComponent<Animator>().Play("ATK_bottom");
                    targetManager.attackOrder.Remove(targetManager.attackOrder.First());
                    break;
            }
        } else
        {
            GetComponent<Animator>().Play("ATK_back");
        }
    }
    public void stopAttack()
    {
        GetComponent<Animator>().Play("Idle");
        timeManager.playerTimer.fillAmount = 1;
        timeManager.continueTimer();
        timeManager.defaultAction();
    }
    public void returnCamera()
    {
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);
    }
    public void stopHurt()
    {
        gameObject.GetComponent<Animator>().SetBool("HURT", false);
    }
    public void stopDefendSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("DF_Skill", false);
        timeManager.continueTimer();
        timeManager.defaultAction();
    }
    public void stopDodgeSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("evadeSuccess", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.continueTimer();
        timeManager.defaultAction();
    }
    public void stopDodgeSkillFail()
    {
        GetComponent<Animator>().SetBool("skillFail", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopEvadeJump()
    {
        missed = false;
        GetComponent<Animator>().SetBool("evadeJump", false);
        timeManager.defaultAction();
    }
    public void stopDodgeIcon()
    {
        dodgeBuffIcon.GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        dodgeBuffIcon.transform.DOLocalMoveY(270, 0.8f).SetEase(Ease.OutExpo).OnComplete(() => dodgeBuffIcon.SetActive(false));
    }
    public void stopFocusSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("focusSuccess", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopShieldSuccess()
    {
        gameObject.GetComponent<Animator>().SetBool("skillShieldSuccess", false);
        FindObjectOfType<Combat_UI>().shieldFeed();
        timeManager.playerTimer.fillAmount = 1;
        timeManager.continueTimer();
        timeManager.defaultAction();
    }
    public void stopRest()
    {
        gameObject.GetComponent<Animator>().SetBool("Resting", false);
    }
    public void stopSuperDefend()
    {
        gameObject.GetComponent<Animator>().SetBool("DF2", false);
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
    public void incrementBaseSpeed(float BaseSpeed)
    {
        baseSpeed += BaseSpeed;
    }
    public void reduceBaseSpeed(float BaseSpeed)
    {
        baseSpeed -= BaseSpeed;
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
    public void reduceadrenaline(int newAdrenaline)
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
}

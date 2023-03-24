using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    //Time Manager
    private timeManager timeManager;
    //Target manager
    private TargetManager targetManager;

    //Name of the player *TODO* -> get name of player
    public string playerName;

    //Start damage
    public int native_damage;

    //HP
    public int maxHP;
    public int currentHP;

    //Speed
    public float maxSpeed;
    public float baseSpeed;

    //Adrenaline
    public int adrenaline;

    //Shield
    public int maxShield;
    public int currentShield;
    private shieldPool shieldPool;

    //Stamina
    public float maxStamina;
    public float currentStamina;

    //Agility (Dodging)
    public int maxAgility;
    public int currentAgility;
    public bool missed = false;
    public GameObject dodgeBuffIcon;

    public float evade;

    public Camera MainCamera;


    // Current Enemy
    public GameObject enemy_unit;
    //Making a change
    private void Start()
    {
        targetManager = FindObjectOfType<TargetManager>();
        timeManager = FindObjectOfType<timeManager>();
        shieldPool = FindObjectOfType<shieldPool>();
    }

    //this is a comment

    public bool TakeDamage(int dmg)
    {
        //Check Agility
        if (!missed)
        {
            missed = false;
            currentHP -= dmg;
            if (currentShield > 0)
            {
                shieldPool.RemoveShield();
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

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    // ------- Stop Anim Methods -------
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
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void returnCamera()
    {
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);
    }
    public void stopHurt()
    {
        gameObject.GetComponent<Animator>().SetBool("HURT1", false);
    }
    public void stopDefendSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("DF_Skill", false);
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopDodgeSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("DG_Skill", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopDodgeSkillFail()
    {
        GetComponent<Animator>().SetBool("DG_Skill_Fail", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }
    public void stopDodgeBuff()
    {
        missed = false;
        GetComponent<Animator>().SetBool("Evade", false);
        timeManager.defaultAction();
    }
    public void stopDodgeIcon()
    {
        dodgeBuffIcon.transform.DOScale(new Vector2(0, 0), 0.5f).SetEase(Ease.OutExpo).OnComplete(() => dodgeBuffIcon.SetActive(false));
    }

    public void stopFocusSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("FC_Skill", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }

    public void stopFocusSkillFail()
    {
        gameObject.GetComponent<Animator>().SetBool("FC_Skill_Fail", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.enemyTimerControl = true;
        timeManager.playerTimerControl = true;
        timeManager.defaultAction();
    }

    // TO DO

    public void stopSuperAttack()
    {
        gameObject.GetComponent<Animator>().SetBool("ATK2", false);
    }

    public void stopCounter()
    {
        gameObject.GetComponent<Animator>().SetBool("Counter", false);
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

    public void shakeHit()
    {
        transform.DOShakePosition(0.3f, 0.2f, 22, 10, false, true);
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

    public IEnumerator boostSpeed()
    {
        baseSpeed += 2;
        yield return new WaitForSeconds(3.5f);
        baseSpeed -= 2;
    }
}

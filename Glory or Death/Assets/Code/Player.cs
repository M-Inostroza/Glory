using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //Time Manager
    private timeManager timeManager;

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
    public shieldPool shieldPool;

    //Stamina
    public int maxStamina;
    public int currentStamina;

    //Agility (Dodging)
    public int maxAgility;
    public int currentAgility;
    public bool missed = false;

    public float evade;

    // Effects
    public ParticleSystem atk_normal_spark;
    public ParticleSystem atk_normal_blood;
    public ParticleSystem jump_dust;
    public ParticleSystem land_dust;
    public ParticleSystem groundJumpEffect;

    public ParticleSystem superHitEffect;
    public ParticleSystem superBloodEffect;

    AudioManager audioManager;
    public Camera MainCamera;


    // Current Enemy
    public GameObject enemy_unit;
    //Making a change
    private void Start()
    {
        timeManager = FindObjectOfType<timeManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

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
    public void stopAttack()
    {
        gameObject.GetComponent<Animator>().SetBool("ATK1", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
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
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
    }
    public void stopDodgeSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("DG_Skill", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
    }
    public void stopDodgeSkillFail()
    {
        gameObject.GetComponent<Animator>().SetBool("DG_Skill_Fail", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
    }

    public void stopFocusSkill()
    {
        gameObject.GetComponent<Animator>().SetBool("FC_Skill", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
    }

    public void stopFocusSkillFail()
    {
        gameObject.GetComponent<Animator>().SetBool("FC_Skill_Fail", false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
    }

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
        FindObjectOfType<BattleSystem>().switchToEnemy();
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
        transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);
    }

    public void playJump()
    {
        audioManager.Play("jump_basic");
        jump_dust.Play();
    }

    public void playSuperJump()
    {
        audioManager.Play("superJumpWosh");
        jump_dust.Play();
    }

    public void playLand()
    {
        audioManager.Play("fall");
        land_dust.Play();
    }

    public void playStab()
    {
        audioManager.Play("stab");
        atk_normal_spark.Play();
        atk_normal_blood.Play();
    }

    public void playSuperStab()
    {
        audioManager.Play("superStab");
        superHitEffect.Play();
        superBloodEffect.Play();
    }

    public void playEvadeAttack()
    {
        audioManager.Play("evadeAttack");
        groundJumpEffect.Play();
    }


    
    
}

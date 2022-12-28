using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //Name of the player *TODO* -> get name of player
    public string playerName;

    //Start damage
    public int native_damage;

    //HP
    public int maxHP;
    public int currentHP;

    //Adrenaline
    public int adrenaline;

    //Shield
    public int maxShield;
    public int currentShield;
    public Shield_Manager shield_manager;

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


    // Current Enemy
    public GameObject enemy_unit;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public bool TakeDamage(int dmg)
    {
        //Check Agility
        if (!missed)
        {
            missed = false;
            currentShield--;
            currentHP -= dmg;
            if (currentShield > 0)
            {
                shield_manager.destroyShield();
            } else if (currentShield <= 0)
            {
                currentShield = 0;
                shield_manager.gameObject.SetActive(false);
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

    public void stopAttack()
    {
        gameObject.GetComponent<Animator>().SetBool("ATK1", false);
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

    public void stopEvade()
    {
        gameObject.GetComponent<Animator>().SetBool("Evade", false);
    }

    public void stopDefend()
    {
        gameObject.GetComponent<Animator>().SetBool("DF", false);
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
        audioManager.Play("jumpWosh");
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

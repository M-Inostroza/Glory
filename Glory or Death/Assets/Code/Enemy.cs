using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    //Name of the enemy
    public string enemyName;

    //Start damage
    public int native_damage;

    //HP
    public int maxHP;
    public int currentHP;

    //Shield
    public int maxShield;
    public int currentShield;

    //Stamina
    public int maxStamina;
    public int currentStamina;

    public int adrenaline;

    //Agility (Dodging)
    public int maxAgility;
    public int currentAgility;

    public bool hasHit = false;

    private counterManager counterManager;

    // Effects
    /*public ParticleSystem sandEffect;
    public ParticleSystem hitEffect;

    public ParticleSystem hitShieldEffect;
    public ParticleSystem bloodShieldEffect;

    public ParticleSystem bloodEffect;
    public ParticleSystem superBloodEffect;

    public ParticleSystem hitStrong;*/

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        counterManager = FindObjectOfType<counterManager>();
    }

    public bool TakeDamage(int dmg)
    {
        transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);

        currentHP -= Math.Max(dmg - (currentShield > 0 ? 2 : 0), 0);
        currentShield = Math.Max(currentShield - 1, 0);

        return currentHP <= 0;
    }

    public void stopAttackBasic()
    {
        GetComponent<Animator>().SetBool("attack_basic", false);
    }

    public void stopHurtBasic()
    {
        gameObject.GetComponent<Animator>().SetBool("hurt_basic", false);
    }

    public void stopEnemyDefense()
    {
        GetComponent<Animator>().SetBool("Hurt", false);
    }


    public void playJump()
    {
        audioManager.Play("jumpWosh");
    }

    public void playAction()
    {
        if (FindObjectOfType<Player>().missed)
        {
            audioManager.Play("evadeWosh");
        }
        else
        {
            audioManager.Play("stab");
        }
    }

    public void attackStrong()
    {
        audioManager.Play("superStabEnemy");
    }

    public void shieldAttack()
    {
        audioManager.Play("shieldHitEnemy");
    }
    public void stopAttackStrong()
    {
        GetComponent<Animator>().SetBool("ATK2", false);
        adrenaline = 0;
    }

    // Experimental anim
    public void base_to_attack()
    {
        gameObject.GetComponent<Animator>().SetBool("attack", true);
    }
    
    public void base_to_fail()
    {
        gameObject.GetComponent<Animator>().SetBool("fail", true);
    }
    public void stop_fail()
    {
        gameObject.GetComponent<Animator>().SetBool("fail", false);
        gameObject.GetComponent<Animator>().SetBool("base", false);
    }

    public void hurt_player()
    {
        var player = FindObjectOfType<Player>();
        player.GetComponent<Animator>().SetBool("DF", true);

        if (player.currentShield > 0)
        {
            FindObjectOfType<BattleSystem>().showHit(native_damage - 2);
        } else
        {
            FindObjectOfType<BattleSystem>().showHit(native_damage);
        } 
    }


}

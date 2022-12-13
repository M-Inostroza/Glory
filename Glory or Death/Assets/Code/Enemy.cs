using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    //Name of the player *TODO* -> get name of player
    public string playerName;

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
    public ParticleSystem sandEffect;
    public ParticleSystem hitEffect;

    public ParticleSystem hitShieldEffect;
    public ParticleSystem bloodShieldEffect;

    public ParticleSystem bloodEffect;
    public ParticleSystem superBloodEffect;

    public ParticleSystem hitStrong;

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

    public void StopAttack()
    {
        GetComponent<Animator>().SetBool("ATK1", false);
    }

    public void stopEnemyDefense()
    {
        GetComponent<Animator>().SetBool("Hurt", false);
    }


    public void playJump()
    {
        audioManager.Play("jumpWosh");
        sandEffect.Play();
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
            hitEffect.Play();
            bloodEffect.Play();
        }
    }

    public void attackStrong()
    {
        hitStrong.Play();
        superBloodEffect.Play();
        audioManager.Play("superStabEnemy");
    }

    public void shieldAttack()
    {
        audioManager.Play("shieldHitEnemy");
        bloodShieldEffect.Play();
        hitShieldEffect.Play();
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

    
}

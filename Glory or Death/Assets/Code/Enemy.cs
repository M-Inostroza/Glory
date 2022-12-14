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

    private defendManager defendManager;

    // Effects
    public ParticleSystem jump_dust;
    public ParticleSystem atk_normal_spark;
    /*
    public ParticleSystem hitShieldEffect;
    public ParticleSystem bloodShieldEffect;

    public ParticleSystem bloodEffect;
    public ParticleSystem superBloodEffect;

    public ParticleSystem hitStrong;*/

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        defendManager = FindObjectOfType<defendManager>();
    }

    public bool TakeDamage(int dmg)
    {
        transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);

        currentHP -= Math.Max(dmg - (currentShield > 0 ? 2 : 0), 0);
        currentShield = Math.Max(currentShield - 1, 0);

        return currentHP <= 0;
    }

    public void executeAttack()
    {
        if (!FindObjectOfType<Player>().missed)
        {
            FindObjectOfType<Player>().GetComponent<Animator>().SetBool("HURT1", true);
            adrenaline += 2;
            if (FindObjectOfType<Player>().currentShield > 0)
            {
                bool isDead = FindObjectOfType<Player>().TakeDamage(native_damage - 2);
                //showHit(enemyUnit.native_damage - 2);

                if (isDead)
                {
                    FindObjectOfType<BattleSystem>().state = BattleState.LOST;
                    FindObjectOfType<BattleSystem>().EndBattle();
                }
                else
                    FindObjectOfType<BattleSystem>().state = BattleState.PLAYERTURN;
            }
            else
            {
                bool isDead = FindObjectOfType<Player>().TakeDamage(native_damage);
                FindObjectOfType<BattleSystem>().showHit(native_damage);
                if (isDead)
                {
                    FindObjectOfType<BattleSystem>().state = BattleState.LOST;
                    FindObjectOfType<BattleSystem>().EndBattle();
                }
                else
                    FindObjectOfType<BattleSystem>().state = BattleState.PLAYERTURN;
            }
        }
        else
        {
            FindObjectOfType<BattleSystem>().missHit();
            adrenaline++;
            FindObjectOfType<BattleSystem>().state = BattleState.PLAYERTURN;
        }
    }

    public void playJump()
    {
        audioManager.Play("jump_basic");
        jump_dust.Play();
    }
    public void playAttack()
    {
        atk_normal_spark.Play();
        audioManager.Play("enemy_attack_basic");
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

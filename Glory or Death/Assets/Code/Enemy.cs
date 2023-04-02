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

    //Speed
    public float maxSpeed;
    public float baseSpeed;

    //Stamina
    public float maxStamina;
    public float currentStamina;

    public int adrenaline;

    //Agility (Dodging)
    public int maxAgility;
    public int currentAgility;

    public bool hasHit = false;

    private defendManager defendManager;
    private timeManager timeManager;

    [SerializeField]
    private dirtToss dirtManager;

    // Effects
    public ParticleSystem jump_dust;
    public ParticleSystem atk_normal_spark;
    

    AudioManager audioManager;
    BattleSystem BS;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        BS = FindObjectOfType<BattleSystem>();
        timeManager = FindObjectOfType<timeManager>();
        defendManager = FindObjectOfType<defendManager>();
    }

    public bool TakeDamage(int dmg)
    {
        //transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);

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
                BS.showHit(native_damage - 2, "player");

                if (isDead)
                {
                    FindObjectOfType<BattleSystem>().EndBattle();
                }
            }
            else
            {
                bool isDead = FindObjectOfType<Player>().TakeDamage(native_damage);
                FindObjectOfType<BattleSystem>().showHit(native_damage, "player");
                if (isDead)
                {
                    FindObjectOfType<BattleSystem>().EndBattle();
                }
            }
        }
        else
        {
            FindObjectOfType<BattleSystem>().missHit();
            adrenaline++;
        }
    }
    public void executeDirt()
    {
        dirtManager.gameObject.SetActive(true);
        adrenaline += 2;
    }

    
    public void playAttack()
    {
        if (FindObjectOfType<Player>().missed)
        {
            FindObjectOfType<SoundPlayer>().jumpSounds();
            FindObjectOfType<Player>().GetComponent<Animator>().SetBool("Evade", true);
        } else
        {
            atk_normal_spark.Play();
            FindObjectOfType<SoundPlayer>().blunt_hit();
        }
    }

    //Stop anim controllers
    public void stopAttack()
    {
        timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
        timeManager.enemyTimer.fillAmount = 1;
        timeManager.playerTimerControl = true;
        timeManager.enemyTimerControl = true;
        GetComponent<Animator>().SetBool("attack", false);
    }
    public void stopHurt()
    {
        timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
        timeManager.playerTimerControl = true;
        timeManager.enemyTimerControl = true;
        GetComponent<Animator>().Play("Idle");
    }

    public void stopDirt()
    {
        timeManager.fadeInUnitTimer();
        timeManager.playerTimerControl = true;
        timeManager.enemyTimerControl = true;
        GetComponent<Animator>().Play("Idle");
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
}

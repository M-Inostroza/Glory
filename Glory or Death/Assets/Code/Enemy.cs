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

    private timeManager timeManager;

    [SerializeField]
    private dirtToss dirtManager;

    // Effects
    public ParticleSystem jump_dust;
    public ParticleSystem atk_normal_spark;

    [SerializeField]
    private Camera mainCamera;

    private bool isAngry = false;

    AudioManager audioManager;
    BattleSystem BS;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        BS = FindObjectOfType<BattleSystem>();
        timeManager = FindObjectOfType<timeManager>();
    }

    private void Update()
    {
        executeRage();
        limitHP();
    }

    private void limitHP()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    public bool TakeDamage(int dmg)
    {
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
                BS.showHit(native_damage - 2, BS.hitText_Player);

                if (isDead)
                {
                    FindObjectOfType<BattleSystem>().EndBattle();
                }
            }
            else
            {
                bool isDead = FindObjectOfType<Player>().TakeDamage(native_damage);
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

    public void executeRage()
    {
        if (currentHP < (maxHP / 2) && !isAngry)
        {
            executeCameraZoom();
            timeManager.stopUnitTimer();
            FindObjectOfType<Combat_UI>().move_UI_out();
            isAngry = true;
            native_damage += 2;
            baseSpeed += 4;
            GetComponent<Animator>().Play("Rage");
        }
    }
    public void returnFromRage()
    {
        returnCameraZoom();
        FindObjectOfType<Combat_UI>().move_UI_in();
        timeManager.continueTimer();
    }
    public void executeCameraZoom()
    {
        mainCamera.DOFieldOfView(40, 1f);
        mainCamera.transform.DOLocalMove(new Vector3(2.5f, -1, -10), 1f);
    }
    public void returnCameraZoom()
    {
        mainCamera.DOFieldOfView(50, 1f);
        mainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.7f);
    }

    public void playAudience()
    {
        audioManager.Play("Audience_cheer");
    }
    public void playGrunt()
    {
        audioManager.Play("Enemy_charge");
    }
    public void doDamageBuff()
    {
        FindObjectOfType<Combat_UI>().damageBuff();
    }
    public void doSpeedBuff()
    {
        FindObjectOfType<Combat_UI>().speedBuff();
    }
    public void testRage()
    {
        currentHP -= 10;
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
        timeManager.fadeInUnitTimer();
        timeManager.playerTimerControl = true;
        timeManager.enemyTimerControl = true;
        GetComponent<Animator>().SetBool("attack", false);
    }
    public void stopHurt()
    {
        timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
        timeManager.continueTimer();
        backToIdle();
    }

    public void stopDirt()
    {
        timeManager.fadeInUnitTimer();
        timeManager.continueTimer();
        backToIdle();
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

    public void backToIdle()
    {
        GetComponent<Animator>().Play("Idle");
    }
}

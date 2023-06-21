using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    public int nativeDamage;
    public int maxHP;
    public int currentHP;
    public int adrenaline;

    //Speed
    public float maxSpeed;
    public float baseSpeed;

    [SerializeField] dirtToss dirtManager;
    [SerializeField] CounterManager counterManager;

    [SerializeField]
    private Camera mainCamera;

    private bool isAngry = false;
    public bool hasHit = false;

    AudioManager audioManager;
    
    BattleSystem BS;
    timeManager timeManager;
    Player Player;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        BS = FindObjectOfType<BattleSystem>();
        timeManager = FindObjectOfType<timeManager>();
        Player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        limitHP();
    }

    private void limitHP()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
    }

    public void executeAttack()
    {
        if (!Player.missed)
        {
            adrenaline += 2;
            if (Player.getCurrentShield() > 0)
            {
                counterManager.gameObject.SetActive(true);
            }
            else
            {
                BS.showHit(nativeDamage, BS.hitText_Player.transform);
                GetComponent<Animator>().SetBool("attack", true);
                Player.GetComponent<Animator>().SetBool("HURT", true);
                Player.TakeDamage(nativeDamage);
            }
        }
        else
        {
            BS.missHit();
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
        executeCameraZoom();
        timeManager.stopUnitTimer();
        FindObjectOfType<Combat_UI>().move_UI_out();
        baseSpeed += 5;
        nativeDamage += 3;
        GetComponent<Animator>().Play("Rage");
    }
    
    
    public void playAttack()
    {
        if (FindObjectOfType<Player>().missed)
        {
            FindObjectOfType<SoundPlayer>().jumpSounds();
            FindObjectOfType<Player>().GetComponent<Animator>().SetBool("evadeJump", true);
        } else
        {
            FindObjectOfType<SoundPlayer>().blunt_hit();
        }
    }

    // Utilities
    public void stopAttack()
    {
        timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
        timeManager.enemyTimer.fillAmount = 1;
        timeManager.fadeInUnitTimer();
        timeManager.continueTimer();
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
        timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
        timeManager.enemyTimer.fillAmount = 1;
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

    public void returnFromRage()
    {
        returnCameraZoom();
        FindObjectOfType<Combat_UI>().move_UI_in();
        timeManager.continueTimer();
        timeManager.fadeInUnitTimer();
    }
    public void executeCameraZoom()
    {
        mainCamera.DOFieldOfView(45, 1f);
        mainCamera.transform.DOLocalMove(new Vector3(2.5f, -1, -10), 1f);
    }
    public void returnCameraZoom()
    {
        mainCamera.DOFieldOfView(50, 1f);
        mainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.7f);
    }

    public void playAudience()
    {
        audioManager.Play("Audience_boo");
    }
    public void playGrunt()
    {
        audioManager.Play("Enemy_charge");
    }
    public void doDamageBuff()
    {
        FindObjectOfType<Combat_UI>().damageBuff("enemy");
    }
    public void doSpeedBuff()
    {
        FindObjectOfType<Combat_UI>().speedBuff("enemy");
    }

    public void doCameraShake()
    {
        mainCamera.DOShakePosition(0.8f, 1, 80, 20);
    }
    public void testRage()
    {
        currentHP -= 20;
    }
    public bool getAngryState()
    {
        return isAngry;
    }
    public void setAngryState(bool newState)
    {
        isAngry = newState;
    }
}

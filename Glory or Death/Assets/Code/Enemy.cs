using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int nativeDamage;
    public int maxHP;
    public int currentHP;
    public int adrenaline;
    [SerializeField] int superDMG;

    //Speed
    public float maxSpeed;
    public float baseSpeed;

    [Header("Systems")]
    [SerializeField] dirtToss dirtManager;
    [SerializeField] CounterManager counterManager;
    [SerializeField] superAttackManager superAttackManager;

    [SerializeField]
    private Camera mainCamera;

    private bool isAngry = false;
    public bool hasHit = false;

    SoundPlayer soundPlayer;
    
    BattleSystem BS;
    timeManager timeManager;
    Player Player;
    Combat_UI combat_UI;
    cameraManager cameraManager;
    AudioManager audioManager;

    Animator myAnimator;
    Animator playerAnimator;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        cameraManager = FindObjectOfType<cameraManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        BS = FindObjectOfType<BattleSystem>();
        timeManager = FindObjectOfType<timeManager>();
        Player = FindObjectOfType<Player>();
        playerAnimator = Player.GetComponent<Animator>();
        myAnimator = GetComponent<Animator>();
        combat_UI = FindObjectOfType<Combat_UI>();

        currentHP = maxHP;
    }

    private void Update()
    {
        updateHP();
        updateSpeed();
        capHP();
    }

    private void updateHP()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
        }
        FindObjectOfType<EnemyHUD>().setHP(currentHP);
        myAnimator.SetInteger("CurrentHP", currentHP);
    }

    void updateSpeed()
    {
        if (dirtManager.IsDirty)
        {
            baseSpeed = 15;
        }
        else
        {
            baseSpeed = 13;
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
    }

    // Getter Setters
    public int GetCurrentAdrenaline()
    {
        return adrenaline;
    }

    public void executeAttack()
    {
        if (!Player.missed)
        {
            adrenaline += 4;
            if (Player.getCurrentShield() > 0 && !dirtManager.IsDirty)
            {
                audioManager.Play("Counter_On");
                counterManager.gameObject.SetActive(true);
            }
            else
            {
                myAnimator.SetBool("attack", true);
                Player.GetComponent<Animator>().SetBool("HURT", true);
            }
        }
        else
        {
            myAnimator.SetBool("attack", true);
            adrenaline+=2;
        }
    }
    public void executeSuperAttack()
    {
        if (!dirtManager.IsDirty)
        {
            superAttackManager.gameObject.SetActive(true);
        }
    }
    public void executeDirt()
    {
        dirtManager.gameObject.SetActive(true);
        adrenaline += 3;
    }
    public void executeRage(int speedBuff, int dmgBuff)
    {
        timeManager.stopUnitTimer();
        executeCameraZoom();
        Combat_UI.move_UI_out();
        baseSpeed += speedBuff;
        nativeDamage += dmgBuff;
        myAnimator.Play("Rage");
        adrenaline += 3;
    }


    // Utilities
    #region Combat Functions
    public void playAttack()
    {
        if (Player.missed)
        {
            soundPlayer.jumpSounds();
            playerAnimator.SetBool("evadeJump", true);
        }
        else
        {
            Player.TakeDamage(nativeDamage);
            if (Player.GetCurrentHP() <= 0)
            {
                playerAnimator.SetBool("HURT", false);
                playerAnimator.Play("Defeat");
            }
            soundPlayer.blunt_hit();
        }
    }
    public void doBlockedDMG()
    {
        Player.TakeDamage(nativeDamage - 2);
    }
    public void doSuperDMG()
    {
        Player.TakeDamage(superDMG);
    }
    public void stopAttack()
    {
        if (!BS.GetDeadPlayer())
        {
            timeManager.enemyTimer.fillAmount = 1;
            timeManager.fadeInUnitTimer();
            timeManager.continueUnitTimer();
            myAnimator.SetBool("attack", false);
        } else
        {
            backToIdle();
        }
    }
    public void stopSuperAttack()
    {
        adrenaline = 0;
        if (!BS.GetDeadPlayer())
        {
            timeManager.enemyTimer.fillAmount = 1;
            timeManager.fadeInUnitTimer();
            timeManager.continueUnitTimer();
            backToIdle();
        }
        else
        {
            backToIdle();
        }
    }
    public void stopHurt()
    {
        backToIdle();
    }
    public void playfanfare()
    {
        audioManager.Play("Victory_Sound");
    }
    #endregion

    public void stopDirt()
    {
        timeManager.enemyTimer.fillAmount = 1;
        timeManager.fadeInUnitTimer();
        timeManager.continueUnitTimer();
        backToIdle();
    }
    public void stopEnemyDefense()
    {
        myAnimator.SetBool("Hurt", false);
    }
    public void shieldAttack()
    {
        audioManager.Play("shieldHitEnemy");
    }
    public void backToIdle()
    {
        myAnimator.SetBool("attack", false);
        myAnimator.Play("Idle");
    }
    public void returnFromRage()
    {
        returnCameraZoom();
        Combat_UI.move_UI_in();
        timeManager.continueUnitTimer();
        timeManager.fadeInUnitTimer();
    }
    public void executeCameraZoom()
    {
        mainCamera.DOFieldOfView(40, 0.6f);
        mainCamera.transform.DOLocalMove(new Vector3(3, -0.3f, -10), 0.6f);
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

    public void selectNextAction()
    {
        timeManager.selectEnemyAction();
    }

    // Buffs
    public void doDamageBuff()
    {
        combat_UI.damageBuff("enemy");
    }
    public void doSpeedBuff()
    {
        combat_UI.speedBuff("enemy");
    }

    // Camera
    public void doCameraShake(int level)
    {
        if (level == 1)
        {
            mainCamera.DOShakePosition(0.8f, 1, 80, 20);
        } else {
            mainCamera.DOShakePosition(0.6f, 0.5f, 20, 10);
        }
    }
    public void doChrome()
    {
        cameraManager.playChrome();
    }
    public void doSlow()
    {
        StartCoroutine(timeManager.slowMotion(.2f, .2f));
    }
    public void doSuperATKzoom()
    {
        mainCamera.transform.DOLocalMoveX(-1.3f, 0.5f);
        mainCamera.transform.DOLocalMoveY(-0.5f, 0.5f);
        mainCamera.DOFieldOfView(40, 0.5f);
    }
    public void doSuperATKzoomReturn()
    {
        mainCamera.transform.DOLocalMoveX(0, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
        mainCamera.DOFieldOfView(50, 0.5f);
    }
    public void doUIOut()
    {
        Combat_UI.move_UI_out();
    }
    public void doUIIn()
    {
        if (!BS.GetDeadPlayer() && !dirtManager.IsDirty)
        {
            Combat_UI.move_UI_in();
        }
        else if (dirtManager.IsDirty)
        {
            Combat_UI.move_UI_in(false);
        }
    }

    // DMG Feedbacks
    public void showDmgFeedbackPlayer()
    {
        if (!Player.missed)
        {
            BS.showHit(nativeDamage, BS.hitText_Player.transform);
        } else
        {
            BS.showHit(nativeDamage, BS.missText_Player.transform);
        }
    }
    public void showDmgFeedbackPlayerReduced()
    {
        BS.showHit(nativeDamage - 2, BS.hitText_Player.transform);
    }
    public void showDmgFeedbackPlayerSuper(int step)
    {
        int quotient = superDMG / 3;
        int remainder = superDMG % 3;
        int firstATK = quotient + (remainder >= 1 ? 1 : 0);
        int secondATK = quotient + (remainder >= 2 ? 1 : 0);
        int thirdATK = quotient;
        switch (step)
        {
            case 1:
                BS.showHit(firstATK, BS.hitText_Player.transform);
                break;
            case 2:
                BS.showHit(secondATK, BS.hitText_Player.transform);
                break;
            case 3:
                BS.showHit(thirdATK, BS.hitText_Player.transform);
                break;
        }
    }

    // G & S
    public bool getAngryState()
    {
        return isAngry;
    }
    public void setAngryState(bool newState)
    {
        isAngry = newState;
    }
    public void setSuperDMG(int dmg)
    {
        superDMG = dmg;
    }

    // Caps
    void capHP()
    {
        if(currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }

}

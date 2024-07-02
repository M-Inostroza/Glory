using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    private static int _nativeDamage;
    private static int _maxHP;
    private static int _currentHP;
    public int adrenaline;
    [SerializeField] int superDMG;

    //Speed
    public float maxSpeed;
    public float baseSpeed; // Default 13

    [Header("Systems")]
    [SerializeField] dirtToss DirtManager;
    [SerializeField] CounterManager counterManager;
    [SerializeField] SuperCounterManager SuperCounterManager;

    [SerializeField]
    private Camera mainCamera;

    private bool isAngry = false;
    private bool criticBlock = false;
    public bool hasHit = false;

    SoundPlayer soundPlayer;
    
    BattleSystem BS;
    TimeManager TimeManager;
    Player Player;
    CombatManager CombatManager;
    AudioManager audioManager;
    cameraManager _cameraManager;
    EnemyHUD _enemyHud;

    Animator myAnimator;
    Animator playerAnimator;
    private void Awake()
    {
        ResetStats();
    }
    private void Start()
    {
        ImportScripts();
    }

    private void Update()
    {
        UpdateHP();
        UpdateSpeed();
        capHP();
    }

    private void UpdateHP()
    {
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
        _enemyHud.setHP(_currentHP);
        myAnimator.SetInteger("CurrentHP", _currentHP);
    }

    void UpdateSpeed()
    {
        if (DirtManager.IsDirty)
        {
            baseSpeed = 16;
        }
        else
        {
            baseSpeed = 13;
        }
    }

    public void TakeDamage(int dmg)
    {
        _currentHP -= dmg;
    }

    // Getter Setters
    public int GetCurrentAdrenaline()
    {
        return adrenaline;
    }

    public void ExecuteAttack()
    {
        if (!Player.missed)
        {
            if (Player.getCurrentShield() > 0 && !DirtManager.IsDirty)
            {
                adrenaline += 5;
                audioManager.Play("Counter_On");
                counterManager.gameObject.SetActive(true);
            }
            else
            {
                adrenaline += 6;
                myAnimator.SetBool("attack", true);
                Player.GetComponent<Animator>().SetBool("HURT", true);
            }
        }
        else
        {
            myAnimator.SetBool("attack", true);
            adrenaline+=5;
        }
    }
    public void executeSuperAttack()
    {
        if (!DirtManager.IsDirty)
        {
            SuperCounterManager.gameObject.SetActive(true);
        }
    }
    public void executeDirt()
    {
        DirtManager.gameObject.SetActive(true);
        adrenaline += 5;
    }
    public void ExecuteRage(int speedBuff, int dmgBuff)
    {
        TimeManager.stopUnitTimer();
        CameraZoom(true);
        CombatManager.move_UI_out();
        baseSpeed += speedBuff;
        _nativeDamage += dmgBuff;
        myAnimator.Play("Rage");
        adrenaline += 6;
    }


    // Utilities
    public void playAttack()
    {
        if (Player.missed)
        {
            soundPlayer.jumpSounds();
            playerAnimator.SetBool("evadeJump", true);
        }
        else
        {
            Player.TakeDamage(_nativeDamage);
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
        if (criticBlock)
        {
            Player.TakeDamage(0);
        } else
        {
            Player.TakeDamage(_nativeDamage / 2);
        }
    }
    public void doSuperDMG()
    {
        Player.TakeDamage(superDMG);
    }
    public void stopAttack()
    {
        if (!BS.GetDeadPlayer())
        {
            TimeManager.enemyTimer.fillAmount = 1;
            TimeManager.fadeInUnitTimer();
            TimeManager.continueUnitTimer();
            myAnimator.SetBool("attack", false);
        } else
        {
            BackToIdle();
        }
    }
    public void stopSuperAttack()
    {
        adrenaline = 0;
        if (!BS.GetDeadPlayer())
        {
            TimeManager.enemyTimer.fillAmount = 1;
            TimeManager.fadeInUnitTimer();
            TimeManager.continueUnitTimer();
            BackToIdle();
            CombatManager.move_UI_in();
        }
        else
        {
            BackToIdle();
        }
    }
    public void stopHurt()
    {
        BackToIdle();
    }
    public void playfanfare()
    {
        audioManager.Play("Victory_Sound");
    }

    public void stopDirt()
    {
        TimeManager.enemyTimer.fillAmount = 1;
        TimeManager.fadeInUnitTimer();
        TimeManager.continueUnitTimer();
        BackToIdle();
    }
    public void stopEnemyDefense()
    {
        myAnimator.SetBool("Hurt", false);
    }
    public void shieldAttack()
    {
        audioManager.Play("shieldHitEnemy");
    }
    public void BackToIdle()
    {
        myAnimator.SetBool("attack", false);
        myAnimator.Play("Idle");
    }
    
    public void CameraZoom(bool inOut)
    {
        if (inOut)
        {
            mainCamera.DOFieldOfView(40, 0.3f);
            mainCamera.transform.DOLocalMove(new Vector3(3, -0.3f, -10), 0.3f);
        } else
        {
            mainCamera.DOFieldOfView(50, 0.5f);
            mainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.4f);
        }
    }




    // ------------------- Anim Callbacks ------------------- //
    public void selectNextAction()
    {
        TimeManager.SelectEnemyAction();
    }
    public void playAudience()
    {
        audioManager.Play("Audience_boo");
    }
    public void playGrunt()
    {
        audioManager.Play("Enemy_charge");
    }
    public void returnFromRage()
    {
        CameraZoom(false);
        CombatManager.move_UI_in();
        TimeManager.continueUnitTimer();
        TimeManager.fadeInUnitTimer();
    }
    // ------------------- Anim Callbacks ------------------- //




    // Buffs
    public void doDamageBuff()
    {
        CombatManager.damageBuff("enemy");
    }
    public void doSpeedBuff()
    {
        CombatManager.speedBuff("enemy");
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
        _cameraManager.playChrome();
    }
    public void doSlow()
    {
        StartCoroutine(TimeManager.slowMotion(.2f, .2f));
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
        CombatManager.move_UI_out();
    }
    public void doUIIn()
    {
        // Calling from end animation super attack
        if (!BS.GetDeadPlayer() && !DirtManager.IsDirty)
        {
            CombatManager.move_UI_in();
        }
        else if (DirtManager.IsDirty)
        {
            CombatManager.move_UI_in(false);
        }
    }

    // DMG Feedbacks
    public void showDmgFeedbackPlayer()
    {
        if (!Player.missed)
        {
            BS.showHit(_nativeDamage, BS.hitText_Player.transform);
        } else
        {
            BS.showHit(_nativeDamage, BS.missText_Player.transform);
        }
    }
    public void showDmgFeedbackPlayerReduced()
    {
        if (criticBlock)
        {
            BS.showHit(0, BS.hitText_Player.transform);
        } else
        {
            BS.showHit(_nativeDamage - 2, BS.hitText_Player.transform);
        }
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
    public static int GetMaxHP()
    {
        return _maxHP;
    }
    public static int GetCurrentHP()
    {
        return _currentHP;
    }

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

    public bool GetCriticBlock()
    {
        return criticBlock;
    }
    public void SetCriticBlock(bool isCritic)
    {
        criticBlock = isCritic;
    }

    // Caps
    void capHP()
    {
        if(_currentHP >= _maxHP)
        {
            _currentHP = _maxHP;
        }
    }

    void ImportScripts()
    {
        _enemyHud = FindObjectOfType<EnemyHUD>();
        audioManager = FindObjectOfType<AudioManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        BS = FindObjectOfType<BattleSystem>();
        TimeManager = FindObjectOfType<TimeManager>();
        Player = FindObjectOfType<Player>();
        CombatManager = FindObjectOfType<CombatManager>();
        _cameraManager = FindObjectOfType<cameraManager>();
        playerAnimator = Player.GetComponent<Animator>();
        myAnimator = GetComponent<Animator>();
    }

    public void SetStartingStats()
    {
        _nativeDamage = 4; // Default
        baseSpeed = 13; // Default
        setAngryState(false);
        _currentHP += (int)(_maxHP * 0.3f);
        adrenaline = 0;
    }

    public void ResetStats()
    {
        adrenaline = 0;
        baseSpeed = 13;
        _maxHP = 40;
        _nativeDamage = 4;
        _currentHP = _maxHP;
    }

}

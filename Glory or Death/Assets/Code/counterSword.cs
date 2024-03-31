using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using AssetKits.ParticleImage;

public class counterSword : MonoBehaviour
{
    [SerializeField] GameObject counterManager, heart;
    [SerializeField] ParticleImage _criticStars;
    [SerializeField] ParticleImage _criticSparks;

    Player player;
    Enemy enemy;
    CombatManager combatUI;
    SoundPlayer soundPlayer;
    AudioManager audioManager;
    TutorialManager TutorialManager;
    CombatManager CombatManager;
    cameraManager _cameraManager;
    CounterManager _counterManager;

    [Header("Materials")]
    [SerializeField] Material heartMaterial;
    [SerializeField] Material swordMaterial;
    [SerializeField] Material shieldMaterial;

    bool canCollide;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        combatUI = FindObjectOfType<CombatManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        CombatManager = FindObjectOfType<CombatManager>();
        _cameraManager = FindObjectOfType<cameraManager>();
        _counterManager = FindObjectOfType<CounterManager>();
    }
    private void OnEnable()
    {
        canCollide = true;
        transform.GetComponent<PolygonCollider2D>().enabled = true;
        transform.DOLocalMoveX(12, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        counterManager.GetComponent<CounterManager>().canRotateBool(false);
        if (canCollide)
        {
            switch (collision.name)
            {
                case "Shield Image":
                    canCollide = false;
                    if (!GameManager.isTutorial())
                    {
                        enemy.SetCriticBlock(false);
                        combatUI.shakeShieldBar();
                        enemy.GetComponent<Animator>().Play("Attack_Blocked");
                        player.GetComponent<Animator>().Play("blockAttack");
                    }
                    else
                    {
                        TutorialManager.counterDetailTutorial(3);
                    }
                    player.incrementAdrenaline(player.GetAdrenalineFactor());
                    soundPlayer.shield_metal();
                    transform.DOKill();
                    transform.GetComponent<PolygonCollider2D>().enabled = false;
                    _counterManager.fadeElements(.2f, false);
                    canCollide = false;
                    StartCoroutine(TimeManager.slowMotion(.3f, .3f, () =>
                    {
                        counterManager.SetActive(false);
                    }));
                    break;

                case "Counter Target":
                    canCollide = false;
                    if (GameManager.isTutorial())
                    {
                        TutorialManager.counterDetailTutorial(2);
                    }
                    else
                    {
                        enemy.SetCriticBlock(false);
                    }
                    transform.GetComponent<PolygonCollider2D>().enabled = false;
                    _cameraManager.playChrome();
                    audioManager.Play("Counter_Fail");
                    soundPlayer.stabSounds();
                    _cameraManager.PlayBloom(2);
                    meltHeart();
                    break;

                case "Critic":
                    canCollide = false;
                    if (!GameManager.isTutorial())
                    {
                        enemy.SetCriticBlock(true);
                        _criticSparks.Play();
                        CombatManager.showStars();
                        combatUI.shakeShieldBar();
                        enemy.GetComponent<Animator>().Play("Attack_Blocked");
                        player.GetComponent<Animator>().Play("blockAttack");
                    }
                    else
                    {
                        TutorialManager.counterDetailTutorial(3);
                    }
                    player.incrementAdrenaline(player.GetAdrenalineFactor()+2);
                    transform.GetComponent<PolygonCollider2D>().enabled = false;
                    audioManager.Play("Critic Counter");
                    _cameraManager.playChrome();
                    _cameraManager.PlayBloom(1);
                    transform.DOKill();
                    _counterManager.fadeElements(.2f, false);
                    StartCoroutine(TimeManager.slowMotion(.3f, .3f, () =>
                    {
                        _criticStars.Play();
                        counterManager.SetActive(false);
                    }));
                    break;
            }
        }
    }


    void meltHeart()
    {
        StartCoroutine(TimeManager.slowMotion(0.4f, 0.3f));
        DOTween.To(() => heartMaterial.GetFloat("_FadeAmount"), x => heartMaterial.SetFloat("_FadeAmount", x), 1, 0.5f)
            .OnComplete(()=> playAnims_closeGame());
        DOTween.To(() => swordMaterial.GetFloat("_FadeAmount"), x => swordMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
        DOTween.To(() => shieldMaterial.GetFloat("_FadeAmount"), x => shieldMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
    }
    void playAnims_closeGame()
    {
        if (!GameManager.isTutorial())
        {
            enemy.GetComponent<Animator>().SetBool("attack", true);
            player.GetComponent<Animator>().SetBool("HURT", true);
        }
        counterManager.SetActive(false);
    }
}

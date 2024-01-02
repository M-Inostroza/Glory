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

    Player player;
    Enemy enemy;
    Combat_UI combatUI;
    SoundPlayer soundPlayer;
    AudioManager audioManager;
    Tutorial_UI tutorial_UI;

    [Header("Materials")]
    [SerializeField] Material heartMaterial;
    [SerializeField] Material swordMaterial;
    [SerializeField] Material shieldMaterial;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        combatUI = FindObjectOfType<Combat_UI>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
    }
    private void OnEnable()
    {
        transform.DOLocalMoveX(12, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        counterManager.GetComponent<CounterManager>().canRotateBool(false);
        switch (collision.name)
        {
            case "Shield Image":
                if (!gameManager.isTutorial())
                {
                    combatUI.shakeShieldBar();
                    enemy.GetComponent<Animator>().Play("Attack_Blocked");
                    player.GetComponent<Animator>().Play("blockAttack");
                }
                else
                {
                    tutorial_UI.counterDetailTutorial(3);
                }
                soundPlayer.shield_metal();
                counterManager.SetActive(false);
                break;
            case "Counter Target":
                if (gameManager.isTutorial())
                {
                    tutorial_UI.counterDetailTutorial(2);
                }
                cameraManager.playChrome();
                audioManager.Play("Counter_Fail");
                soundPlayer.stabSounds();
                meltHeart();
                break;
            case "Critic":
                Debug.Log("Critic Counter");
                if (!gameManager.isTutorial())
                {
                    _criticStars.Play();
                    combatUI.shakeShieldBar();
                    enemy.GetComponent<Animator>().Play("Attack_Blocked");
                    player.GetComponent<Animator>().Play("blockAttack");
                }
                else
                {
                    tutorial_UI.counterDetailTutorial(3);
                }
                soundPlayer.shield_metal();
                counterManager.SetActive(false);
                break;
        }
        /*if (collision.name == "Shield Image")
        {
            if (!gameManager.isTutorial())
            {
                combatUI.shakeShieldBar();
                enemy.GetComponent<Animator>().Play("Attack_Blocked");
                player.GetComponent<Animator>().Play("blockAttack");
            } else
            {
                tutorial_UI.counterDetailTutorial(3);
            }
            soundPlayer.shield_metal();
            counterManager.SetActive(false);
        }
        else if (collision.name == "Counter Target")
        {
            if (gameManager.isTutorial())
            {
                tutorial_UI.counterDetailTutorial(2);
            }
            cameraManager.playChrome();
            audioManager.Play("Counter_Fail");
            soundPlayer.stabSounds();
            meltHeart();
        }
        else if (collision.name == "shieldStop")
        {
            counterManager.GetComponent<CounterManager>().canRotateBool(false);
        }*/
    }

    void meltHeart()
    {
        StartCoroutine(timeManager.slowMotion(0.4f, 0.3f));
        DOTween.To(() => heartMaterial.GetFloat("_FadeAmount"), x => heartMaterial.SetFloat("_FadeAmount", x), 1, 0.5f)
            .OnComplete(()=> playAnims_closeGame());
        DOTween.To(() => swordMaterial.GetFloat("_FadeAmount"), x => swordMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
        DOTween.To(() => shieldMaterial.GetFloat("_FadeAmount"), x => shieldMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
    }
    void playAnims_closeGame()
    {
        if (!gameManager.isTutorial())
        {
            enemy.GetComponent<Animator>().SetBool("attack", true);
            player.GetComponent<Animator>().SetBool("HURT", true);
        }
        counterManager.SetActive(false);
    }
}

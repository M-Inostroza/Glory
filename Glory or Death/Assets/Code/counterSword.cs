using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class counterSword : MonoBehaviour
{
    [SerializeField] GameObject counterManager, heart;

    Player player;
    Enemy enemy;
    Combat_UI combatUI;
    SoundPlayer soundPlayer;

    [Header("Materials")]
    [SerializeField] Material heartMaterial;
    [SerializeField] Material swordMaterial;
    [SerializeField] Material shieldMaterial;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        combatUI = FindObjectOfType<Combat_UI>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }
    private void OnEnable()
    {
        transform.DOLocalMoveX(12, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        counterManager.GetComponent<CounterManager>().canRotateBool(false);
        
        if (collision.name == "Shield Image")
        {
            combatUI.shakeShieldBar();
            soundPlayer.shield_metal();
            enemy.GetComponent<Animator>().Play("Attack_Blocked");
            player.GetComponent<Animator>().Play("blockAttack");
            counterManager.SetActive(false);
        }
        else if (collision.name == "Counter Target")
        {
            FindObjectOfType<cameraManager>().playChrome();
            AudioManager.Play("Counter_Fail");
            soundPlayer.stabSounds();
            meltHeart();
        }
        else if (collision.name == "shieldStop")
        {
            counterManager.GetComponent<CounterManager>().canRotateBool(false);
        }
    }

    void meltHeart()
    {
        FindObjectOfType<timeManager>().executeSlowMotion(0.6f, 0.3f);
        DOTween.To(() => heartMaterial.GetFloat("_FadeAmount"), x => heartMaterial.SetFloat("_FadeAmount", x), 1, 0.5f)
            .OnComplete(()=> playAnims_closeGame());
        DOTween.To(() => swordMaterial.GetFloat("_FadeAmount"), x => swordMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
        DOTween.To(() => shieldMaterial.GetFloat("_FadeAmount"), x => shieldMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
    }
    void playAnims_closeGame()
    {
        enemy.GetComponent<Animator>().SetBool("attack", true);
        player.GetComponent<Animator>().SetBool("HURT", true);
        counterManager.SetActive(false);
    }
}

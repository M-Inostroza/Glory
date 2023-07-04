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
    AudioManager audioManager;

    [SerializeField] Material heartMaterial, swordMaterial;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        combatUI = FindObjectOfType<Combat_UI>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("create a stop for the shields rotation");
        Debug.Log("and design a new one");
        transform.DOLocalMoveX(12, 0);
        if (collision.name == "Shield Image")
        {
            player.decreaseCurrentShield();
            combatUI.shakeShieldBar();
            soundPlayer.shield_metal();
            enemy.GetComponent<Animator>().Play("Attack_Blocked");
            player.GetComponent<Animator>().Play("blockAttack");
            player.TakeDamage(enemy.nativeDamage - 2);
            counterManager.SetActive(false);
        }
        else if (collision.name == "Counter Target")
        {
            FindObjectOfType<cameraManager>().playChrome();
            audioManager.Play("Counter_Fail");
            soundPlayer.stabSounds();
            meltHeart();
            player.TakeDamage(enemy.nativeDamage);
        }
    }

    void meltHeart()
    {
        FindObjectOfType<timeManager>().executeSlowMotion(0.6f, 0.3f);
        DOTween.To(() => heartMaterial.GetFloat("_FadeAmount"), x => heartMaterial.SetFloat("_FadeAmount", x), 1, 0.5f)
            .OnComplete(()=> playAnims_closeGame());
        DOTween.To(() => swordMaterial.GetFloat("_FadeAmount"), x => swordMaterial.SetFloat("_FadeAmount", x), 1, 0.5f);
    }
    void playAnims_closeGame()
    {
        enemy.GetComponent<Animator>().SetBool("attack", true);
        player.GetComponent<Animator>().SetBool("HURT", true);
        counterManager.SetActive(false);
    }
}

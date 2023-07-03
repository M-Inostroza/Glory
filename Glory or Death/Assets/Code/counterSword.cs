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

    Material heartMaterial;
    [SerializeField] Image heartImage;

    private void Awake()
    {
        heartMaterial = heartImage.material;
    }
    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        combatUI = FindObjectOfType<Combat_UI>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            soundPlayer.stabSounds();
            enemy.GetComponent<Animator>().SetBool("attack", true);
            player.GetComponent<Animator>().SetBool("HURT", true);
            player.TakeDamage(enemy.nativeDamage);
            meltHeart();
        }
    }

    void meltHeart()
    {
        DOTween.To(() => heartMaterial.GetFloat("_FadeAmount"), x => heartMaterial.SetFloat("_FadeAmount", x), 1, 1)
            .OnComplete(()=> counterManager.SetActive(false));
    }
}

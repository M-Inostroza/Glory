using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class counterSword : MonoBehaviour
{
    [SerializeField]
    GameObject counterManager;

    Player player;
    Enemy enemy;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.DOLocalMoveX(12, 0);
        if (collision.name == "Shield Image")
        {
            player.decreaseCurrentShield();
            FindObjectOfType<Combat_UI>().shakeShieldBar();
            enemy.GetComponent<Animator>().Play("Attack_Blocked");
            player.GetComponent<Animator>().Play("blockAttack");
            player.TakeDamage(enemy.nativeDamage - 2);
            counterManager.SetActive(false);
        }
        else if (collision.name == "Counter Target")
        {
            enemy.GetComponent<Animator>().SetBool("attack", true);
            player.GetComponent<Animator>().SetBool("HURT", true);
            player.TakeDamage(enemy.nativeDamage);
            counterManager.SetActive(false);
        }
    }
}

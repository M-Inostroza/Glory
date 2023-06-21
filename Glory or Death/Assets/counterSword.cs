using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterSword : MonoBehaviour
{
    [SerializeField]
    GameObject counterManager;

    Player player;
    Enemy enemy;
    BattleSystem BS;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        BS = FindObjectOfType<BattleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shield Image")
        {
            player.decreaseCurrentShield();
            FindObjectOfType<Combat_UI>().shakeShieldBar();
            enemy.GetComponent<Animator>().Play("Attack_Blocked");
            player.GetComponent<Animator>().Play("blockAttack");
            player.TakeDamage(enemy.nativeDamage - 2);
            BS.showHit(enemy.nativeDamage - 2, BS.hitText_Player.transform);
            counterManager.SetActive(false);
        }
        else if (collision.name == "Counter Target")
        {
            BS.showHit(enemy.nativeDamage, BS.hitText_Player.transform);
            enemy.GetComponent<Animator>().SetBool("attack", true);
            player.GetComponent<Animator>().SetBool("HURT", true);
            player.TakeDamage(enemy.nativeDamage);
            counterManager.SetActive(false);
        }
    }
}

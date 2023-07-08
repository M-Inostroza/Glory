using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class superSword : MonoBehaviour
{
    Player player;
    Enemy enemy;
    Combat_UI combatUI;
    SoundPlayer soundPlayer;
    AudioManager audioManager;

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
        if (collision.name == "shield")
        {
            Debug.Log("shield");
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
        else if (collision.name == "Heart")
        {
            Debug.Log("heart");
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
    }
}

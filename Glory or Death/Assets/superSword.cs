using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class superSword : MonoBehaviour
{
    SoundPlayer soundPlayer;

    private void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "shield")
        {
            Debug.Log("shield");
            soundPlayer.shield_metal();
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
        else if (collision.name == "Heart")
        {
            Debug.Log("heart");
            FindObjectOfType<superAttackManager>().fillSword();
            soundPlayer.stabSounds();
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
    }
}

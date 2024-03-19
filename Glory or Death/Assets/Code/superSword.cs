using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class superSword : MonoBehaviour
{
    SoundPlayer soundPlayer;
    superAttackManager superATK;
    TutorialManager TutorialManager;

    private void Start()
    {
        superATK = FindObjectOfType<superAttackManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        TutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "shield")
        {
            soundPlayer.shield_metal();
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
        else if (collision.name == "Heart")
        {
            superATK.fillSword();
            soundPlayer.stabSounds();
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
    }
}

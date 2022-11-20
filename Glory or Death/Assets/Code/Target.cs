using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    //Anim
    Animator anim;
    CircleCollider2D colider;

    //BattleSystem
    public BattleSystem BattleSystem;

    //Audio
    AudioMAnager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioMAnager>();
        anim = gameObject.GetComponent<Animator>();
        colider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnMouseDown()
    {
        audioManager.Play("targetHit");
        colider.enabled = false;
        anim.SetBool("hit", true);
        BattleSystem.targetHit++;
        FindObjectOfType<Player>().adrenaline += 3;
    }

    void killTarget()
    {
        anim.Rebind();
        colider.enabled = true;
        gameObject.SetActive(false);
    }
}

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

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        colider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnMouseDown()
    {
        colider.enabled = false;
        anim.SetBool("hit", true);
        BattleSystem.targetHit++;
    }

    void killTarget()
    {
        anim.Rebind();
        colider.enabled = true;
        gameObject.SetActive(false);
    }
}

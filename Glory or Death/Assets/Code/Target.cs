using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    //Anim
    Animator anim;

    //BattleSystem
    public BattleSystem BattleSystem;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        anim.SetBool("hit", true);
        BattleSystem.targetHit++;
    }

    void killTarget()
    {
        anim.Rebind();
        gameObject.SetActive(false);
    }
}

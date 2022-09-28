using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    //Anim
    Animator anim;
    public GameObject timer;

    //BattleSystem
    public BattleSystem BS;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        anim.SetBool("hit", true);
        timer.SetActive(false);
        BS.targetHit++;
    }

    void killTarget()
    {
        gameObject.SetActive(false);
    }
}

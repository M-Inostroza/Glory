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
        FindObjectOfType<SoundPlayer>().targetSounds();
        switch (tag)
        {
            case "target_0":
                FindObjectOfType<TargetManager>().attackOrder.Add(0);
                break;
            case "target_1":
                FindObjectOfType<TargetManager>().attackOrder.Add(1);
                break;
            case "target_2":
                FindObjectOfType<TargetManager>().attackOrder.Add(2);
                break;
        }
        colider.enabled = false;
        anim.SetBool("hit", true);
        BattleSystem.targetHit++;
        FindObjectOfType<Player>().adrenaline += 1;
    }

    void killTarget()
    {
        anim.Rebind();
        colider.enabled = true;
        gameObject.SetActive(false);
    }
}

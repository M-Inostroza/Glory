using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public GameObject vFeedback;
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
                Debug.Log("Adding Head");
                animateFeedback(0);
                break;
            case "target_1":
                FindObjectOfType<TargetManager>().attackOrder.Add(1);
                Debug.Log("Adding Torso");
                animateFeedback(1);
                break;
            case "target_2":
                FindObjectOfType<TargetManager>().attackOrder.Add(2);
                Debug.Log("Adding Leg");
                animateFeedback(2);
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

    void animateFeedback(int target)
    {
        vFeedback.transform.GetChild(target).transform.GetComponent<Image>().DOFade(1, 0.5f);
        vFeedback.transform.GetChild(target).transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.4f, 8, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    public GameObject timer;

    //Parameters
    private float minSize;
    public float speed;

    //Anim
    Tween timerAnim;
    
    SpriteRenderer render;
    Animator anim;

    //Effect
    public GameObject hitEffect;

    //BattleSystem
    public BattleSystem BS;

    private void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        anim = hitEffect.GetComponent<Animator>();
        minSize = 1f;
    }
    private void OnEnable()
    {
        render.DOFade(1, 0.1f);
        timerAnim = timer.transform.DOScale(minSize, speed).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        timerAnim.Rewind();
    }

    private void OnMouseDown()
    {
        timer.SetActive(false);
        render.DOFade(0, 0.1f);
        anim.Play(0);
        BS.targetHit++;

        StartCoroutine(killTarget());
    }


    IEnumerator killTarget()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}

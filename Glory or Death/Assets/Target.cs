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

    //Effect
    public GameObject hitEffect;

    //BattleSystem
    public BattleSystem BS;

    private void OnEnable()
    {
        minSize = 0.25f;
        timerAnim = timer.transform.DOScale(minSize, speed).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnMouseDown()
    {
        Instantiate<GameObject>(hitEffect, transform.position, transform.rotation);
        BS.targetHit++;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        timerAnim.Rewind();
    }
}

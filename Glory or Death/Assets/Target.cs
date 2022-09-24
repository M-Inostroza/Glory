using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    public GameObject timer;

    private Tween scale;

    private void OnEnable()
    {
        scale = timer.transform.DOScale(1, 1).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        scale.Rewind();
    }
}

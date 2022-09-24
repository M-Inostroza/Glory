using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    public Transform timer;

    private void OnEnable()
    {
        timer = transform.Find("timer");
        timer.DOScale(1, 2);
        Invoke("kill", 2f);
    }
    void kill()
    {
        gameObject.SetActive(false);
    }

}

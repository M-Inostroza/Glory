using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class counterManager : MonoBehaviour
{
    //Scale tween
    private Tween scaleDown;

    [SerializeField]
    private GameObject shadow;

    [SerializeField]
    private ParticleSystem fire_hit;

    private void Start()
    {
        scaleDown = transform.DOScale(1, 3.5f).SetEase(Ease.InBack);
    }

    private void OnEnable()
    {
        shadow.SetActive(true);
        scaleDown.Play();
    }
    private void Update()
    {
        scaleDown.Play().OnComplete(() => killEverything());
        executeShield(0.9f);
    }

    void executeShield(float scaleLimit)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            scaleDown.Kill();

            if (transform.localScale.x < scaleLimit)
            {
                Debug.Log("Too soon");
                transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => killEverything());
            }
            else if (transform.localScale.x > scaleLimit)
            {
                StartCoroutine(hitShield());
            }
            shadow.SetActive(false);
        }
    }

    void killEverything()
    {
        scaleDown.Restart();
        shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator hitShield()
    {
        fire_hit.Play();
        
        yield return new WaitForSeconds(0.3f);
        killEverything();
    }
}



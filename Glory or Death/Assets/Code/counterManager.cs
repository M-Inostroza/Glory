using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class counterManager : MonoBehaviour
{
    private bool canPress;
    private Tween scaleDown;

    [SerializeField]
    private GameObject timer;

    [SerializeField]
    private ParticleSystem fire_loop, fire_hit;

    private void OnEnable()
    {
        canPress = true;
        fire_loop.Play();
        timer.SetActive(true);
        scaleDown = transform.DOScale(1, 3.5f);
    }
    private void Update()
    {
        scaleDown.Play().OnComplete(() => { killSprites(); fire_loop.Stop(); });
        executeShield(0.8f);
    }

    void executeShield(float scaleLimit)
    {
        if (Input.GetKeyDown(KeyCode.Space) && canPress)
        {
            fire_loop.Stop();
            scaleDown.Kill();

            canPress = false;
            if (transform.localScale.x < scaleLimit)
            {
                Debug.Log("Too soon");
                transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => killSprites());
            }
            else if (transform.localScale.x > scaleLimit)
            {
                StartCoroutine(hitShield());
            }
            timer.SetActive(false);
        }
    }

    void killSprites()
    {
        scaleDown.Rewind();
        timer.SetActive(false);
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator hitShield()
    {
        fire_hit.Play();
        
        yield return new WaitForSeconds(0.3f);
        killSprites();
    }
}



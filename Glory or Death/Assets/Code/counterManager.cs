using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class counterManager : MonoBehaviour
{
    //Scale tween
    private Tween scaleDown;
    private AudioManager audioManager;

    [SerializeField]
    private Animator playerAnim;

    [SerializeField]
    private GameObject shadow;

    [SerializeField]
    private ParticleSystem fire_hit;

    public GameObject enemyUnit;


    private void Start()
    {
        scaleDown = transform.DOScale(1, 3f).SetEase(Ease.InOutQuad);
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        shadow.SetActive(true);
        scaleDown.Play();
    }

    private void OnDisable()
    {
        transform.DOScale(0.01f, 0.1f);
        scaleDown.Rewind();
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
            scaleDown.Pause();

            if (transform.localScale.x < scaleLimit)
            {
                audioManager.Play("counter_fail");
                transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => killEverything());
                enemyUnit.GetComponent<Animator>().SetBool("attack", true);
            }
            else if (transform.localScale.x > scaleLimit)
            {
                enemyUnit.GetComponent<Animator>().SetBool("fail", true);
                playerAnim.SetBool("Counter", true);
                StartCoroutine(hitShield());
                audioManager.Play("counter_buff");
            }
            shadow.SetActive(false);
        }
    }

    void killEverything()
    {
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



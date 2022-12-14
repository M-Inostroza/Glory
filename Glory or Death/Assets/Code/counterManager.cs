using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class counterManager : MonoBehaviour
{
    //Scale tween
    private Tween scaleDown;
    private AudioManager audioManager;

    //Animator of player
    [SerializeField]
    private Animator playerAnim;

    //Object that shows the total size of the target
    [SerializeField]
    private GameObject shadow, playerUnit;

    //Fire effect that shows when success
    [SerializeField]
    private ParticleSystem fire_hit;

    //Enemy unit
    public GameObject enemyUnit;

    //Control bool
    public bool canCounter;
    public bool counterSuccess;


    private void Start()
    {
        scaleDown = transform.DOScale(1, 2f).SetEase(Ease.InOutQuad);
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        counterSuccess = false;
        canCounter = true;
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
        if (canCounter)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                 scaleDown.Pause();
                 canCounter = false;
                 if (transform.localScale.x < scaleLimit)
                 {
                    enemyUnit.GetComponent<Animator>().SetBool("attack", true);
                    audioManager.Play("counter_fail");
                    transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => killEverything());
                 }
                 else if (transform.localScale.x > scaleLimit)
                 {
                    counterSuccess = true;
                    enemyUnit.GetComponent<Animator>().SetBool("fail", true);
                    playerAnim.SetBool("Counter", true);
                    StartCoroutine(hitShield());
                    audioManager.Play("counter_buff");
                 }
                 shadow.SetActive(false);
            }
        }

        if (transform.localScale.x >= 1)
        {
            killEverything();
        }
    }

    void killEverything()
    {
        enemyUnit.GetComponent<Animator>().SetBool("attack", true);
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



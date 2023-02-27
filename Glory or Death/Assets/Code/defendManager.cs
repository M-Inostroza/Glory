using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class defendManager : MonoBehaviour
{
    // Scale tween
    private Tween scaleUp;

    // Animator of player
    [SerializeField] private Animator playerAnim;

    // Audio Manager
    [SerializeField] private AudioManager audioManager;

    // Object that shows the total size of the target
    [SerializeField] private GameObject shadow, playerUnit;

    // Fire effect that shows when success
    [SerializeField] private ParticleSystem success_hit;

    // Battle System
    [SerializeField] private BattleSystem BS;

    // Shield Pool
    [SerializeField] private shieldPool shieldPool;


    // Enemy unit
    public GameObject enemyUnit;

    // Control bool
    public bool defendSuccess;

    private void OnEnable()
    {
        audioManager.Play("DF_charge");
        defendSuccess = false;
        shadow.SetActive(true);
        transform.DOScale(1, 2f).SetEase(Ease.InOutQuad).OnComplete(() => Fail());
    }

    private void OnDisable()
    {
        transform.DOScale(0, 0.1f);
    }

    private void Update()
    {
        controlDefend();
    }

    // Method to handle player's defense
    void executeShield(float scaleLimit)
    {
        if (transform.localScale.x < scaleLimit )
        {
            transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => Fail());
        }
        else if (transform.localScale.x > scaleLimit)
        {
            defendSuccess = true;
            shieldPool.AddShield();
            playerUnit.GetComponent<Player>().currentShield++;
            playerAnim.SetBool("DF_Skill", true);
            audioManager.Play("defend_success");
            success_hit.Play();
            shadow.SetActive(false);
            gameObject.SetActive(false);
        }
        shadow.SetActive(false);
    }

    // Method to handle enemy's defeat
    void Fail()
    {
        playerAnim.SetBool("DG_Skill_Fail", true);
        audioManager.Play("defend_fail");
        shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    void controlDefend()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            executeShield(0.85f);
        }
    }

    /*void controlDefend()
    {
        if (Input.touchCount > 0)
        { // if there's at least one touch
            Touch touch = Input.GetTouch(0); // get the first touch
            if (touch.phase == TouchPhase.Began)
            { // if the touch just started
                executeShield(0.85f); // execute the shield action
            }
        }
    }*/
}



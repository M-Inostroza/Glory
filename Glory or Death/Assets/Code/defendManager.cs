using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class defendManager : MonoBehaviour
{
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
    bool transformControl;

    Tween scaleUP;

    private void OnEnable()
    {
        scaleUP = transform.DOScale(1, 2f).SetEase(Ease.InOutQuad);
        audioManager.Play("DF_charge");
        defendSuccess = false;
        shadow.SetActive(true);
        transformControl = true;
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
            transform.DOShakePosition(0.4f, 0.05f, 40);
            scaleUP.Rewind();
            Fail();
        }
        else if (transform.localScale.x > scaleLimit)
        {
            audioManager.Play("defend_success");
            scaleUP.Rewind();
            defendSuccess = true;
            shieldPool.AddShield();
            playerUnit.GetComponent<Player>().currentShield++;
            playerAnim.SetBool("DF_Skill", true);
            success_hit.Play();
        } 
        closeMinigame();
    }

    // Method to handle enemy's defeat
    void Fail()
    {
        audioManager.Play("defend_fail");
        playerAnim.SetBool("DG_Skill_Fail", true);
        closeMinigame();
    }

    void closeMinigame()
    {
        shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    void controlDefend()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            executeShield(0.85f);
        }

        if (transform.localScale.x == 1 && transformControl)
        {
            scaleUP.Rewind();
            Fail();
        }
    }
    private void OnDisable()
    {
        transformControl = false;
    }

    // Touch Controls
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



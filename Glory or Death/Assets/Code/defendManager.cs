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

    private void Start()
    {
        // Set the tween
        scaleUp = transform.DOScale(1, 2f).SetEase(Ease.InOutQuad);
    }

    private void OnEnable()
    {
        audioManager.Play("DF_charge");
        defendSuccess = false;
        shadow.SetActive(true);
    }

    private void OnDisable()
    {
        transform.DOScale(0.01f, 0.1f);
        scaleUp.Rewind();
    }

    private void Update()
    {
        scaleUp.Play().OnComplete(() => KillOnFail());
        controlDefend();
    }

    // Method to handle player's defense
    void executeShield(float scaleLimit)
    {
        scaleUp.Pause();
        if (transform.localScale.x < scaleLimit)
        {
            playerAnim.SetBool("DG_Skill_Fail", true);
            audioManager.Play("defend_fail");
            transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => KillOnFail());
        }
        else if (transform.localScale.x > scaleLimit)
        {
            defendSuccess = true;
            shieldPool.AddShield();
            playerUnit.GetComponent<Player>().currentShield++;
            playerAnim.SetBool("DF_Skill", true);
            StartCoroutine(HitShield());
            audioManager.Play("defend_success");
        }
        shadow.SetActive(false);
    }

    // Method to handle enemy's defeat
    void KillOnFail()
    {
        shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    // Coroutine to play hit effect and kill everything on success
    IEnumerator HitShield()
    {
        success_hit.Play();

        yield return new WaitForSeconds(0.3f);
        KillOnFail();
    }

    void controlDefend()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            executeShield(0.9f);
        }
    }
}



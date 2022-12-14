using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private ParticleSystem fire_hit;

    // Battle System
    [SerializeField] private BattleSystem BS;

    // Time Manager
    [SerializeField] private timeManager timeManager;

    // Enemy unit
    public GameObject enemyUnit;

    // Control bool
    public bool canDefend = false;
    public bool defendSuccess;

    private void Start()
    {
        // Set the tween
        scaleUp = transform.DOScale(1, 2f).SetEase(Ease.InOutQuad);
        timeManager = FindObjectOfType<timeManager>();
    }

    private void OnEnable()
    {
        audioManager.Play("defend_charge");
        defendSuccess = false;
        shadow.SetActive(true);
        canDefend = true;
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
            audioManager.Play("defend_fail");
            transform.DOShakePosition(0.4f, 0.05f, 40).OnComplete(() => KillOnFail());
        }
        else if (transform.localScale.x > scaleLimit)
        {
            defendSuccess = true;
            playerAnim.SetBool("DF_Skill", true);
            StartCoroutine(HitShield());
            audioManager.Play("defend_success");
        }
        shadow.SetActive(false);
    }

    // Method to handle enemy's defeat
    void KillOnFail()
    {
        Debug.Log("Kill on fail executing");
        BS.switchToEnemy();
        shadow.SetActive(false);
        timeManager.playerTimer.fillAmount = 1;
        timeManager.can_perform_player = true;
        gameObject.SetActive(false);
    }

    // Coroutine to play hit effect and kill everything on success
    IEnumerator HitShield()
    {
        fire_hit.Play();

        yield return new WaitForSeconds(0.3f);
        KillOnFail();
    }

    void controlDefend()
    {
        if (Input.GetKeyDown(KeyCode.A) && canDefend)
        {
            executeShield(0.9f);
            canDefend = false;
        }
    }
}



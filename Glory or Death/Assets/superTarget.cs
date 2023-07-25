using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class superTarget : MonoBehaviour
{
    BattleSystem BS;
    superATKManager SAM;
    SoundPlayer soundPlayer;
    timeManager timeManager;
    CircleCollider2D colider;

    private void Awake()
    {
        colider = GetComponent<CircleCollider2D>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        SAM = FindObjectOfType<superATKManager>();
        BS = FindObjectOfType<BattleSystem>();
        timeManager = FindObjectOfType<timeManager>();
    }
    private void OnEnable()
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.1f);
        StartCoroutine(deactivate(1.8f));
    }
    IEnumerator deactivate(float n)
    {
        yield return new WaitForSeconds(n);
        colider.enabled = false;
        transform.DOScale(new Vector3(0, 0, 0), 0.1f).OnComplete(kill);
        void kill()
        {
            transform.DOKill();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SAM.IncrementSpawnCounter();
    }

    private void OnMouseDown()
    {
        if (!BS.GetGamePaused())
        {
            timeManager.enemyTimer.fillAmount += 0.02f;
            soundPlayer.targetSounds();
            transform.DOScale(1.2f, 0.1f);
            SAM.IncrementHits();
            GetComponent<Image>().DOFade(0, 0.1f).OnComplete(()=>Destroy(gameObject));
        }
    }
}

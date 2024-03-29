using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class superTarget : MonoBehaviour
{
    SuperATKManager SuperATKManager;
    SoundPlayer soundPlayer;
    TimeManager TimeManager;
    CircleCollider2D colider;

    Image _targetImage;

    private void Awake()
    {
        _targetImage = GetComponent<Image>();
        colider = GetComponent<CircleCollider2D>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        SuperATKManager = FindObjectOfType<SuperATKManager>();
        TimeManager = FindObjectOfType<TimeManager>();
    }
    private void OnEnable()
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.1f);
        StartCoroutine(deactivate(1));
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
        SuperATKManager.IncrementSpawnCounter();
    }

    private void OnMouseDown()
    {
        if (!BattleSystem.IsPaused)
        {
            soundPlayer.targetSounds();
            if (!gameManager.isTutorial())
            {
                TimeManager.enemyTimer.fillAmount += 0.01f;
            }
            
            transform.DOScale(1.2f, 0.1f).OnComplete(()=> transform.DOKill());
            SuperATKManager.activateFeedSwords();
            SuperATKManager.IncrementHits();
            _targetImage.DOFade(0, 0.1f).OnComplete(()=>Destroy(gameObject));
        }
    }
}

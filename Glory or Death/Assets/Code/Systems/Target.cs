using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public GameObject vFeedback;
    
    CircleCollider2D colider;
    TargetManager targetManager;
    SoundPlayer soundPlayer;
    BattleSystem BS;
    timeManager _timeManager;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        targetManager = FindObjectOfType<TargetManager>();
        colider = gameObject.GetComponent<CircleCollider2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        BS = FindObjectOfType<BattleSystem>();
        _timeManager = FindObjectOfType<timeManager>();
    }

    private void OnMouseDown()
    {
        if (!BattleSystem.IsPaused)
        {
            if (!gameManager.isTutorial())
            {
                _timeManager.enemyTimer.fillAmount += 0.03f;
                BS.targetHit++;
                if (BS.targetHit == 3)
                {
                    // Fix add sound
                    // StartCoroutine(timeManager.slowMotion(.3f, .5f));
                    FindObjectOfType<cameraManager>().PlayBloom(1);
                }
            }
            soundPlayer.targetSounds();
            switch (tag)
            {
                case "target_0":
                    targetManager.attackOrder.Add(0);
                    animateFeedback(0);
                    break;
                case "target_1":
                    targetManager.attackOrder.Add(1);
                    animateFeedback(1);
                    break;
                case "target_2":
                    targetManager.attackOrder.Add(2);
                    animateFeedback(2);
                    break;
            }
            colider.enabled = false;
            transform.DOScale(1.2f, 0.1f);
            _spriteRenderer.DOFade(0, 0.1f).OnComplete(() => killTarget());
        }
    }

    void animateFeedback(int target)
    {
        vFeedback.transform.GetChild(target).transform.GetComponent<Image>().DOFade(1, 0.5f);
        vFeedback.transform.GetChild(target).transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.4f, 8, 1);
    }

    public void killTarget()
    {
        gameObject.SetActive(false);
        colider.enabled = true;
    }
}

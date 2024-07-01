using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public GameObject vFeedback;
    
    CircleCollider2D colider;
    TargetManager TargetManager;
    SoundPlayer soundPlayer;
    BattleSystem BS;
    TimeManager _TimeManager;
    SpriteRenderer _spriteRenderer;
    Player Player;
    TutorialManager TutorialManager;

    private void Awake()
    {
        TutorialManager = FindObjectOfType<TutorialManager>();
        Player = FindObjectOfType<Player>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        TargetManager = FindObjectOfType<TargetManager>();
        colider = gameObject.GetComponent<CircleCollider2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        BS = FindObjectOfType<BattleSystem>();
        _TimeManager = FindObjectOfType<TimeManager>();
    }

    private void OnMouseDown()
    {
        if (!BattleSystem.IsPaused)
        {
            if (!GameManager.isTutorial())
            {
                _TimeManager.enemyTimer.fillAmount += 0.03f;
                Player.incrementAdrenaline(3);
                BS.targetHit++;
            } else
            {
                TutorialManager.AddTutorialHits();
            }
            soundPlayer.targetSounds();
            switch (tag)
            {
                case "target_0":
                    TargetManager.attackOrder.Add(0);
                    animateFeedback(0);
                    break;
                case "target_1":
                    TargetManager.attackOrder.Add(1);
                    animateFeedback(1);
                    break;
                case "target_2":
                    TargetManager.attackOrder.Add(2);
                    animateFeedback(2);
                    break;
            }
            colider.enabled = false;
            transform.DOScale(1.2f, 0.1f);
            _spriteRenderer.DOFade(0, 0.2f).OnComplete(() => killTarget());
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

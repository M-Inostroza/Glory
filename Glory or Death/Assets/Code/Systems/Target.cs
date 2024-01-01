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
    Player _player;
    timeManager _timeManager;

    private void Awake()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        targetManager = FindObjectOfType<TargetManager>();
        colider = gameObject.GetComponent<CircleCollider2D>();
        BS = FindObjectOfType<BattleSystem>();
        _player = FindObjectOfType<Player>();
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
            GetComponent<SpriteRenderer>().DOFade(0, 0.1f).OnComplete(() => killTarget());
            _player.incrementAdrenaline(_player.GetAdrenalineFactor());
        }
    }

    void animateFeedback(int target)
    {
        vFeedback.transform.GetChild(target).transform.GetComponent<Image>().DOFade(1, 0.5f);
        vFeedback.transform.GetChild(target).transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.4f, 8, 1);
    }

    public void killTarget()
    {
        //transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
        colider.enabled = true;
    }
}

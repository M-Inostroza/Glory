using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public GameObject vFeedback;
    
    Animator anim;
    CircleCollider2D colider;
    BattleSystem BattleSystem;

    private void Start()
    {
        BattleSystem = FindObjectOfType<BattleSystem>();
        anim = gameObject.GetComponent<Animator>();
        colider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnMouseDown()
    {
        FindObjectOfType<SoundPlayer>().targetSounds();
        switch (tag)
        {
            case "target_0":
                FindObjectOfType<TargetManager>().attackOrder.Add(0);
                animateFeedback(0);
                break;
            case "target_1":
                FindObjectOfType<TargetManager>().attackOrder.Add(1);
                animateFeedback(1);
                break;
            case "target_2":
                FindObjectOfType<TargetManager>().attackOrder.Add(2);
                animateFeedback(2);
                break;
        }
        colider.enabled = false;
        anim.SetBool("hit", true);
        BattleSystem.targetHit++;
        FindObjectOfType<Player>().incrementAdrenaline(1);
    }

    void animateFeedback(int target)
    {
        vFeedback.transform.GetChild(target).transform.GetComponent<Image>().DOFade(1, 0.5f);
        vFeedback.transform.GetChild(target).transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.4f, 8, 1);
    }

    public void killTarget()
    {
        anim.Rebind();
        gameObject.SetActive(false);
        colider.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    /*
     - 0 -> attack head
     - 1 -> attack mid
     - 2 -> attack bottom
    */

    public GameObject[] targets;
    public List<int> attackOrder = new List<int>();

    private BattleSystem BattleSystem;
    private Camera MainCamera;
    private Combat_UI combat_UI;

    [SerializeField] SpriteRenderer courtain;
    [SerializeField] GameObject vFeedback;
    [SerializeField] float targetScale, wait_time;
    
    private void Start()
    {
        BattleSystem = FindObjectOfType<BattleSystem>();
        MainCamera = FindObjectOfType<Camera>();
        combat_UI = FindObjectOfType<Combat_UI>();
    }

    public void attack()
    {
        combat_UI.move_UI_out();
        vFeedback.SetActive(true);
        foreach (Transform child in vFeedback.transform)
        {
            child.transform.GetComponent<Image>().DOFade(.25f, 0);
            child.transform.DOScale(1, 0.3f).SetEase(Ease.InBack);
        }
        BattleSystem.targetHit = 0;

        // Courtain
        courtain.DOColor(new Color(0, 0, 0, .5f), 1f);
        StartCoroutine(activateTargets());

        // Camera
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 3f);
        MainCamera.DOFieldOfView(35, 3f);
    }

    IEnumerator activateTargets()
    {
        var targets = this.targets;
        var targetScale = this.targetScale;
        var wait_time = this.wait_time;

        for (int i = 0; i < 3; i++)
        {
            targets[i].SetActive(true);

            if (i == 0)
            {
                targets[i].transform.localPosition.Set(Random.Range(-1f, 1.5f), Random.Range(3f, 1.5f), 0);
            }
            else if (i == 1)
            {
                targets[i].transform.localPosition = new Vector3(Random.Range(-1.1f, 2.4f), Random.Range(1.3f, -0.4f), 0);
            }
            else
            {
                targets[i].transform.localPosition = new Vector3(Random.Range(-1, 2.6f), Random.Range(-1.4f, -2.2f), 0);
            }

            // Set target position & scale
            targets[i].transform.DOScale(targetScale, 0.2f);
        }

        // Deactivates after timer (Mejorable)!!
        yield return new WaitForSeconds(wait_time);

        for (int i = 0; i < 3; i++)
        {
            targets[i].transform.DOKill();
            targets[i].transform.DOScale(0, 0.3f).OnComplete(() => targets[i].gameObject.SetActive(false));
        }

        courtain.DOColor(new Color(0, 0, 0, 0), .5f);
        combat_UI.GetComponent<Combat_UI>().move_UI_in();
    }

    // Getters and Setters
    public GameObject GetAttackFeedback()
    {
        return vFeedback;
    }
}

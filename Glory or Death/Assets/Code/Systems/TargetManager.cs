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

    [SerializeField] SpriteRenderer courtain;
    [SerializeField] GameObject combat_UI;
    [SerializeField] GameObject vFeedback;
    [SerializeField] float targetScale, wait_time;
    
    private void Start()
    {
        BattleSystem = FindObjectOfType<BattleSystem>();
        MainCamera = FindObjectOfType<Camera>();
    }

    public void attack()
    {
        combat_UI.GetComponent<Combat_UI>().move_UI_out();
        vFeedback.SetActive(true);
        BattleSystem.targetHit = 0;

        // Courtain
        courtain.DOColor(new Color(0, 0, 0, .5f), 1f);
        StartCoroutine(activateTargets());

        // Camera
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 3f);
        MainCamera.DOFieldOfView(35, 3f);
    }

    public void attackHard()
    {
        BattleSystem.targetHit = 0;
        // Courtain
        courtain.DOColor(new Color(0, 0, 0, .5f), 1f);

        // Start target mechanic
        StartCoroutine(activateTargetsHard());

        // Camera effect
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 2);
        MainCamera.DOFieldOfView(35, 1.5f);
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
    IEnumerator activateTargetsHard()
    {
        var targets = this.targets;
        var targetScale = this.targetScale;
        var wait_time = this.wait_time;

        for (int i = 0; i < targets.Length; i++)
        {
            // Activate Target
            targets[i].SetActive(true);

            if (i == 0)
            {
                targets[i].transform.localPosition.Set(0, Random.Range(0.3f, 0.7f), 0);
            }
            else if (i == 1)
            {
                targets[i].transform.localPosition = new Vector3(Random.Range(-0.1f, -0.6f), 0, 0);
            }
            else
            {
                targets[i].transform.localPosition = new Vector3(Random.Range(0.3f, -0.2f), Random.Range(-0.3f, -0.7f), 0);
            }

            // Set target position & scale
            targets[i].transform.DOScale(targetScale, 0.3f);

            yield return new WaitForSeconds(wait_time);
        }

        // Deactivates the targets after timer
        yield return new WaitForSeconds(1.4f);

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].transform.DOKill();
            targets[i].transform.DOScale(0, 0.3f).OnComplete(() => targets[i].gameObject.SetActive(false));
        }

        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);

        // Courtain
        courtain.DOColor(new Color(0, 0, 0, 0), .5f);

    }
}

using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets;

    public BattleSystem BattleSystem;

    public Camera MainCamera;

    public SpriteRenderer courtain;

    [SerializeField]
    private float targetScale, wait_time;

    public void attack()
    {
        BattleSystem.targetHit = 0;
        // Courtain
        courtain.DOColor(new Color(0, 0, 0, .5f), 1f);

        // Start target mechanic
        StartCoroutine(activateTargets());

        // Camera effect
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
        //10 calls

        for (int i = 0; i < targets.Length - 3; i++)
        {
            // Activate Target
            targets[i].SetActive(true);
             //20 calls

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
                targets[i].transform.localPosition = new Vector3(Random.Range(0.7f, -0.5f), Random.Range(-0.8f, -1.2f), 0);
            }

            // Set target position & scale
            targets[i].transform.DOScale(targetScale, 0.3f);

            yield return new WaitForSeconds(wait_time);
        }

        // Deactivates the targets after timer
        yield return new WaitForSeconds(1.4f);

        for (int i = 0; i < targets.Length - 3; i++)
        {
            targets[i].transform.DOKill();
            targets[i].transform.DOScale(0, 0.3f).OnComplete(() => targets[i].gameObject.SetActive(false));
        }

        // Courtain
        courtain.DOColor(new Color(0, 0, 0, 0), .5f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets;

    public float wait_time;

    public BattleSystem BattleSystem;

    public Camera MainCamera;

    public Player playerScript;

    public SpriteRenderer courtain;

    private void OnEnable()
    {
        // Courtain
        courtain.DOColor(new Color(0, 0, 0, .5f), 1f);

        if (playerScript.adrenaline >= 20)
        {
            attackHard();
        } else
        {
            attack();
        }
    }

    void attack()
    {
        BattleSystem.targetHit = 0;

        // Start target mechanic
        StartCoroutine(activateTargets());

        // Camera effect
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 1);
        MainCamera.DOFieldOfView(35, 0.5f);
    }

    void attackHard()
    {
        BattleSystem.targetHit = 0;

        // Start target mechanic
        StartCoroutine(activateTargetsHard());

        // Camera effect
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 2);
        MainCamera.DOFieldOfView(35, 1.5f);
    }

    IEnumerator activateTargets()
    {
        // Activate Target
        targets[0].SetActive(true);
        targets[0].transform.localPosition.Set(0, Random.Range(0.3f, 0.7f), 0);
        // Set target position & scale
        targets[0].transform.DOScale(0.3f, 0.3f);
        
        yield return new WaitForSeconds(wait_time);

        targets[1].SetActive(true);
        targets[1].transform.localPosition = new Vector3(Random.Range(-0.1f, -0.6f), 0, 0);
        targets[1].transform.DOScale(0.3f, 0.3f);

        yield return new WaitForSeconds(wait_time);

        targets[2].SetActive(true); 
        targets[2].transform.localPosition = new Vector3(Random.Range(0.3f, -0.2f), Random.Range(-0.3f, -0.7f), 0);
        targets[2].transform.DOScale(0.3f, 0.3f);

        // Deactivates the targets after timer
        yield return new WaitForSeconds(1.4f);
        
        foreach (var target in targets)
        {
            target.transform.DOScale(0, 0.3f).OnComplete(() => target.SetActive(false));   
        }
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);

        // Courtain
        courtain.DOColor(new Color(0, 0, 0, 0), .5f);

        // Kill target manager
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    IEnumerator activateTargetsHard()
    {
        // Activate Target
        targets[0].SetActive(true);
        targets[0].transform.localPosition.Set(0, Random.Range(0.3f, 0.7f), 0);
        // Set target position & scale
        targets[0].transform.DOScale(0.3f, 0.3f);
        //targets[0].transform.localPosition = new Vector3(0, Random.Range(0.3f, 0.7f), 0);

        yield return new WaitForSeconds(0.15f);

        targets[1].SetActive(true);
        targets[1].transform.localPosition = new Vector3(Random.Range(-0.1f, -0.6f), 0, 0);
        targets[1].transform.DOScale(0.3f, 0.3f);

        yield return new WaitForSeconds(0.15f);

        targets[2].SetActive(true);
        targets[2].transform.localPosition = new Vector3(Random.Range(0.3f, -0.2f), Random.Range(-0.3f, -0.7f), 0);
        targets[2].transform.DOScale(0.3f, 0.3f);

        yield return new WaitForSeconds(0.15f);
        // Activate Target
        targets[3].SetActive(true);
        targets[3].transform.localPosition.Set(0, Random.Range(0.2f, 0.6f), 0);
        // Set target position & scale
        targets[3].transform.DOScale(0.3f, 0.3f);
        //targets[0].transform.localPosition = new Vector3(0, Random.Range(0.3f, 0.7f), 0);

        yield return new WaitForSeconds(0.15f);

        targets[4].SetActive(true);
        targets[4].transform.localPosition = new Vector3(Random.Range(-0.2f, -0.7f), 0, 0);
        targets[4].transform.DOScale(0.3f, 0.3f);

        yield return new WaitForSeconds(0.15f);

        targets[5].SetActive(true);
        targets[5].transform.localPosition = new Vector3(Random.Range(0.4f, -0.3f), Random.Range(-0.2f, -0.8f), 0);
        targets[5].transform.DOScale(0.3f, 0.3f);

        // Deactivates the targets after timer
        yield return new WaitForSeconds(1.5f);

        foreach (var target in targets)
        {
            target.transform.DOScale(0, 0.3f).OnComplete(() => target.SetActive(false));
        }
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);

        // Courtain
        courtain.DOColor(new Color(0, 0, 0, 0), .5f);

        // Kill target manager
        yield return new WaitForSeconds(1);
        playerScript.adrenaline = 0;
        gameObject.SetActive(false);
    }
}

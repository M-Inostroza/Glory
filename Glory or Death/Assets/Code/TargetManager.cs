using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets;

    public float wait_time;

    public BattleSystem BS;

    public Camera MainCamera;

    private void OnEnable()
    {
        StartCoroutine(activateTargets());
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -1, -10), 1);
        MainCamera.DOFieldOfView(35, 0.5f);
    }

    IEnumerator activateTargets()
    {
        //Activate Targets
        targets[0].SetActive(true);
        targets[0].transform.localPosition = new Vector3(0, Random.Range(0.3f, 0.7f), 0);
        targets[0].transform.DOScale(0.3f, 0.5f);
        yield return new WaitForSeconds(wait_time);

        targets[1].SetActive(true);
        targets[1].transform.DOScale(0.3f, 0.5f);
        targets[1].transform.localPosition = new Vector3(Random.Range(-0.1f, -0.6f), 0, 0);
        yield return new WaitForSeconds(wait_time);

        targets[2].SetActive(true);
        targets[2].transform.DOScale(0.3f, 0.5f);
        targets[2].transform.localPosition = new Vector3(Random.Range(0.3f, -0.2f), Random.Range(-0.3f, -0.7f), 0);

        //Deactivate myself
        yield return new WaitForSeconds(2);
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), .5f);
        MainCamera.DOFieldOfView(50, 0.5f);

        targets[0].transform.DOScale(0f, 0.5f);
        targets[1].transform.DOScale(0f, 0.5f);
        targets[2].transform.DOScale(0f, 0.5f);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        BS.targetHit = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets;

    public float wait_time;

    private void OnEnable()
    {
        StartCoroutine(activateTargets());
    }

    IEnumerator activateTargets()
    {
        targets[0].SetActive(true);
        yield return new WaitForSeconds(wait_time);
        targets[1].SetActive(true);
        yield return new WaitForSeconds(wait_time);
        targets[2].SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class shieldPool : MonoBehaviour
{
    public GameObject shield;
    private Player playerScript;

    private float originalPos = -50;

    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
        Invoke("startShields", 0.3f);
    }

    void startShields()
    {
        for (int i = 0; i < playerScript.currentShield; i++)
        {
            spawnShield();
        }
    }


    public void RemoveShield()
    {
        int lastActiveChild = -1;

        if (playerScript.currentShield == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        } 
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    lastActiveChild = i;
                }
            }
            if (lastActiveChild != -1)
            {
                transform.GetChild(lastActiveChild).transform.DOShakePosition(0.5f, 5)
                .OnComplete(()=>transform.GetChild(lastActiveChild).gameObject.SetActive(false));
            }
        }
        playerScript.currentShield--;
    }

    public void AddShield()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }

    private void spawnShield()
    {
        var shieldPrefab = Instantiate(shield, new Vector3(0, 0, 0), Quaternion.identity);
        shieldPrefab.transform.SetParent(transform, false);
        shieldPrefab.transform.localScale = new Vector3(1, 1, 1);
        shieldPrefab.transform.localPosition = new Vector3(originalPos, 0, 0);

        originalPos += 18;
    }
}

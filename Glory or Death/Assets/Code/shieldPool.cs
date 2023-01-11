using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class shieldPool : MonoBehaviour
{
    public GameObject shield;
    private Player playerScript;

    private float originalPos = 18;

    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
        for (int i = 0; i < playerScript.currentShield; i++)
        {
            var shieldPrefab = Instantiate(shield, new Vector2(0, 0), Quaternion.identity);
            shieldPrefab.transform.SetParent(transform, false);
            shieldPrefab.transform.localScale = new Vector2(1, 1);
            shieldPrefab.transform.localPosition = new Vector2(originalPos, -17.5f);

            originalPos += 20;
        }
    }


}


/*
  public void AddShield()
    {
        var shieldPrefab = Instantiate(shield, new Vector2(0, 0), Quaternion.identity);
        shieldPrefab.transform.SetParent(transform, false);
        shieldPrefab.transform.localScale = new Vector2(1, 1);
        shieldPrefab.transform.localPosition = new Vector2(originalPos, -17.5f);
        originalPos += 20;
    }
    public void RemoveShield()
    {
        if(transform.childCount>0)
        {
            var lastShield = transform.GetChild(transform.childCount-1);
            originalPos -= 20;
            Destroy(lastShield);
        }
    }



    public int numOfShields;
    public void UpdateShields()
    {
        if(numOfShields < transform.childCount)
        {
            while(numOfShields < transform.childCount)
            {
                var lastShield = transform.GetChild(transform.childCount-1);
                originalPos -= 20;
                Destroy(lastShield);
            }
        }
        else if(numOfShields > transform.childCount)
        {
            for (int i = transform.childCount; i < numOfShields; i++)
            {
                var shieldPrefab = Instantiate(shield, new Vector2(0, 0), Quaternion.identity);
                shieldPrefab.transform.SetParent(transform, false);
                shieldPrefab.transform.localScale = new Vector2(1, 1);
                shieldPrefab.transform.localPosition = new Vector2(originalPos, -17.5f);
                originalPos += 20;
            }
        }
    }
 */

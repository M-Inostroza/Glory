using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class shieldPool : MonoBehaviour
{
    public GameObject shield;
    private Player playerScript;

    //private List<GameObject> shield_Pool = new List<GameObject>();
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

            originalPos += 22;
            //shield_Pool.Add(shieldPrefab);
        }
    }


    public void removeShield()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1));
            //shield_Pool.RemoveAt(shield_Pool.Count - 1);
        }
    }
}

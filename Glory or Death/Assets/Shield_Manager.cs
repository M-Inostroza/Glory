using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shield_Manager : MonoBehaviour
{
    public GameObject shield;
    public Player playerUnit;

    public float originalPos;

    bool defenseAtive = false;
    List<GameObject> shieldPrefabs = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        activateShields();
    }

    void activateShields()
    {
        if (!defenseAtive)
        {
            originalPos = -12.5f;
            for (int i = 0; i < playerUnit.currentShield; i++)
            {
                shieldPrefabs.Add(Instantiate(shield, new Vector2(originalPos, 2.8f), Quaternion.identity));
                shieldPrefabs[i].transform.SetParent(gameObject.transform);
                originalPos += .4f;
            }
            defenseAtive = true;
        }
    }

    public void destroyShield()
    {
        shieldPrefabs[playerUnit.currentShield].transform.DOShakePosition(.5f, 8, 20).OnComplete(() => shieldPrefabs[playerUnit.currentShield].SetActive(false));
    }
}

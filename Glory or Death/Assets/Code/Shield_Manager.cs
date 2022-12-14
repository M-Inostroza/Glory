using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shield_Manager : MonoBehaviour
{
    public GameObject shield;
    private Player playerScript;

    public float originalPos;

    bool defenseAtive = false;
    List<GameObject> shieldPrefabs = new List<GameObject>();

    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        activateShields();
    }

    void activateShields()
    {
        if (!defenseAtive)
        {
            originalPos = -13.5f;
            for (int i = 0; i < playerScript.currentShield; i++)
            {
                shieldPrefabs.Add(Instantiate(shield, new Vector2(originalPos, 3.3f), Quaternion.identity));
                shieldPrefabs[i].transform.SetParent(gameObject.transform);
                originalPos += .4f;
            }
            defenseAtive = true;
        }
    }

    public void destroyShield()
    {
        var currentShield = shieldPrefabs[playerScript.currentShield];
        currentShield.transform.DOShakePosition(.5f, 8, 20).OnComplete(() => currentShield.SetActive(false));
    }
}

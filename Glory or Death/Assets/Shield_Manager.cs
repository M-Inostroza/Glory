using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Manager : MonoBehaviour
{
    public GameObject shield;
    public Player playerUnit;

    int originalPos = -180;

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
            for (int i = 0; i < playerUnit.currentShield; i++)
            {
                shieldPrefabs.Add(Instantiate(shield, new Vector2(gameObject.transform.position.x + originalPos, gameObject.transform.position.y), Quaternion.identity));
                shieldPrefabs[i].transform.SetParent(gameObject.transform);
                originalPos += 45;
            }
            defenseAtive = true;
            originalPos = 200;
        }
    }

    public void destroyShield()
    {
        shieldPrefabs[playerUnit.currentShield].SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public timeManager timeManager;
    public BattleSystem battleSystem;

    private void Update()
    {
        if (timeManager.timeOut)
        {
            Debug.Log("Time out");
        }
    }
}

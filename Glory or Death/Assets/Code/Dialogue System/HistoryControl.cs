using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryControl : MonoBehaviour
{
    TimeManager TimeManager;


    Transform _panelBox;
    Transform _panelPlayer;
    Transform _panelEnemy;

    void Start()
    {
        TimeManager = FindObjectOfType<TimeManager>();

        TimeManager.StopTime();
    }

}

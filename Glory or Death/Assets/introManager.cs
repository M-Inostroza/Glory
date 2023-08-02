using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class introManager : MonoBehaviour
{
    [SerializeField] Transform panelPlayer;
    [SerializeField] Transform panelEnemy;

    public void closePanels()
    {
        panelPlayer.DOMoveX(308, 1);
        panelEnemy.DOMoveX(-308, 1);
    }
}

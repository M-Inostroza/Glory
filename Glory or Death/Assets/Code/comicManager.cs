using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class comicManager : MonoBehaviour
{
    [SerializeField] GameObject comicContainer;
    [SerializeField] Image darkOverlay;

    // Loading Screen
    [SerializeField] Transform enemyPanel;
    [SerializeField] Transform playerPanel;

    [SerializeField] GameObject loadingContainer;
    [SerializeField] GameObject thunder;

    public void playNext(int next)
    {
        GetComponent<Animator>().Play("stripe_" + next);
    }

    public void activateLoadingScreen()
    {
        loadingContainer.SetActive(true);
        enemyPanel.DOMoveX(308, 1);
        playerPanel.DOMoveX(-308, 1);
        thunder.GetComponent<Animator>().Play("thunder");
    }
}

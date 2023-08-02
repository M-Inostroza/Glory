using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class comicManager : MonoBehaviour
{
    private List<GameObject> comicEpisodes = new List<GameObject>();

    [SerializeField] GameObject comicContainer;
    [SerializeField] Image darkOverlay;

    void Start()
    {
        foreach (Transform child in comicContainer.transform)
        {
            comicEpisodes.Add(child.gameObject);
        }
    }

    public void playNext(int next)
    {
        GetComponent<Animator>().Play("stripe_" + next);
    }
}

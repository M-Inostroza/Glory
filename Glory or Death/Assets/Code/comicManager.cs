using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class comicManager : MonoBehaviour
{
    private List<GameObject> comicEpisodes = new List<GameObject>();
    private Animator comicAnimator;

    [SerializeField]
    private GameObject comicContainer;
    [SerializeField]
    private Image darkOverlay;

    void Start()
    {
        comicAnimator = GetComponent<Animator>();

        foreach (Transform child in comicContainer.transform)
        {
            comicEpisodes.Add(child.gameObject);
        }

        startComic();
    }

    void startComic()
    {
        darkOverlay.DOFade(0, 2);
        comicEpisodes[0].SetActive(true);
    }

    // Getter setters
    public List<GameObject> getComicEpisodes()
    {
        return comicEpisodes;
    }
}

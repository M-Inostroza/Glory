using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class comicManager : MonoBehaviour
{
    private List<GameObject> comicEpisodes = new List<GameObject>();

    [SerializeField]
    private GameObject comicContainer;
    [SerializeField]
    private Image darkOverlay;

    void Start()
    {
        foreach (Transform child in comicContainer.transform)
        {
            comicEpisodes.Add(child.gameObject);
        }

        StartCoroutine(animateEpisodes());
    }

    IEnumerator animateEpisodes()
    {
        comicEpisodes[0].transform.DOLocalMoveX(0, 20);
        comicEpisodes[0].transform.DOScale(new Vector3(0.9f, 0.9f, 1), 25).OnComplete(() => comicEpisodes[0].SetActive(false));
        darkOverlay.DOFade(0, 5);

        yield return new WaitForSeconds(24);

        comicEpisodes[1].SetActive(true);
        comicEpisodes[1].transform.DOScale(new Vector3(1, 1, 1), 10);

        yield return new WaitForSeconds(10);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

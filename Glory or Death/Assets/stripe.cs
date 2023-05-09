using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stripe : MonoBehaviour
{
    [SerializeField]
    private Animator comicAnimator;
    private comicManager comicManager;

    private void Start()
    {
        comicManager = FindObjectOfType<comicManager>();
    }

    public void deactive()
    {
        gameObject.SetActive(false);
    }

    public void playNext(string nextAnim)
    {
        comicManager.getComicEpisodes()[int.Parse(nextAnim)].SetActive(true);
        comicAnimator.Play(nextAnim);
    }
    
}

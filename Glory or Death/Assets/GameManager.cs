using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // UI Elements
    public GameObject startUI;
    public GameObject title;
    public GameObject button;
    public GameObject myName;
    public GameObject CS50;

    //Debug
    public float move;

    // Audio
    public AudioMAnager audioManager;
    // Start is called before the first frame update
    private void Start()
    {
        audioManager.Play("MainTheme");
        animateUI();
    }

    void animateUI()
    {
        title.transform.DOLocalMoveY(140, 1f).SetEase(Ease.InOutSine);
        CS50.transform.DOLocalMoveX(-280, 1f).SetEase(Ease.InOutSine).SetDelay(2f);
        myName.transform.DOLocalMoveX(340, 1f).SetEase(Ease.InOutSine).SetDelay(2f);
        button.transform.DOLocalMoveY(-100, 1f).SetEase(Ease.InOutSine).SetDelay(3f);
    }

    
    public void startFaceOff()
    {
        CS50.transform.DOLocalMoveX(-600, 1f).SetEase(Ease.InOutSine);
        myName.transform.DOLocalMoveX(600, 1f).SetEase(Ease.InOutSine).OnComplete(()=>audioManager.Stop("MainTheme"));
        button.transform.DOLocalMoveY(-450, 1f).SetEase(Ease.InOutSine).OnComplete(()=>audioManager.Play("Combat"));
        title.transform.DOLocalMoveY(500, 1.1f).SetEase(Ease.InOutSine).OnComplete(()=>startUI.SetActive(false));

        audioManager.Play("startGame");
    }

   

}

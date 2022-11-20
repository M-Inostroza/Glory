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
    public GameObject BG;
    public GameObject combatUI;

    public GameObject faceOff_1;
    public GameObject faceOff_2;
    public GameObject thunder;

    public GameObject img_1;
    public GameObject img_2;

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
        CS50.transform.DOLocalMoveX(-280, 1f).SetEase(Ease.InOutSine).SetDelay(1f);
        myName.transform.DOLocalMoveX(340, 1f).SetEase(Ease.InOutSine).SetDelay(1f);
        button.transform.DOLocalMoveY(-100, 1f).SetEase(Ease.InOutSine).SetDelay(1.5f);
    }

    
    public void startFaceOff()
    {
        audioManager.Play("startGame");

        CS50.transform.DOLocalMoveX(-600, 1f).SetEase(Ease.InOutSine);
        myName.transform.DOLocalMoveX(600, 1f).SetEase(Ease.InOutSine);
        button.transform.DOLocalMoveY(-500, 1f).SetEase(Ease.InOutSine);
        title.transform.DOLocalMoveY(500, 1.1f).SetEase(Ease.InOutSine);

        faceOff_1.transform.DOLocalMoveX(-305, 0.5f).SetDelay(1f).OnComplete(() => shakeTheFaceOff());
        faceOff_2.transform.DOLocalMoveX(305, 0.5f).SetDelay(1f).OnComplete(() => BG.SetActive(false));

        faceOff_1.transform.DOLocalMoveX(-900, 0.5f).SetDelay(4f).OnComplete(() => combatUI.SetActive(true));
        faceOff_2.transform.DOLocalMoveX(900, 0.5f).SetDelay(4f).OnComplete(()=>dissapearUI());
    }

    void dissapearUI()
    {
        startUI.SetActive(false);
        audioManager.Stop("MainTheme");
        audioManager.Play("Combat");
    }

    void shakeTheFaceOff()
    {
        img_1.transform.DOShakePosition(1f, 3f, 30);
        img_2.transform.DOShakePosition(1f, 3f, 30);
        thunder.SetActive(true);
        audioManager.Play("thunder");
    }

}

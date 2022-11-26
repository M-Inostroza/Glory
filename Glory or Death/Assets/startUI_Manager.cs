using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class startUI_Manager : MonoBehaviour
{
    // UI Elements
    public GameObject startUI;
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

    // Title
    public GameObject title_parent;
    public GameObject glory_img;
    public GameObject death_img;
    public GameObject or_img;

    public GameObject rayo_img;
    public ParticleSystem rayo_effect;

    

    // Audio
    public AudioMAnager audioManager;
    // Start is called before the first frame update
    private void Start()
    {
        audioManager.Play("MainTheme");
        animateUI();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void animateUI()
    {
        glory_img.transform.DOLocalMoveY(145, .7f).SetEase(Ease.InOutSine).SetDelay(1.2f).OnComplete(()=>audioManager.Play("clash_glory"));
        death_img.transform.DOLocalMoveY(67, .7f).SetEase(Ease.InOutSine).SetDelay(1.2f);

        or_img.transform.DOScale(0.3f, .3f).SetDelay(2.5f);
        rayo_img.transform.DOLocalMove(new Vector2(0, 105), .4f).SetEase(Ease.InOutSine).SetDelay(2f).OnComplete(() => thunderIntro());


        CS50.transform.DOLocalMoveX(-280, 1f).SetEase(Ease.InOutSine).SetDelay(2f);
        myName.transform.DOLocalMoveX(340, 1f).SetEase(Ease.InOutSine).SetDelay(2f);
        button.transform.DOLocalMoveY(-100, 1f).SetEase(Ease.InOutSine).SetDelay(2.5f);
    }

    
    public void startFaceOff()
    {
        audioManager.Play("startGame");

        CS50.transform.DOLocalMoveX(-600, 1f).SetEase(Ease.InOutSine);
        myName.transform.DOLocalMoveX(600, 1f).SetEase(Ease.InOutSine);
        button.transform.DOLocalMoveY(-500, 1f).SetEase(Ease.InOutSine);
        title_parent.transform.DOLocalMoveY(500, 1.1f).SetEase(Ease.InOutSine);

        faceOff_1.transform.DOLocalMoveX(-310, 0.5f).SetDelay(.8f).OnComplete(() => shakeTheFaceOff());
        faceOff_2.transform.DOLocalMoveX(310, 0.5f).SetDelay(.8f).OnComplete(() => BG.SetActive(false));

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

    void thunderIntro()
    {
        rayo_effect.Play();
        audioManager.Play("thunder_title");
    }

}

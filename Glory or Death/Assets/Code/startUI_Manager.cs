using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class startUI_Manager : MonoBehaviour
{
    // UI Elements
    private Transform play_button;
    private Transform howToPlay_button;
    
    [SerializeField]
    private GameObject background_image;

    [SerializeField]
    private Transform player_image, enemy_image, faceOff_1, faceOff_2;
    [SerializeField]
    private GameObject faceOff_thunder;

    public GameObject combat_UI;

    // Title
    [SerializeField]
    private GameObject title_container;

    private Transform glory_img;
    private Transform death_img;
    private Transform or_img;
    private Transform bolt_img;

    public ParticleSystem rayo_effect;

    // Audio
    public AudioManager audioManager;
    
    private void Start()
    {
        getUI_elements();
        animateUI();
        audioManager.Play("Main_Theme");
    }

    void animateUI()
    {
        glory_img.transform.DOLocalMoveY(145, .7f).SetEase(Ease.InOutSine).SetDelay(1.2f).OnComplete(()=>audioManager.Play("Clash_Intro"));
        death_img.transform.DOLocalMoveY(67, .7f).SetEase(Ease.InOutSine).SetDelay(1.2f);

        howToPlay_button.transform.DOLocalMoveY(-230, 1f).SetEase(Ease.InOutSine).SetDelay(2.5f);

        or_img.transform.DOScale(0.3f, .3f).SetDelay(2.5f);
        bolt_img.transform.DOLocalMove(new Vector2(0, 105), .4f).SetEase(Ease.InOutSine).SetDelay(2f).OnComplete(() => thunderIntro());

        play_button.transform.DOLocalMoveY(-100, 1f).SetEase(Ease.InOutSine).SetDelay(2.5f);
    }

    public void startFaceOff()
    {
        audioManager.Play("Game_Start");

        play_button.transform.DOLocalMoveY(-500, 1f).SetEase(Ease.InOutSine);
        title_container.transform.DOLocalMoveY(500, 1.1f).SetEase(Ease.InOutSine);
        howToPlay_button.gameObject.SetActive(false);

        faceOff_1.transform.DOLocalMoveX(-300, 0.5f).SetDelay(.8f).OnComplete(() => shakeTheFaceOff());
        faceOff_2.transform.DOLocalMoveX(300, 0.5f).SetDelay(.8f).OnComplete(() => background_image.SetActive(false));

        faceOff_1.transform.DOLocalMoveX(-900, 0.5f).SetDelay(4f).OnComplete(() => combat_UI.SetActive(true));
        faceOff_2.transform.DOLocalMoveX(900, 0.5f).SetDelay(4f).OnComplete(()=>dissapearUI());
    }

    void dissapearUI()
    {
        gameObject.SetActive(false);
        audioManager.Stop("Main_Theme");
        audioManager.Play("Combat");
    }

    void shakeTheFaceOff()
    {
        player_image.transform.DOShakePosition(1f, 3f, 30);
        enemy_image.transform.DOShakePosition(1f, 3f, 30);
        faceOff_thunder.SetActive(true);
        audioManager.Play("Thunder");
    }

    void thunderIntro()
    {
        rayo_effect.Play();
        audioManager.Play("Thunder_Title");
    }

    void getUI_elements()
    {
        //Get UI elements
        play_button = transform.Find("play_button");
        howToPlay_button = transform.Find("howToPlay_button");

        //Title
        glory_img = title_container.transform.Find("Glory");
        death_img = title_container.transform.Find("Death");
        or_img = title_container.transform.Find("Or");
        bolt_img = title_container.transform.Find("Bolt");
    }
}

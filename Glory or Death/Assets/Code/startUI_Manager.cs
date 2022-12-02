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

    // FaceOff Elements
    private Transform faceOff_1;
    private Transform player_image;

    private Transform faceOff_2;
    private Transform enemy_image;

    [SerializeField]
    private GameObject faceOff_thunder;

    public GameObject combatUI;

    // Title
    public GameObject title_parent;
    public GameObject glory_img;
    public GameObject death_img;
    public GameObject or_img;

    public GameObject rayo_img;
    public ParticleSystem rayo_effect;

    // Audio
    public AudioManager audioManager;
    // Start is called before the first frame update
    private void Start()
    {
        //Get UI elements
        play_button = transform.Find("play_button");
        howToPlay_button = transform.Find("howToPlay_button");

        faceOff_1 = transform.Find("faceOff_1");
        player_image = faceOff_1.transform.Find("player_img");

        faceOff_2 = transform.Find("faceOff_2");
        enemy_image = faceOff_2.transform.Find("enemy_img");

        audioManager.Play("MainTheme");
        animateUI();
    }

    void animateUI()
    {
        glory_img.transform.DOLocalMoveY(145, .7f).SetEase(Ease.InOutSine).SetDelay(1.2f).OnComplete(()=>audioManager.Play("clash_glory"));
        death_img.transform.DOLocalMoveY(67, .7f).SetEase(Ease.InOutSine).SetDelay(1.2f);

        howToPlay_button.transform.DOLocalMoveY(-230, 1f).SetEase(Ease.InOutSine).SetDelay(2.5f);

        or_img.transform.DOScale(0.3f, .3f).SetDelay(2.5f);
        rayo_img.transform.DOLocalMove(new Vector2(0, 105), .4f).SetEase(Ease.InOutSine).SetDelay(2f).OnComplete(() => thunderIntro());

        play_button.transform.DOLocalMoveY(-100, 1f).SetEase(Ease.InOutSine).SetDelay(2.5f);
    }

    
    public void startFaceOff()
    {
        audioManager.Play("startGame");

        play_button.transform.DOLocalMoveY(-500, 1f).SetEase(Ease.InOutSine);
        title_parent.transform.DOLocalMoveY(500, 1.1f).SetEase(Ease.InOutSine);
        howToPlay_button.gameObject.SetActive(false);

        faceOff_1.transform.DOLocalMoveX(-300, 0.5f).SetDelay(.8f).OnComplete(() => shakeTheFaceOff());
        faceOff_2.transform.DOLocalMoveX(300, 0.5f).SetDelay(.8f).OnComplete(() => background_image.SetActive(false));

        faceOff_1.transform.DOLocalMoveX(-900, 0.5f).SetDelay(4f).OnComplete(() => combatUI.SetActive(true));
        faceOff_2.transform.DOLocalMoveX(900, 0.5f).SetDelay(4f).OnComplete(()=>dissapearUI());
    }

    void dissapearUI()
    {
        gameObject.SetActive(false);
        audioManager.Stop("MainTheme");
        audioManager.Play("Combat");
    }

    void shakeTheFaceOff()
    {
        player_image.transform.DOShakePosition(1f, 3f, 30);
        enemy_image.transform.DOShakePosition(1f, 3f, 30);
        faceOff_thunder.SetActive(true);
        audioManager.Play("thunder");
    }

    void thunderIntro()
    {
        rayo_effect.Play();
        audioManager.Play("thunder_title");
    }

}

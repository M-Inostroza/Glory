using UnityEngine;
using DG.Tweening;

public class defendManager : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] GameObject _minigameCircle;
    [SerializeField] GameObject _aKeyCanvas;
    [SerializeField] Camera mainCamera;

    SpriteRenderer _auraEffect;

    AudioManager AudioManager;
    TutorialManager TutorialManager;
    Player Player;
    cameraManager CameraManager;
    
    // Control
    bool transformControl;
    bool canDefend = false;

    float _defaultScaleLimit = 0.8f;

    Tween scaleUP;
    private void Awake()
    {
        CameraManager = FindObjectOfType<cameraManager>();
        Player = FindObjectOfType<Player>();
        AudioManager = FindObjectOfType<AudioManager>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        _auraEffect = _minigameCircle.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        ControlDefend();
        ColorFeedback();
    }

    public void activateShieldMinigame()
    {
        if (gameManager.isTutorial())
        {
            TutorialManager.defendDetailTutorial(3);
        }
        canDefend = true;
        transformControl = true;
        cameraAndKeyIn();
        
        scaleUP = transform.DOScale(1, 1.5f).SetEase(Ease.InOutQuad).OnComplete(Fail);
        _minigameCircle.SetActive(true);
        AudioManager.Play("Shield_charge");
    }

    void ControlDefend()
    {
        if (!BattleSystem.IsPaused)
        {
            if (canDefend)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    executeShield();
                }
            }

            if (transform.localScale.x == 1 && transformControl)
            {
                transform.DOScale(0, 0);
                Fail();
            }
        }
    }

    void executeShield()
    {
        float _criticStart = .32f;
        float _criticEnd = .44f;

        if (transform.localScale.x < _defaultScaleLimit && transform.localScale.x > _criticEnd || transform.localScale.x < _criticStart)
        {   // Fail
            CameraManager.PlayBloom(2);
            AudioManager.Play("UI_select_fail");
            transform.DOShakePosition(0.2f, 0.05f, 40).OnComplete(Fail);
            scaleUP.Kill();
            transform.DOScale(0, 0);
            cameraZoomOut();
        }
        else if (transform.localScale.x > _defaultScaleLimit && transform.localScale.x < .95f)
        {   // Normal win
            if (!gameManager.isTutorial())
            {
                Player.incrementAdrenaline(Player.GetAdrenalineFactor());
            }
            AudioManager.Play("defend_success");
            scaleUP.Rewind();
            playerAnim.SetBool("skillShieldSuccess", true);
            closeMinigame();
        } 
        else if (transform.localScale.x > _criticStart && transform.localScale.x < _criticEnd) 
        {   // Critic win
            if (!gameManager.isTutorial())
            {
                Player.incrementAdrenaline(Player.GetAdrenalineFactor() + 2);
            }
            CameraManager.PlayBloom(1);
            playerAnim.SetBool("skillShieldSuccess", true);
            AudioManager.Play("defend_success");
            scaleUP.Rewind();
            transform.DOScale(0, 0);
            closeMinigame();
        }
    }

    void Fail()
    {
        scaleUP.Rewind();
        AudioManager.Play("defend_fail");
        playerAnim.SetBool("skillFail", true);
        closeMinigame();
    }

    void closeMinigame()
    {
        cameraZoomOut();
        _minigameCircle.SetActive(false);
        _aKeyCanvas.SetActive(false);
        transformControl = false;
        canDefend = false;

        if (gameManager.isTutorial() && !TutorialManager.hasShownDetail_defend && TutorialManager.GetNumberOfTries() != 0)
        {
            StartCoroutine(TutorialManager.toggleInput(1, 1, 2.5f));
        }
    }

    void ColorFeedback()
    {
        if (transform.localScale.x > 0.65f)
        {
            _auraEffect.DOFade(0.8f, 0.05f);
        } else
        {
            _auraEffect.DOFade(0,0);
        }
    }

    void cameraZoomIn()
    {
        mainCamera.DOFieldOfView(35, 0.2f);
        mainCamera.transform.DOLocalMoveY(-1.7f, 0.2f);
    }
    void cameraZoomOut()
    {
        mainCamera.DOFieldOfView(50, 0.2f);
        mainCamera.transform.DOLocalMoveY(0, 0.2f);
    }
    void cameraAndKeyIn()
    {
        cameraZoomIn();
        _aKeyCanvas.SetActive(true);
        if (gameManager.isTutorial())
        {
            TutorialManager.activateA();
        } else
        {
            FindObjectOfType<Combat_UI>().activateA();
        }
    }
}



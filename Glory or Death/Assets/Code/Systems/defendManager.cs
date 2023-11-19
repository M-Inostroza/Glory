using UnityEngine;
using DG.Tweening;

public class defendManager : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] GameObject shadow;
    [SerializeField] GameObject keyCanvas;
    [SerializeField] Camera mainCamera;

    SpriteRenderer shine;

    AudioManager audioManager;
    Tutorial_UI tutorial_UI;

    // Control
    bool transformControl;
    bool canDefend = false;

    Tween scaleUP;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
        shine = shadow.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        controlDefend();
        colorFeedback();
    }

    public void activateShieldMinigame()
    {
        tutorial_UI.defendDetailTutorial(3);
        canDefend = true;
        transformControl = true;
        cameraAndKeyIn();
        
        scaleUP = transform.DOScale(1, 1.5f).SetEase(Ease.InOutQuad).OnComplete(Fail);
        shadow.SetActive(true);
        audioManager.Play("Shield_charge");
    }

    void controlDefend()
    {
        if (!BattleSystem.IsPaused)
        {
            if (canDefend)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    executeShield(0.8f);
                }
            }

            if (transform.localScale.x == 1 && transformControl)
            {
                transform.DOScale(0, 0);
                Fail();
            }
        }
    }

    void executeShield(float scaleLimit)
    {
        if (transform.localScale.x < scaleLimit )
        {
            audioManager.Play("UI_select_fail"); 
            transform.DOShakePosition(0.2f, 0.05f, 40).OnComplete(Fail);
            scaleUP.Kill();
            transform.DOScale(0, 0);
        }
        else if (transform.localScale.x > scaleLimit && transform.localScale.x < 95)
        {
            audioManager.Play("defend_success");
            scaleUP.Rewind();
            playerAnim.SetBool("skillShieldSuccess", true);
            closeMinigame();
        }
    }

    void Fail()
    {
        scaleUP.Rewind();
        audioManager.Play("defend_fail");
        playerAnim.SetBool("skillFail", true);
        closeMinigame();
    }

    void closeMinigame()
    {
        cameraZoomOut();
        shadow.SetActive(false);
        keyCanvas.SetActive(false);
        transformControl = false;
        canDefend = false;
    }

    void colorFeedback()
    {
        if (transform.localScale.x > 0.65f)
        {
            shine.DOFade(0.8f, 0.05f);
        } else
        {
            shine.DOFade(0,0);
        }
    }

    void cameraZoomIn()
    {
        mainCamera.DOFieldOfView(35, 1);
        mainCamera.transform.DOLocalMoveY(-1.7f, 1);
    }
    void cameraZoomOut()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
    }
    void cameraAndKeyIn()
    {
        cameraZoomIn();
        keyCanvas.SetActive(true);
        if (gameManager.isTutorial())
        {
            FindObjectOfType<Tutorial_UI>().activateA();
        } else
        {
            FindObjectOfType<Combat_UI>().activateA();
        }
    }
}



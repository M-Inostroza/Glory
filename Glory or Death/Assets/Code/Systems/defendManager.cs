using UnityEngine;
using DG.Tweening;

public class defendManager : MonoBehaviour
{
    [SerializeField] Animator playerAnim;
    [SerializeField] GameObject shadow;
    [SerializeField] GameObject keyCanvas;
    [SerializeField] Camera mainCamera;

    // Control
    bool transformControl;
    bool canDefend = false;

    Tween scaleUP;
    AudioManager audioManager;
    Player Player;
    

    private void Awake()
    {
        Player = FindObjectOfType<Player>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    private void Update()
    {
        controlDefend();
    }

    public void activateShieldMinigame()
    {
        cameraZoomIn();
        keyCanvas.SetActive(true);
        FindObjectOfType<Combat_UI>().activateA();
        canDefend = true;
        scaleUP = transform.DOScale(1, 1.5f).SetEase(Ease.InOutQuad).OnComplete(() => Fail());
        shadow.SetActive(true);
        transformControl = true;
        audioManager.Play("Shield_charge");
    }

    void controlDefend()
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

    void executeShield(float scaleLimit)
    {
        if (transform.localScale.x < scaleLimit )
        {
            transform.DOShakePosition(0.4f, 0.05f, 40);
            Fail();
        }
        else if (transform.localScale.x > scaleLimit && transform.localScale.x < 95)
        {
            audioManager.Play("defend_success");
            scaleUP.Rewind();
            Player.increaseCurrentShield();
            playerAnim.SetBool("skillShieldSuccess", true);
        } 
        closeMinigame();
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
}



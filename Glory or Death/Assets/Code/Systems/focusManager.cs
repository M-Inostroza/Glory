using System.Collections;
using UnityEngine;
using DG.Tweening;
using AssetKits.ParticleImage;
using UnityEngine.UI;

public class focusManager : MonoBehaviour
{
    float cursorSpeed; // menos es m�s
    float targetSpeed;

    [SerializeField] ParticleImage _celebrationParticle;
    bool hasPlayedCelebration;

    [SerializeField] GameObject cursor;
    [SerializeField] GameObject target;

    [SerializeField] Camera mainCamera;

    // Timer bars
    Slider _timerBarLeft;
    Slider _timerBarRight;

    Player playerUnit;
    AudioManager audioManager;

    static int _totalATKBuff;
    
    // Limits
    private float minX = -7;
    private float maxX = 7f;
    bool canFocus = false;
    bool canMoveTarget;

    // Range within which the target sprite should be when the "a" key is pressed
    private float targetRangeMin;

    Combat_UI combat_UI;
    Tutorial_UI _tutorial_UI;

    private void Awake()
    {
        _tutorial_UI = FindObjectOfType<Tutorial_UI>();
        playerUnit = FindObjectOfType<Player>();
        combat_UI = FindObjectOfType<Combat_UI>();
        audioManager = FindObjectOfType<AudioManager>();

        if (!gameManager.isTutorial())
        {
            _timerBarLeft = transform.GetChild(5).GetComponent<Slider>();
            _timerBarRight = transform.GetChild(6).GetComponent<Slider>();
        }
    }

    private void Start()
    {
        _totalATKBuff = 0;
    }

    private void OnEnable()
    {
        hasPlayedCelebration = false;
        canFocus = true;
        canMoveTarget = true;
        
        cameraZoomIn();
        StartCoroutine(focusTimer(5));
        audioManager.Play("Focus_On");
        activateKey();
        
        // Target's range and position
        targetRangeMin = Random.Range(minX + 1.2f, maxX - 1.2f);
        target.transform.localPosition = new Vector2(targetRangeMin, -14.85f);

        setDificulty();
        SetCursor();
        if (!gameManager.isTutorial())
        {
            RunBarTimers();
        }
    }

    private void Update()
    {
        moveTarget();
        checkInputHit();
    }

    IEnumerator focusTimer(float time) // Mejorable!!
    {
        yield return new WaitForSeconds(time);
        failFocus();
    }

    // Executes when the user presses the S key and checks the hit
    void checkInputHit()
    {
        float boxOffset = 1.3f;

        if (canFocus && !BattleSystem.IsPaused)
        {
            if (Input.GetKey(KeyCode.S))
            {
                float targetXstart = target.transform.localPosition.x - boxOffset;
                float targetXend = target.transform.localPosition.x + boxOffset;

                StopBarTimers();

                if (cursor.transform.localPosition.x > targetXstart && cursor.transform.localPosition.x < targetXend)
                {
                    successFocus();
                }
                else
                {
                    failFocus();
                }
            }
        }
    }
    void successFocus()
    {
        if (!gameManager.isTutorial())
        {
            playVisualEffects();
        }
        audioManager.Play("Focus_Success");
        playerUnit.GetComponent<Animator>().SetBool("focusSuccess", true);
        cursor.transform.DOKill();
        canMoveTarget = false;
        StartCoroutine(timeManager.slowMotion(.4f, .4f, () =>
        {
            if (!gameManager.isTutorial())
            {
                _celebrationParticle.Clear();
                _celebrationParticle.gameObject.SetActive(false);
                playerUnit.incrementAdrenaline(playerUnit.GetAdrenalineFactor());
                _totalATKBuff++;
            }
            
            cameraZoomOut();
            canFocus = false;
            gameObject.SetActive(false);
        }));
    }
    void failFocus()
    {
        audioManager.Play("Focus_Fail");
        playerUnit.GetComponent<Animator>().SetBool("skillFail", true);
        cameraZoomOut();
        gameObject.SetActive(false);
        canFocus = false;
    }
    void moveTarget()
    {
        if (canMoveTarget)
        {
            target.transform.Translate(targetSpeed * Time.deltaTime, 0, 0);

            // Loop
            if (target.transform.localPosition.x > (maxX - 0.5f) || target.transform.localPosition.x < (minX + 0.5f))
            {
                targetSpeed *= -1;
            }
        }
    }
    private void setDificulty()
    {
        if (gameManager.isTutorial())
        {
            cursorSpeed = 2;
            targetSpeed = 1.6f;
            if (!_tutorial_UI.hasShownDetail_focus)
            {
                cursorSpeed = 2.4f;
                targetSpeed = 1;
                _tutorial_UI.focusDetailTutorial(3);
            }
        } else
        {
            cursorSpeed = 1.6f;
            targetSpeed = 1.8f;
        }
    }

    void activateKey()
    {
        if (gameManager.isTutorial())
        {
            _tutorial_UI.activateS();
        }
        else
        {
            combat_UI.activateS();
        }
    }

    void SetCursor()
    {
        // Cursor's initial position (right)
        cursor.transform.localPosition = new Vector2(maxX, cursor.transform.localPosition.y);
        // Moves cursor from left to right
        cursor.transform.DOLocalMoveX(minX, cursorSpeed).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    void RunBarTimers()
    {
        _timerBarLeft.value = 10;
        _timerBarRight.value = 10;
        _timerBarLeft.DOValue(0, 5);
        _timerBarRight.DOValue(0, 5);
    }

    void StopBarTimers()
    {
        _timerBarLeft.DOKill();
        _timerBarRight.DOKill();
    }

    void cameraZoomIn()
    {
        mainCamera.DOFieldOfView(30, 1);
        mainCamera.transform.DOLocalMoveY(-2.5f, 1);
    }
    void cameraZoomOut()
    {
        mainCamera.DOFieldOfView(50, 0.5f);
        mainCamera.transform.DOLocalMoveY(0, 0.5f);
    }

    void playVisualEffects()
    {
        mainCamera.GetComponent<cameraManager>().playChrome();
        mainCamera.GetComponent<cameraManager>().PlayBloom(1);
        if (!hasPlayedCelebration)
        {
            _celebrationParticle.transform.localPosition = new Vector2(target.transform.localPosition.x, _celebrationParticle.transform.localPosition.y);
            _celebrationParticle.gameObject.SetActive(true);
            _celebrationParticle.Play();
            hasPlayedCelebration = true;
        }
    }

    public static int GetTotalATKBuff()
    {
        return _totalATKBuff;
    }
    public static void ResetATKBuff()
    {
        _totalATKBuff = 0;
    }
}

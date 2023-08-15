using System.Collections;
using UnityEngine;
using DG.Tweening;

public class focusManager : MonoBehaviour
{
    float cursorSpeed = 1.4f; // menos es más
    float targetSpeed = 1.8f; 

    [SerializeField] GameObject cursor;
    [SerializeField] GameObject target;

    [SerializeField] Camera mainCamera;

    Player playerUnit;
    AudioManager audioManager;
    
    // Limits
    private float minX = -7.8f;
    private float maxX = 7.8f;
    bool canFocus = false;

    // Range within which the target sprite should be when the "a" key is pressed
    private float targetRangeMin;

    timeManager timeManager;
    Combat_UI combat_UI;
    BattleSystem BS;

    private void Awake()
    {
        BS = FindObjectOfType<BattleSystem>();
        playerUnit = FindObjectOfType<Player>();
        combat_UI = FindObjectOfType<Combat_UI>();
        timeManager = FindObjectOfType<timeManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        // Cursor's initial position (right)
        cursor.transform.localPosition = new Vector2(maxX, cursor.transform.localPosition.y);

        // Moves cursor from left to right
        cursor.transform.DOLocalMoveX(minX, cursorSpeed).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        cameraZoomIn();
        StartCoroutine(focusTimer(5));
        audioManager.Play("Focus_On");

        combat_UI.activateS();

        // Target's range and position
        targetRangeMin = Random.Range(minX + 1.2f, maxX - 1.2f);
        target.transform.localPosition = new Vector2(targetRangeMin, -14.85f);

        canFocus = true;
    }

    private void Update()
    {
        moveTarget();
        checkFocus();
    }

    IEnumerator focusTimer(float time) // Mejorable!!
    {
        yield return new WaitForSeconds(time);
        failFocus();
    }
    void checkFocus()
    {
        if (canFocus && !BattleSystem.IsPaused)
        {
            if (Input.GetKey(KeyCode.S))
            {
                float targetXstart = target.transform.localPosition.x - 1.3f;
                float targetXend = target.transform.localPosition.x + 1.3f;

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
        audioManager.Play("Focus_Success");
        playerUnit.GetComponent<Animator>().SetBool("focusSuccess", true);
        cameraZoomOut();
        gameObject.SetActive(false);
        canFocus = false;
    }
    void failFocus()
    {
        audioManager.Play("Focus_Fail");
        timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
        playerUnit.GetComponent<Animator>().SetBool("skillFail", true);
        cameraZoomOut();
        gameObject.SetActive(false);
        canFocus = false;
    }
    void moveTarget()
    {
        target.transform.Translate(targetSpeed * Time.deltaTime, 0, 0);

        // Loop
        if (target.transform.localPosition.x > (maxX - 0.5f) || target.transform.localPosition.x < (minX + 0.5f))
        {
            targetSpeed *= -1;
        }
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
}

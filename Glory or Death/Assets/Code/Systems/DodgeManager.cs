using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DodgeManager : MonoBehaviour
{
    private Player playerUnit;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Slider evadeSlider;
    [SerializeField] private GameObject evadeTarget, starFeedback;
    [SerializeField] private float Timer;

    bool hasStarJumped = false;
    bool isCritic = false;

    public GameObject[] arrowPrefabs;

    // Set evade state
    public GameObject dodgeBuffIcon;

    // Initial position of the arrows
    float intPos;

    Animator playerAnimator;
    AudioManager audioManager;

    // Instantiated arrows
    List<GameObject> instantArrows = new List<GameObject>();

    private void Start()
    {
        playerUnit = FindObjectOfType<Player>();
        playerAnimator = playerUnit.GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        hasStarJumped = false;
        isCritic = false;

        spawnArrows();
        doCameraSlow(35, 0.7f, 0.5f);
        openMinigame();
        
        StartCoroutine(mainTimer(Timer)); // Normal 1.8
    }

    private void Update()
    {
        runCommands();
    }

    // Main arrow mechanic
    void openMinigame()
    {
        Transform fillArea = evadeSlider.transform.GetChild(0);

        // Set target position
        float newRandom = Random.Range(0, 80);
        evadeTarget.transform.DOLocalMoveX(newRandom, 0.1f);

        // Animates bar
        evadeSlider.transform.DOLocalMoveY(-70, 0.3f);
        evadeSlider.transform.DOScale(1.3f, 0.3f);

        // Opacity fade ON
        foreach (Transform child in fillArea.transform)
        {
            Image childImage = child.GetComponent<Image>();
            if (childImage)
            {
                childImage.DOFade(1, 0.3f);
            }
        }
    }
    void spawnArrows()
    {
        intPos = -1.5f;
        for (int i = 0; i < 4; i++)
        {
            GameObject randomArrow = arrowPrefabs[Random.Range(0, 4)];
            instantArrows.Add(Instantiate(randomArrow, new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), randomArrow.transform.rotation));
            instantArrows[i].transform.SetParent(gameObject.transform);
            intPos += .8f;
        }
    }
    void runCommands()
    {
        if(instantArrows.Count > 0)
        {
            switch (instantArrows[0].name)
            {
                case "down(Clone)":
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                case "up(Clone)":
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        killArrow(instantArrows[0]);
                    } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                case "left(Clone)":
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                case "right(Clone)":
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                    
            }
        } else if (instantArrows.Count == 0)
        {
            if (evadeSlider.value >= 95 && hasStarJumped == false)
            {
                isCritic = true;
                starFeedback.GetComponent<Image>().DOFade(1, 0).OnComplete(() => doCritic());
            } else
            {
                StartCoroutine(mainTimer(0.4f));
            }
        }
    }

    void killArrow(GameObject arrow)
    {
        audioManager.Play("arrowEvadeWosh");
        evadeSlider.DOValue(evadeSlider.value + 50, 0.08f);
        instantArrows.RemoveAt(0);
        arrow.transform.DOLocalJump(new Vector2(arrow.transform.localPosition.x, arrow.transform.localPosition.y + 10), 6, 1, 0.3f).OnComplete(() => Destroy(arrow.gameObject));
    }
    void failArrow(GameObject arrow)
    {
        audioManager.Play("arrowFail");
        evadeSlider.DOValue(evadeSlider.value - 30, 0.08f);
        instantArrows.RemoveAt(0);
        arrow.transform.DOShakePosition(0.3f, 4, 20).OnComplete(() => Destroy(arrow.gameObject));
    }
    void checkSuccess()
    {
        if (evadeSlider.value < evadeTarget.transform.localPosition.x)
        {
            audioManager.Play("Audience_boo");
            playerUnit.missed = false;
            playerAnimator.SetBool("skillFail", true);
        }
        else if (evadeSlider.value >= evadeTarget.transform.localPosition.x)
        {
            audioManager.Play("Audience_cheer_mid");
            animateBuff();
            playerUnit.missed = true;
            playerAnimator.SetBool("evadeSuccess", true);
        }
    }
    void doCritic()
    {
        playerUnit.missed = true;
        audioManager.Play("Audience_cheer_high");
        starFeedback.transform.DOLocalJump(new Vector3(140, 28, 0), 18, 1, 0.6f).OnComplete(()=> starFeedback.transform.DOLocalMove(new Vector2(112,0), 0));
        starFeedback.transform.DOLocalRotate(new Vector3(0, 0, -160), 0.6f).OnComplete(()=> starFeedback.transform.localRotation = Quaternion.identity);
        starFeedback.GetComponent<Image>().DOFade(0, 0.6f).OnComplete(() => closeMinigame());
        hasStarJumped = true;
        playerAnimator.SetBool("evadeSuccess", true);
        animateBuff();
    }
    void closeMinigame()
    {
        if (!isCritic)
        {
            checkSuccess();
        }
        destroyArrows();
        Transform fillArea = evadeSlider.transform.GetChild(0);

        // Opacity fade ON
        foreach (Transform child in fillArea.transform)
        {
            Image childImage = child.GetComponent<Image>();
            if (childImage)
            {
                childImage.DOFade(0, 0.2f);
            }
        }

        returnCameraSlow(50, 0.5f, 1);
        
        evadeSlider.transform.DOLocalMoveY(-170, 0.4f);
        evadeSlider.transform.DOScale(1f, 0.4f);

        gameObject.SetActive(false);
        evadeSlider.value = -100;
    }
    void destroyArrows()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().DOFade(0, 0.4f).OnComplete(()=>clear(child));
        }
        void clear(Transform child)
        {
            instantArrows.Clear();
            Destroy(child.gameObject);
        }
    }

    void doCameraSlow(float intensity, float speed, float timeScale)
    {
        mainCamera.DOFieldOfView(intensity, speed);
        Time.timeScale = timeScale;
        //35, 0.7, 0.5
    }
    void returnCameraSlow(float intensity, float speed, float timeScale)
    {
        mainCamera.DOFieldOfView(intensity, speed);
        Time.timeScale = timeScale;
        // 50, 0.5f, 1
    }
    public void animateBuff()
    {
        dodgeBuffIcon.SetActive(true);
        dodgeBuffIcon.GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        dodgeBuffIcon.transform.DOLocalMoveY(230, 0.8f).SetEase(Ease.OutExpo);
    }

    IEnumerator mainTimer(float time)
    {
        yield return new WaitForSeconds(time);
        closeMinigame();
    }
}

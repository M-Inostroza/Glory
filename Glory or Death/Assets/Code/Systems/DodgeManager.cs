using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DodgeManager : MonoBehaviour
{
    private Player playerUnit;

    [SerializeField] private Slider evadeSlider;
    [SerializeField] private GameObject evadeTarget, starFeedback;

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
        // Slowmo
        Time.timeScale = 0.5f;
        Transform fillArea = evadeSlider.transform.GetChild(0);

        evadeSlider.gameObject.SetActive(true);
        
        foreach (Transform child in fillArea.transform)
        {
            Image childImage = child.GetComponent<Image>();
            if (childImage)
            {
                childImage.DOFade(1, 0.4f);
            }
        }

        // Set target position
        float newRandom = Random.Range(0, 80);
        evadeTarget.transform.DOLocalMoveX(newRandom, 0.2f);


        // Spawn arrows
        intPos = -1.5f;
        for (int i = 0; i < 4; i++)
        {
            GameObject randomArrow = arrowPrefabs[Random.Range(0, 4)];
            instantArrows.Add(Instantiate(randomArrow, new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), randomArrow.transform.rotation));
            instantArrows[i].transform.SetParent(gameObject.transform);
            instantArrows[i].transform.localScale = new Vector3(10, 10, 10);
            intPos += .8f;
        }

        // Timer to close
        StartCoroutine(evadeTimer(4.8f)); // Normal 1.8
    }

    private void Update()
    {
        pressCommands();
        checkCritic();
    }

    
    // Main arrow mechanic
    void pressCommands()
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
        }
    }

    // Improves agility, animates the arrow and removes it from the list
    void killArrow(GameObject arrow)
    {
        audioManager.Play("arrowEvadeWosh");
        evadeSlider.DOValue(evadeSlider.value + 50, 0.08f);
        instantArrows.RemoveAt(0);
        arrow.transform.DOLocalJump(new Vector2(arrow.transform.localPosition.x, arrow.transform.localPosition.y + 10), 6, 1, 0.3f).OnComplete(() => Destroy(arrow.gameObject));
    }

    // Reduces agility, animates and removes it from the list 
    void failArrow(GameObject arrow)
    {
        audioManager.Play("arrowFail");
        evadeSlider.DOValue(evadeSlider.value - 30, 0.4f);
        instantArrows.RemoveAt(0);
        arrow.transform.DOShakePosition(0.3f, 4, 20).OnComplete(() => Destroy(arrow.gameObject));
    }

    // Deactivates self
    IEnumerator evadeTimer(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    bool hasJumped = false;
    public void checkCritic()
    {
        if (evadeSlider.value >= 95 && hasJumped == false)
        {
            starFeedback.GetComponent<Image>().DOFade(1, 0).OnComplete(() => doCritic());
        }
        Debug.Log("DO case time out and arrows fail");
    }
    void doCritic()
    {
        starFeedback.transform.DOLocalJump(new Vector3(140, 28, 0), 18, 1, 0.6f).OnComplete(()=> starFeedback.transform.DOLocalMove(new Vector2(112,0), 0));
        starFeedback.transform.DOLocalRotate(new Vector3(0, 0, -160), 0.6f).OnComplete(()=> starFeedback.transform.localRotation = Quaternion.identity);
        starFeedback.GetComponent<Image>().DOFade(0, 0.6f).OnComplete(() => closeMinigame());
        hasJumped = true;
    }
    

    public void checkSuccess()
    {
        // Triggers miss mechanic and animation if 4 arrows were hit
        if (evadeSlider.value >= (evadeTarget.transform.localPosition.x))
        {
            animateBuff();
            playerUnit.missed = true;
            playerAnimator.SetBool("DG_Skill", true);
        }
        else
        {
            playerUnit.missed = false;
            playerAnimator.SetBool("DG_Skill_Fail", true);
        }
        evadeSlider.value = -100;
        foreach (Transform child in transform)
        {
            instantArrows.Clear();
            Destroy(child.gameObject);
        }
    }

    public void closeMinigame()
    {
        Time.timeScale = 1;
        checkSuccess();
        Transform fillArea = evadeSlider.transform.GetChild(0);
        foreach (Transform child in fillArea.transform)
        {
            Image childImage = child.GetComponent<Image>();
            if (childImage)
            {
                childImage.DOFade(0, 0.2f);
            }
        }
        evadeSlider.gameObject.SetActive(false);
    }

    public void animateBuff()
    {
        dodgeBuffIcon.SetActive(true);
        dodgeBuffIcon.transform.DOPunchScale(new Vector2(12f, 12f), 0.4f, 8, 1);
    }
}

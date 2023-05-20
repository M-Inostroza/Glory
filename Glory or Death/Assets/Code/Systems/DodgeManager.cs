using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DodgeManager : MonoBehaviour
{
    private Player playerUnit;

    [SerializeField] private Slider evadeSlider;
    [SerializeField] private GameObject evadeTarget;

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
        StartCoroutine(evadeTimer(2.8f)); // Normal 1.8
    }

    private void Update()
    {
        pressCommands();
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
        evadeSlider.DOValue(evadeSlider.value + 50, 0.2f);
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

    private void OnDisable()
    {
        Debug.Log("evade slider value: " + evadeSlider.value);
        Debug.Log("X: " + evadeTarget.transform.localPosition.x);

        Time.timeScale = 1;

        // Triggers miss mechanic and animation if 4 arrows were hit
        if (evadeSlider.value >= (evadeTarget.transform.localPosition.x))
        {
            animateBuff();
            playerUnit.missed = true;
            playerAnimator.SetBool("DG_Skill", true);
        } else
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

    public void animateBuff()
    {
        dodgeBuffIcon.SetActive(true);
        dodgeBuffIcon.transform.DOPunchScale(new Vector2(12f, 12f), 0.4f, 8, 1);
    }
}

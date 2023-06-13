using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class focusManager : MonoBehaviour
{
    // Speed at which the cursor sprite moves
    private float cursorDuration = 2f;
    private float targetSpeed = 0.6f;

    // Cursor sprite and target sprite
    public GameObject cursor;
    public GameObject target;


    [SerializeField]
    private GameObject playerUnit;

    private bool isHit;

    // Set limits of bar
    private float minX = -8.5f;
    private float maxX = 8.5f;

    // Range within which the target sprite should be when the "a" key is pressed
    private float targetRangeMin;
    private float targetRangeMax;

    timeManager timeManager;
    private void Start()
    {
        timeManager = FindObjectOfType<timeManager>();

        // Set the initial position of the cursor to the right side of the screen
        cursor.transform.localPosition = new Vector2(maxX, cursor.transform.localPosition.y);

        // Tween the cursor to the left side of the screen
        cursor.transform.DOLocalMoveX(minX, cursorDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        isHit = true;

        // Minigame timer
        StartCoroutine(focusTimer());

        // Set the range for the target sprite
        targetRangeMin = Random.Range(minX + 1, maxX - 1);
        targetRangeMax = targetRangeMin + 3.5f;

        // Set the position of the target sprite
        target.transform.localPosition = new Vector2(targetRangeMin, -14.85f);
    }

    private void Update()
    {
        // Move the cursor sprite from left to right
        //target.transform.Translate(targetSpeed * Time.deltaTime, 0, 0);

        // Check if the cursor sprite has reached the end of the bar
        if (target.transform.localPosition.x > (maxX - 0.5f) || target.transform.localPosition.x < (minX + 0.5f))
        {
            // Reverse the direction of the cursor sprite
            targetSpeed *= -1;
        }

        checkFocus();
    }

    IEnumerator focusTimer() // Mejorable!!
    {
        yield return new WaitForSeconds(8);
        FindObjectOfType<Player>().GetComponent<Animator>().SetBool("skillFail", true);
        gameObject.SetActive(false);
    }


    void checkFocus()
    {
        if (Input.GetKey(KeyCode.A) && isHit)
        {
            isHit = false;

            float cursorXstart = cursor.transform.localPosition.x;
            float cursorXend = cursorXstart + 0.8f;

            Debug.Log("Cursor start: " + cursorXstart);
            Debug.Log("Cursor end: " + cursorXend);

            Debug.Log("Target position: " + target.transform.localPosition.x);

            if (target.transform.localPosition.x < cursorXstart && target.transform.localPosition.x > cursorXend)
            {
                Debug.Log("Is a match!");
            } else
            {
                Debug.Log("Is not a match!");
            }
        }
    }

    void checkHit()
    {
        if (isHit)
        {
            timeManager.enemyActionIcon.sprite = timeManager.iconSprites[0];
            playerUnit.GetComponent<Animator>().SetBool("FC_Skill", true);
            gameObject.SetActive(false);
            playerUnit.GetComponent<Player>().StartCoroutine(playerUnit.GetComponent<Player>().boostSpeed());
        }
        else
        {
            timeManager.enemyActionIcon.sprite = timeManager.iconSprites[1];
            playerUnit.GetComponent<Animator>().SetBool("DG_Skill_Fail", true);
            gameObject.SetActive(false);
        }
    }
}

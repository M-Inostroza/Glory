using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class focusManager : MonoBehaviour
{
    // Speed at which the cursor sprite moves
    private float cursorDuration = 0.3f;
    private float targetSpeed = 1f;

    // Cursor sprite and target sprite
    public GameObject cursor;
    public GameObject target;

    // Set limits of bar
    private float minX = -8.5f;
    private float maxX = 8.5f;

    // Range within which the target sprite should be when the "a" key is pressed
    private float targetRangeMin;
    private float targetRangeMax;

    private void Start()
    {
        // Create the tween and set the initial position of the cursor to the right side of the screen
        cursor.transform.localPosition = new Vector2(maxX, cursor.transform.localPosition.y);

        // Use the DOMoveX method to move the cursor to the left side of the screen
        cursor.transform.DOLocalMoveX(minX, cursorDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        // Focus timer
        StartCoroutine(focusTimer());

        // Set the range for the target sprite
        targetRangeMin = Random.Range(minX + 1, maxX - 1);
        targetRangeMax = targetRangeMin + 3.5f;

        // Set the position of the target sprite
        target.transform.localPosition = new Vector2(targetRangeMin, 0);
    }

    private void Update()
    {
        // Move the cursor sprite from left to right
        target.transform.Translate(targetSpeed * Time.deltaTime, 0, 0);

        // Check if the cursor sprite has reached the end of the bar
        if (target.transform.localPosition.x > (maxX - 0.5f) || target.transform.localPosition.x < (minX + 0.5f))
        {
            // Reverse the direction of the cursor sprite
            targetSpeed *= -1;
        }
    }

    IEnumerator focusTimer() // Mejorable!!
    {
        yield return new WaitForSeconds(1.2f);
        FindObjectOfType<timeManager>().enemyActionIcon.sprite = FindObjectOfType<timeManager>().iconSprites[1];
        FindObjectOfType<Player>().GetComponent<Animator>().SetBool("DG_Skill_Fail", true);
        gameObject.SetActive(false);
    }
}

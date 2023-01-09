using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class focusManager : MonoBehaviour
{
    // Speed at which the cursor sprite moves
    public float cursorSpeed = 1.0f;

    // Cursor sprite and target sprite
    public GameObject cursor;
    public GameObject target;

    // Set limits of bar
    private float minX = -9f;
    private float maxX = 9f;

    // Range within which the target sprite should be when the "a" key is pressed
    private float targetRangeMin;
    private float targetRangeMax;

    private void OnEnable()
    {
        // Set the range for the target sprite
        targetRangeMin = Random.Range(minX, maxX);
        targetRangeMax = targetRangeMin + 3.5f;

        // Set the position of the target sprite
        target.transform.localPosition = new Vector2(targetRangeMin, 0);
    }

    private void FixedUpdate()
    {
        // Move the cursor sprite from left to right
        cursor.transform.Translate(cursorSpeed * Time.deltaTime, 0, 0);

        // Check if the cursor sprite has reached the end of the bar
        if (cursor.transform.localPosition.x > maxX || cursor.transform.localPosition.x < minX)
        {
            // Reverse the direction of the cursor sprite
            cursorSpeed *= -1;
        }

        // Check if the "a" key is pressed and if the cursor sprite is within the target range
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (cursor.transform.localPosition.x < targetRangeMax && cursor.transform.localPosition.x > targetRangeMin)
            {
                Debug.Log("hit");
            }
        }
            
    }
}

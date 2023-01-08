using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class focusManager : MonoBehaviour
{
    // Speed at which the sprite moves
    public float speedCursor = 1.0f;
    public float speedTarget = 0.5f;

    //Set limits of bar
    private float maxLeft = -9f;
    private float maxRight = 9f;

    // Flag to indicate if the player can press the "a" key
    public bool canPressA = false;

    //Cursor sprite
    public GameObject cursor;
    public GameObject target;

    private void FixedUpdate()
    {
        // Move the sprite from left to right
        cursor.transform.Translate(speedCursor * Time.deltaTime, 0, 0);
        target.transform.Translate(speedTarget * Time.deltaTime, 0, 0);

        // Check if the sprite has reached the end of the parent sprite
        if (cursor.transform.localPosition.x > maxRight || cursor.transform.localPosition.x < maxLeft)
        {
            // Reverse the direction of the sprite
            speedCursor *= -1;
        }

        if (target.transform.localPosition.x > 8 || target.transform.localPosition.x < -8)
        {
            // Reverse the direction of the sprite
            speedTarget *= -1;
        }
    }
}

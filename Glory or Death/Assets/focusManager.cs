using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class focusManager : MonoBehaviour
{
    // Speed at which the sprite moves
    public float speed = 1.0f;

    //Set limits of bar
    private float maxLeft = -0.6f;
    private float maxRight = 0.6f;

    // Flag to indicate if the player can press the "a" key
    private bool canPressA = false;

    //Cursor sprite
    public GameObject cursor;

    void Update()
    {
        // Move the sprite from left to right
        cursor.transform.Translate(speed * Time.deltaTime, 0, 0);

        // Check if the sprite has reached the end of the parent sprite
        if (cursor.transform.localPosition.x >= maxRight || cursor.transform.localPosition.x <= maxLeft)
        {
            // Reverse the direction of the sprite
            speed *= -1;
        }

        // Check if the sprite has reached the middle of the horizontal bar sprite
        if (cursor.transform.localPosition.x >= -0.1f && cursor.transform.localPosition.x <= 0.1f && !canPressA)
        {
            // Set the flag to true so the player can press the "a" key
            canPressA = true;
        }

        // Check if the player has pressed the "a" key
        if (Input.GetKeyDown(KeyCode.A) && canPressA)
        {
            // Display a message or perform some other action to indicate that the player has successfully completed the minigame
            Debug.Log("Success!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class focusTarget : MonoBehaviour
{
    public GameObject cursor; // The other game object that you want to check for collisions with
    private focusManager focusManager;

    private void Start()
    {
        focusManager = transform.parent.GetComponent<focusManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is the one belonging to the other game object
        if (other.gameObject == cursor)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Success");
            }
            
        }
    }
}

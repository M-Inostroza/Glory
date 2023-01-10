using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class focusTarget : MonoBehaviour
{
    public GameObject cursor; // The other game object that you want to check for collisions with
    private bool canHit;

    public focusManager focusManager;
    public timeManager timeManager;
    private Player playerUnit;

    private void Start()
    {
        playerUnit = FindObjectOfType<Player>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the other collider is the one belonging to the cursor object
        if (other.gameObject == cursor)
        {
            canHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the other collider is the one belonging to the cursor object
        if (other.gameObject == cursor)
        {
            canHit = false;
        }
    }

    private void Update()
    {
        checkFocus();
    }

    void checkFocus()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (canHit)
            {
                Debug.Log("hit");
                playerUnit.GetComponent<Animator>().SetBool("FC_Skill", true);
                focusManager.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("nope");
                playerUnit.GetComponent<Animator>().SetBool("FC_Skill_Fail", true);
                focusManager.gameObject.SetActive(false);
            }
        }
    }
}

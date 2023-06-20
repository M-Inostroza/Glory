using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterSword : MonoBehaviour
{
    [SerializeField]
    GameObject counterManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shield Image")
        {
            Debug.Log("Defending!");
            counterManager.SetActive(false);
        }
        else if (collision.name == "Counter Target")
        {
            Debug.Log("Hit!");
            counterManager.SetActive(false);
        }
    }
}

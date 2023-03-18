using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInputManager : MonoBehaviour
{
    Player playerUnit;
    public GameObject button_ATK_1;
    public GameObject button_ATK_2;

    public GameObject button_REST;

    private void Start()
    {
        playerUnit = FindObjectOfType<Player>();
    }
}

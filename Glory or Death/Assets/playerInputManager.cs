using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInputManager : MonoBehaviour
{
    Player playerUnit;
    public GameObject button_ATK_1;
    public GameObject button_ATK_2;

    private void Start()
    {
        playerUnit = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        manageButtons();
    }

    void manageButtons()
    {
        if (playerUnit.adrenaline == 20)
        {
            button_ATK_1.SetActive(false);
            button_ATK_2.SetActive(true);
        } else
        {
            button_ATK_1.SetActive(true);
            button_ATK_2.SetActive(false);
        }
    }
}

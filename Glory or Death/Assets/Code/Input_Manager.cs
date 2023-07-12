using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject ATK, DF, DG, FC, RST;

    //Selected actions
    private string selectedPlayerAction;
    private string selectedEnemyAction;

    // Custom classes
    private timeManager timeManager;
    private AudioManager audioManager;

    // Avoid click spam
    private bool canClick = true;

    // Cooldown images (radial fill)
    private Image AttackButtonCD;
    private Image DefendButtonCD;
    private Image DodgeButtonCD;
    private Image FocusButtonCD;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        timeManager = FindObjectOfType<timeManager>();

        AttackButtonCD = GameObject.FindWithTag("AttackCD").GetComponent<Image>();
        DefendButtonCD = GameObject.FindWithTag("DefendCD").GetComponent<Image>();
        DodgeButtonCD = GameObject.FindWithTag("DodgeCD").GetComponent<Image>();
        FocusButtonCD = GameObject.FindWithTag("FocusCD").GetComponent<Image>();

        selectedPlayerAction = "none";
    }

    private void Update()
    {
        timeManager.ReduceCooldown(DefendButtonCD);
        timeManager.ReduceCooldown(AttackButtonCD);
        timeManager.ReduceCooldown(DodgeButtonCD);
        timeManager.ReduceCooldown(FocusButtonCD);
    }

    public void OnAttackButton()
    {
        if (AttackButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "ATK1";
            timeManager.selectIcon("ATK1");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnDefendButton()
    {
        if (DefendButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "DF";
            timeManager.selectIcon("DF");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnDodgeButton()
    {
        if (DodgeButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "DG";
            timeManager.selectIcon("DG");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnFocusButton()
    {
        if (FocusButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "FC";
            timeManager.selectIcon("FC");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    public void OnRestButton()
    {
        if (canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "RST";
            timeManager.selectIcon("RST");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }


    // Getters and Setters 
    public void SetCanClick(bool newValue)
    {
        canClick = newValue;
    }

    public void SetPlayerAction(string newAction)
    {
        selectedPlayerAction = newAction;
    }
    public string GetPlayerAction()
    {
        return selectedPlayerAction;
    }

    public void SetEnemyAction(string newAction)
    {
        selectedEnemyAction = newAction;
    }
    public string GetEnemyAction()
    {
        return selectedEnemyAction;
    }

    public GameObject GetRestButton()
    {
        return RST;
    }

    public Image GetAttackCD()
    {
        return AttackButtonCD;
    }
    public Image GetDefendCD()
    {
        return DefendButtonCD;
    }
    public Image GetDodgeCD()
    {
        return DodgeButtonCD;
    }
    public Image GetFocusCD()
    {
        return FocusButtonCD;
    }
}

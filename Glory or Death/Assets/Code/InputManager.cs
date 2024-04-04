using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ATK, DF, DG, FC, RST, SATK;

    [SerializeField] 

    //Selected actions
    private string selectedPlayerAction;
    private string selectedEnemyAction;

    
    TimeManager TimeManager;
    AudioManager audioManager;

    // Avoid click spam
    private static bool canClick = true;

    // Cooldown images (radial fill)
    private Image AttackButtonCD;
    private Image DefendButtonCD;
    private Image DodgeButtonCD;
    private Image FocusButtonCD;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        TimeManager = FindObjectOfType<TimeManager>();

        GetCooldownImages();

        selectedPlayerAction = "none";
    }

    private void Update()
    {
        UpdateCooldown();
    }

    public void OnAttackButton()
    {
        if (AttackButtonCD.fillAmount == 0 && canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "ATK1";
            TimeManager.selectIcon("ATK1");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }
    public void OnSuperAttackButton()
    {
        if (canClick)
        {
            audioManager.Play("UI_select");
            canClick = false;
            selectedPlayerAction = "ATK2";
            TimeManager.selectIcon("ATK2");
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
            TimeManager.selectIcon("DF");
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
            TimeManager.selectIcon("DG");
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
            TimeManager.selectIcon("FC");
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
            TimeManager.selectIcon("RST");
        }
        else
        {
            audioManager.Play("UI_select_fail");
        }
    }

    // Getters and Setters 
    public static void SetCanClick(bool newValue)
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
    public GameObject GetSATKButton()
    {
        return SATK;
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

    void GetCooldownImages()
    {
        AttackButtonCD = GameObject.FindWithTag("AttackCD").GetComponent<Image>();
        DefendButtonCD = GameObject.FindWithTag("DefendCD").GetComponent<Image>();
        DodgeButtonCD = GameObject.FindWithTag("DodgeCD").GetComponent<Image>();
        FocusButtonCD = GameObject.FindWithTag("FocusCD").GetComponent<Image>();
    }
    void UpdateCooldown()
    {
        TimeManager.ReduceCooldown(DefendButtonCD);
        TimeManager.ReduceCooldown(AttackButtonCD);
        TimeManager.ReduceCooldown(DodgeButtonCD);
        TimeManager.ReduceCooldown(FocusButtonCD);
    }

    public void resetCooldown()
    {
        AttackButtonCD.fillAmount = 0;
        DefendButtonCD.fillAmount = 0;
        DodgeButtonCD.fillAmount = 0;
        FocusButtonCD.fillAmount = 0;
    }
}

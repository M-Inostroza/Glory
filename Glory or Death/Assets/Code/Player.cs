using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //Name of the player *TODO* -> get name of player
    public string playerName;

    //Target manager
    public GameObject target_manager;

    //Start damage
    public int native_damage;

    //HP
    public int maxHP;
    public int currentHP;

    //Shield
    public int maxShield;
    public int currentShield;

    //Stamina
    public int maxStamina;
    public int currentStamina;

    //Agility (Dodging)
    public int maxAgility;
    public int currentAgility;
    public bool missed = false;

    public float evade;

    public bool TakeDamage(int dmg)
    { 
        int hit = Random.Range(1, 11);
        //Check Agility
        if (hit > currentAgility && !missed)
        {
            missed = false;
            transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);
            currentShield--;
            
            if (currentShield > 0)
                dmg -= 2;
            currentHP -= dmg;

            if (currentShield <= 0)
                currentShield = 0;
        } else
        {
            missed = true;
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void activateTargets()
    {
        target_manager.SetActive(true);
    }
}

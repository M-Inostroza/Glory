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
    public Shield_Manager shield_manager;

    //Stamina
    public int maxStamina;
    public int currentStamina;

    //Agility (Dodging)
    public int maxAgility;
    public int currentAgility;
    public bool missed = false;

    public float evade;

    AudioMAnager audioManager;


    // Current Enemy
    public GameObject enemy_unit;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioMAnager>();
    }

    public bool TakeDamage(int dmg)
    { 
        //Check Agility
        if (!missed)
        {
            missed = false;
            currentShield--;
            
            
            if (currentShield >= 0)
            {
                dmg -= 2;
                currentHP -= dmg;
                shield_manager.destroyShield();
            } else if (currentShield < 0)
            {
                currentShield = 0;
                currentHP -= dmg;
            }   
        } 
        else
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

    public void stopAttack()
    {
        gameObject.GetComponent<Animator>().SetBool("ATK1", false);
    }

    public void stopEvade()
    {
        gameObject.GetComponent<Animator>().SetBool("Evade", false);
    }

    public void stopDefend()
    {
        gameObject.GetComponent<Animator>().SetBool("DF", false);
    }

    public void startEnemyDefense()
    {
        enemy_unit.GetComponent<Animator>().SetBool("Hurt", true);
    }

    public void shakeHit()
    {
        transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);
    }

    public void playJump()
    {
        audioManager.Play("jumpWosh");
    }

    public void playStab()
    {
        audioManager.Play("stab");
    }

    public void playEvadeAttack()
    {
        audioManager.Play("evadeAttack");
    }
}

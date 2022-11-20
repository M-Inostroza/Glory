using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    //Name of the player *TODO* -> get name of player
    public string playerName;

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

    public bool hasHit = false;

    // Effects
    public ParticleSystem sandEffect;
    public ParticleSystem hitEffect;
    public ParticleSystem bloodEffect;

    AudioMAnager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioMAnager>();
    }


    public bool TakeDamage(int dmg)
    {
        transform.DOShakePosition(0.3f, 0.1f, 18, 10, false, true);

        if (currentShield > 0)
            dmg -= 2;
        currentHP -= dmg;
        currentShield--;

        if (currentShield <= 0)
            currentShield = 0;
        
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

    public void StopAttack()
    {
        GetComponent<Animator>().SetBool("ATK1", false);
    }

    public void stopEnemyDefense()
    {
        GetComponent<Animator>().SetBool("Hurt", false);
    }


    public void playJump()
    {
        audioManager.Play("jumpWosh");
        sandEffect.Play();
    }

    public void playAction()
    {
        if (FindObjectOfType<Player>().missed)
        {
            audioManager.Play("evadeWosh");
            
        }
        else
        {
            audioManager.Play("stab");
            hitEffect.Play();
            bloodEffect.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // -----Attack----- //
    // Targets
    public void targetSounds()
    {
        int random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                audioManager.Play("ATK_target_hit_1");
                break;
            case 2:
                audioManager.Play("ATK_target_hit_2");
                break;
            case 3:
                audioManager.Play("ATK_target_hit_3");
                break;
            case 4:
                audioManager.Play("ATK_target_hit_4");
                break;
            case 5:
                audioManager.Play("ATK_target_hit_5");
                break;
        }
    }
    public void stabSounds()
    {
        int random = Random.Range(1, 5);
        switch (random)
        {
            case 1:
                audioManager.Play("ATK_stab_1");
                break;
            case 2:
                audioManager.Play("ATK_stab_2");
                break;
            case 3:
                audioManager.Play("ATK_stab_3");
                break;
            case 4:
                audioManager.Play("ATK_stab_4");
                break;
            case 5:
                audioManager.Play("ATK_stab_5");
                break;
        }
    }

    // ------Jump----- //

    public void jumpSounds()
    {
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                audioManager.Play("DG_jump_1");
                break;
            case 2:
                audioManager.Play("DG_jump_2");
                break;
            case 3:
                audioManager.Play("DG_jump_3");
                break;
            case 4:
                audioManager.Play("DG_jump_4");
                break;
        } 
    }
    public void landSounds()
    {
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                audioManager.Play("DG_land_1");
                break;
            case 2:
                audioManager.Play("DG_land_2");
                break;
            case 3:
                audioManager.Play("DG_land_3");
                break;
            case 4:
                audioManager.Play("DG_land_4");
                break;
        }
    }
    public void dropSword()
    {
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                audioManager.Play("DG_drop_1");
                break;
            case 2:
                audioManager.Play("DG_drop_2");
                break;
        }
    }

    // -----Defend----- //

    //Defend(Success)
    public void sword_shield_hit()
    {
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                audioManager.Play("DF_sword_shield_1");
                break;
            case 2:
                audioManager.Play("DF_sword_shield_2");
                break;
            case 3:
                audioManager.Play("DF_sword_shield_3");
                break;
        }
    }

    public void blunt_hit()
    {
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                audioManager.Play("ATK_blunt_1");
                break;
            case 2:
                audioManager.Play("ATK_blunt_2");
                break;
            case 3:
                audioManager.Play("ATK_blunt_3");
                break;
            case 4:
                audioManager.Play("ATK_blunt_4");
                break;
        }
    }
}

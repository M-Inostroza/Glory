using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Manager_Player : MonoBehaviour
{
    public ParticleSystem speed_buff;


    // Dodge animation
    public void SpeedBuff()
    {
        speed_buff.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class counterManager : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(transform.localScale);

        transform.DOScale(0, 10f);

        if (Input.GetKeyDown(KeyCode.Space) && (transform.localScale.x <= 1f || transform.localScale.x >= 0.5f))
        {
            Debug.Log("Too soon");
            
        } 
    }
}



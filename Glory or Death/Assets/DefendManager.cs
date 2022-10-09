using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendManager : MonoBehaviour
{
    public GameObject[] arrows;
    float intPos = -70;
    public int[] rotationValues;
    
    private void OnEnable()
    {
        Vector3 rotate = new Vector3(0, 0, 0);
        for (int i = 0; i < arrows.Length; i++)
        {
            GameObject arrow = Instantiate(arrows[Random.Range(0, 4)], new Vector2(0, 0), Quaternion.Euler(new Vector3(0,0, rotationValues[Random.Range(0,4)])));
            arrow.transform.SetParent(gameObject.transform);
            arrow.transform.localScale = new Vector3(1, 1, 1);
            arrow.transform.SetPositionAndRotation(new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), Quaternion.identity);
            intPos += 45f;
        }  
    }
}

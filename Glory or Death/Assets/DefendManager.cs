using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendManager : MonoBehaviour
{
    public GameObject[] arrowPrefabs;
    float intPos = -70;

    public GameObject randomArrow;
    public GameObject arrow;
    private void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
            randomArrow = arrowPrefabs[Random.Range(0, 4)];
            arrow = Instantiate(randomArrow, new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), randomArrow.transform.rotation);
            arrow.transform.SetParent(gameObject.transform);
            arrow.transform.localScale = new Vector3(1, 1, 1);
            intPos += 45f;
        }  
    }

    private void OnDisable()
    {
        intPos = -70;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

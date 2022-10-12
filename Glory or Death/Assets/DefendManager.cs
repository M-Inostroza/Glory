using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefendManager : MonoBehaviour
{
    public GameObject[] arrowPrefabs;
    float intPos;

    List<GameObject> instantArrows = new List<GameObject>();

    private void OnEnable()
    {
        intPos = -70;
        for (int i = 0; i < 4; i++)
        {
            GameObject randomArrow = arrowPrefabs[Random.Range(0, 4)];
            instantArrows.Add(Instantiate(randomArrow, new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), randomArrow.transform.rotation));
            instantArrows[i].transform.SetParent(gameObject.transform);
            instantArrows[i].transform.localScale = new Vector3(1, 1, 1);
            intPos += 45f;
        }
    }

    private void Update()
    {
        pressCommands();
    }

    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            instantArrows.Clear();
            Destroy(child.gameObject);
        }
    }

    void pressCommands()
    {
        if(instantArrows.Count > 0)
        {
            switch (instantArrows[0].name)
            {
                case "down(Clone)":
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    break;
                case "up(Clone)":
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    break;
                case "left(Clone)":
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    break;
                case "right(Clone)":
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    break;
            }
        } else
        {
            gameObject.SetActive(false);
        }
        
    }


    void killArrow(GameObject arrow)
    {
        Destroy(arrow.gameObject);
        instantArrows.RemoveAt(0);
    }
}

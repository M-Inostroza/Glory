using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefendManager : MonoBehaviour
{
    public Player playerUnit;
    public GameObject[] arrowPrefabs;
    float intPos;
    float timer;

    int extraAgility;

    List<GameObject> instantArrows = new List<GameObject>();
   
    private void OnEnable()
    {
        timer = 1.5f;

        // Resets player's agility after buff
        playerUnit.currentAgility -= extraAgility;
        extraAgility = 0;
        
        intPos = -70;
        for (int i = 0; i < 4; i++)
        {
            GameObject randomArrow = arrowPrefabs[Random.Range(0, 4)];
            instantArrows.Add(Instantiate(randomArrow, new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), randomArrow.transform.rotation));
            instantArrows[i].transform.SetParent(gameObject.transform);
            instantArrows[i].transform.localScale = new Vector3(1, 1, 1);
            intPos += 45f;
        }

        // Sets timer
        StartCoroutine(commandTimer());
    }

    private void Update()
    {
        pressCommands();
    }

    private void OnDisable()
    {
        playerUnit.currentAgility += extraAgility;
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
                    } else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                case "up(Clone)":
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        killArrow(instantArrows[0]);
                    } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                case "left(Clone)":
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
                case "right(Clone)":
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        killArrow(instantArrows[0]);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        failArrow(instantArrows[0]);
                    }
                    break;
            }
        } else
        {
            StartCoroutine(animwait());
        }
        
    }


    void killArrow(GameObject arrow)
    {
        extraAgility++;
        instantArrows.RemoveAt(0);
        arrow.transform.DOLocalJump(new Vector2(arrow.transform.localPosition.x, arrow.transform.localPosition.y + 10), 6, 1, 0.3f).OnComplete(() => Destroy(arrow.gameObject));
    }

    void failArrow(GameObject arrow)
    {
        extraAgility--;
        instantArrows.RemoveAt(0);
        arrow.transform.DOShakePosition(0.3f, 4, 20).OnComplete(() => Destroy(arrow.gameObject));
    }


    IEnumerator commandTimer()
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    IEnumerator animwait()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}

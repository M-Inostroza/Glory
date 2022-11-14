using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EvadeManager : MonoBehaviour
{
    public Player playerUnit;
    public GameObject[] arrowPrefabs;

    // Initial position of the arrows
    float intPos;

    Animator playerAnimator;

    // The agility given by the arrows
    int extraAgility;

    // Instantiated arrows
    List<GameObject> instantArrows = new List<GameObject>();

    private void Start()
    {
        playerAnimator = playerUnit.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Resets player's agility after buff
        playerUnit.currentAgility -= extraAgility;
        extraAgility = 0;
        
        // Spawn arrows
        intPos = -70;
        for (int i = 0; i < 4; i++)
        {
            GameObject randomArrow = arrowPrefabs[Random.Range(0, 4)];
            instantArrows.Add(Instantiate(randomArrow, new Vector2(gameObject.transform.position.x + intPos, gameObject.transform.position.y), randomArrow.transform.rotation));
            instantArrows[i].transform.SetParent(gameObject.transform);
            instantArrows[i].transform.localScale = new Vector3(1, 1, 1);
            intPos += 45f;
        }

        // Sets timer to deactivate defend manager
        StartCoroutine(commandTimer(2f));
    }

    private void Update()
    {
        pressCommands();
    }

    // Main arrow mechanic
    void pressCommands()
    {
        // If there are arrows to play
        if(instantArrows.Count > 0)
        {
            // Checks the direction of the first arrow of the list
            switch (instantArrows[0].name)
            {
                case "down(Clone)":
                    // If player hits the arrow
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        killArrow(instantArrows[0]);
                    } // If player fails the arrow
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
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
            StartCoroutine(animWait());
        }
        
    }

    // Improves agility, animates the arrow and removes it from the list
    void killArrow(GameObject arrow)
    {
        extraAgility++;
        instantArrows.RemoveAt(0);
        arrow.transform.DOLocalJump(new Vector2(arrow.transform.localPosition.x, arrow.transform.localPosition.y + 10), 6, 1, 0.3f).OnComplete(() => Destroy(arrow.gameObject));
    }

    // Reduces agility, animates and removes it from the list 
    void failArrow(GameObject arrow)
    {
        extraAgility--;
        instantArrows.RemoveAt(0);
        arrow.transform.DOShakePosition(0.3f, 4, 20).OnComplete(() => Destroy(arrow.gameObject));
    }

    // Deactivates defend manager
    IEnumerator commandTimer(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    // Waits for the arrow anim to finish before deactivating the defend manager, so it doesn't conflict with DOTween.
    IEnumerator animWait()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    // Adds extra agility, limits agility and destroys arrows
    private void OnDisable()
    {
        playerUnit.currentAgility += extraAgility;

        // Triggers miss mechanic and animation if 4 arrows were hit
        if (extraAgility == 4)
        {
            playerUnit.missed = true;
            playerAnimator.SetBool("Evade", true);
        } else
        {
            playerUnit.missed = false;
            playerAnimator.SetBool("DF", true);
        }

        if (playerUnit.currentAgility < 0)
        {
            playerUnit.currentAgility = 0;
        }

        foreach (Transform child in transform)
        {
            instantArrows.Clear();
            Destroy(child.gameObject);
        }
    }
}

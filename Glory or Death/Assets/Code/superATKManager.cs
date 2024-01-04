using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class superATKManager : MonoBehaviour
{
    Tutorial_UI _tutorial_UI;
    Camera MainCamera;
    timeManager timeManager;
    Player Player;
    Enemy Enemy;
    DialogueManager DialogueManager;

    static int hits;
    int targetAmount = 10;
    int spawnCounter;

    [SerializeField] Image Overlay;
    [SerializeField] Canvas Canvas;
    [SerializeField] Image[] swordFeeds;

    [SerializeField] GameObject targetPrefab;

    private void Awake()
    {
        importManagers();
    }
    private void OnEnable()
    {
        startMinigame();
        StartCoroutine(generateTarget(0.3f));
    }

    void startMinigame()
    {
        spawnCounter = 0;
        hits = 0;
        showFeedback();
        if (gameManager.isTutorial())
        {
            _tutorial_UI.fadeTimer(0);
            _tutorial_UI.selectIcon("Default");
        } else
        {
            Combat_UI.move_UI_out();
            timeManager.stopUnitTimer();
        }
        Overlay.DOFade(.9f, 0.5f);
        zoomCameraIn();
    }

    void zoomCameraIn()
    {
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -0.5f, -10), 1f);
        MainCamera.DOFieldOfView(40, 1f);
    }
    void finishMinigame()
    {
        if (gameManager.isTutorial())
        {
            DialogueManager.superHitCheck();
        }
        
        Overlay.DOFade(0, 0.5f);
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
        MainCamera.DOFieldOfView(50, 0.5f).OnComplete(playAnim);
        void playAnim()
        {
            hideFeedback();
            if (!gameManager.isTutorial())
            {
                Player.GetComponent<Animator>().Play("ATK2");
                Enemy.GetComponent<Animator>().Play("Super_Hurt");
                MainCamera.GetComponent<Animator>().enabled = true;
                MainCamera.GetComponent<Animator>().Play("Cam_ATK2_Player");
            } else
            {
                _tutorial_UI.fadeTimer(1);
            }
            Invoke("deactivateATK2", 2);
        }
    }

    IEnumerator generateTarget(float n)
    {
        for (int i = 0; i < targetAmount; i++)
        {
            float randomX = Random.Range(2, 8);
            float randomY = Random.Range(2, -4);
            GameObject target = Instantiate(targetPrefab, Canvas.transform);
            target.transform.SetPositionAndRotation(new Vector3(randomX, randomY, 0), Quaternion.identity);
            yield return new WaitForSeconds(n);
        }
    }


    // G&S
    public static int GetHits()
    {
        return hits;
    }

    // Tools
    public void deactivateATK2()
    {
        gameObject.SetActive(false);
    }
    public void showFeedback()
    {
        foreach (Image feed in swordFeeds)
        {
            feed.DOFade(.25f, 0.5f);
        }
    }
    public void hideFeedback()
    {
        foreach (Image feed in swordFeeds)
        {
            feed.DOFade(0, 0.5f);
        }
    }
    public void activateFeedSwords()
    {
        swordFeeds[spawnCounter].DOFade(1, 0.2f);
    }
    public void IncrementHits()
    {
        hits++;
    }
    public void IncrementSpawnCounter()
    {
        spawnCounter++;
        checkForLimit();
    }

    public void checkForLimit()
    {
        if (spawnCounter == targetAmount)
        {
            finishMinigame();
        }
    }


    void importManagers()
    {
        _tutorial_UI = FindObjectOfType<Tutorial_UI>();
        Player = FindObjectOfType<Player>();
        Enemy = FindObjectOfType<Enemy>();
        MainCamera = FindObjectOfType<Camera>();
        timeManager = FindObjectOfType<timeManager>();
        DialogueManager = FindObjectOfType<DialogueManager>();
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class superATKManager : MonoBehaviour
{
    TutorialManager TutorialManager;
    Camera MainCamera;
    timeManager timeManager;
    Player Player;
    Enemy Enemy;
    DialogueManager DialogueManager;
    GoalManager GoalManager;

    Animator cameraAnimator;

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

    private void OnDisable()
    {
        if (gameManager.isTutorial())
        {
            GoalManager.MoveGoal(1);
            TutorialManager.showUI();
        }
    }

    void startMinigame()
    {
        spawnCounter = 0;
        hits = 0;
        showFeedback();
        if (gameManager.isTutorial())
        {
            TutorialManager.fadeTimer(0);
            TutorialManager.selectIcon("Default");
        } else
        {
            Combat_UI.move_UI_out();
            timeManager.stopUnitTimer();
        }
        
        zoomCameraIn();
    }

    
    void finishMinigame()
    {
        if (gameManager.isTutorial())
        {
            DialogueManager.superHitCheck();
        }
        zoomCameraOut(); // Also plays anims
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
    void zoomCameraIn()
    {
        Overlay.DOFade(.9f, 0.5f);
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -0.5f, -10), .3f);
        MainCamera.DOFieldOfView(40, .3f);
    }
    void zoomCameraOut()
    {
        Overlay.DOFade(0, 0.5f);
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.3f);
        MainCamera.DOFieldOfView(50, 0.3f).OnComplete(playAnim);
        void playAnim()
        {
            hideFeedback();
            if (!gameManager.isTutorial())
            {
                playUnitAnim();
                playCameraAnim();
                gameObject.SetActive(false);
            }
            else
            {
                TutorialManager.fadeTimer(1);
                gameObject.SetActive(false);
            }
        }
    }
    void playUnitAnim()
    {
        Player.GetComponent<Animator>().Play("ATK2");
        Enemy.GetComponent<Animator>().Play("Super_Hurt");
    }
    void playCameraAnim()
    {
        cameraAnimator.enabled = true;
        cameraAnimator.Rebind();
        cameraAnimator.Play("Cam_ATK2_Player");
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
        GoalManager = FindObjectOfType<GoalManager>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        Player = FindObjectOfType<Player>();
        Enemy = FindObjectOfType<Enemy>();
        MainCamera = FindObjectOfType<Camera>();
        timeManager = FindObjectOfType<timeManager>();
        DialogueManager = FindObjectOfType<DialogueManager>();

        cameraAnimator = MainCamera.GetComponent<Animator>();
    }
}

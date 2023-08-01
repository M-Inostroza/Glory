using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class superATKManager : MonoBehaviour
{
    BattleSystem BattleSystem;
    Camera MainCamera;
    Combat_UI combat_UI;
    timeManager timeManager;
    Player Player;
    Enemy Enemy;

    int hits;
    int targetAmount;
    int spawnCounter;

    [SerializeField] Image Overlay;
    [SerializeField] Canvas Canvas;
    [SerializeField] GameObject vFeedback;
    [SerializeField] Image[] swordFeeds;

    [SerializeField] GameObject targetPrefab;

    private void Awake()
    {
        Player = FindObjectOfType<Player>();
        Enemy = FindObjectOfType<Enemy>();
        BattleSystem = FindObjectOfType<BattleSystem>();
        MainCamera = FindObjectOfType<Camera>();
        combat_UI = FindObjectOfType<Combat_UI>();
        timeManager = FindObjectOfType<timeManager>();
    }
    private void OnEnable()
    {
        startMinigame();
        StartCoroutine(generateTarget(0.3f));
    }

    private void Update()
    {
        if (spawnCounter == targetAmount)
        {
            finishMinigame();
        }
    }
    void startMinigame()
    {
        hits = 0;
        spawnCounter = 0;
        showFeedback();
        timeManager.stopUnitTimer();
        Overlay.DOFade(.9f, 0.5f);
        combat_UI.move_UI_out();
        zoomCameraIn();
        targetAmount = 10;
    }

    void zoomCameraIn()
    {
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -0.5f, -10), 1f);
        MainCamera.DOFieldOfView(40, 1f);
    }
    void finishMinigame()
    {
        Overlay.DOFade(0, 0.5f);
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
        MainCamera.DOFieldOfView(50, 0.5f).OnComplete(playAnim);
        void playAnim()
        {
            hideFeedback();
            Player.GetComponent<Animator>().Play("ATK2");
            Enemy.GetComponent<Animator>().Play("Super_Hurt");
            MainCamera.GetComponent<Animator>().enabled = true;
            MainCamera.GetComponent<Animator>().Play("Cam_ATK2_Player");
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
    public int GetHits()
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
    }
}

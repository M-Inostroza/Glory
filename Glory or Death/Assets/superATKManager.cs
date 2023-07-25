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
    int hits;
    int targetAmount;
    int spawnCounter = 0;

    [SerializeField] Image Overlay;
    [SerializeField] Canvas Canvas;
    [SerializeField] GameObject vFeedback;
    [SerializeField] Image[] swordFeeds;

    [SerializeField] GameObject targetPrefab;

    private void Awake()
    {
        BattleSystem = FindObjectOfType<BattleSystem>();
        MainCamera = FindObjectOfType<Camera>();
        combat_UI = FindObjectOfType<Combat_UI>();
        timeManager = FindObjectOfType<timeManager>();
    }
    private void OnEnable()
    {
        startMinigame();
        StartCoroutine(generateTarget(0.4f));
    }

    private void Update()
    {
        if (spawnCounter == targetAmount)
        {
            zoomCameraOut(); // Also deactivates GO and overlay
        }
    }
    void startMinigame()
    {
        vFeedback.SetActive(true);
        timeManager.stopUnitTimer();
        Overlay.DOFade(.9f, 0.5f);
        combat_UI.move_UI_out();
        zoomCameraIn();
        targetAmount = 10;
    }

    public void checkCritic()
    {
        if (hits == 7 && !BattleSystem.GetDeadEnemy())
        {
            combat_UI.showStars();
        }
    }

    void zoomCameraIn()
    {
        MainCamera.transform.DOLocalMove(new Vector3(3.3f, -0.5f, -10), 2f);
        MainCamera.DOFieldOfView(40, 2f);
    }
    void zoomCameraOut()
    {
        Overlay.DOFade(0, 0.5f);
        MainCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
        MainCamera.DOFieldOfView(50, 0.5f).OnComplete(()=> gameObject.SetActive(false));
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
    public void IncrementHits()
    {
        hits++;
    }
    public void IncrementSpawnCounter()
    {
        spawnCounter++;
    }
}

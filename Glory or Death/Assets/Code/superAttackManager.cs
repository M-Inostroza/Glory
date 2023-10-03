using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class superAttackManager : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] Transform spawnRotator;
    [SerializeField] Transform[] swordSpawners;

    [SerializeField] GameObject swordProyectilePrefab, parentCanvas;
    [SerializeField] Transform heartTarget, shield, feedbackContainer;
    [SerializeField] float rotationSpeed;
    [SerializeField] int swordNumber;
    [SerializeField] Camera mainCamera;

    cameraManager cameraManager;
    Combat_UI combat_UI;
    Enemy enemy;
    AudioManager audioManager;
    Tutorial_UI tutorial_UI;

    int swordCounter = 0;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        combat_UI = FindObjectOfType<Combat_UI>();
        cameraManager = FindObjectOfType<cameraManager>();
        enemy = FindObjectOfType<Enemy>();
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
    }
    private void Update()
    {
        rotateSpawners();
        rotateOnKey();
        setEnemySuperDMG();
    }
    
    private void OnEnable()
    {
        moveFeedback();
        audioManager.Play("Super_Attack_Enemy_On");
        cameraManager.playChrome();
        moveCameraIn();
        StartCoroutine(MinigameTimer(7));
        StartCoroutine(SpawnSwordsWithDelay(0.4f));
    }

    private void OnDisable()
    {
        if (gameManager.isTutorial())
        {
            tutorial_UI.fadeTimer(1);
            if (Tutorial_UI._hasPlayedTutorial)
            {
                tutorial_UI.showAllInput(1);
            }
        }
        swordCounter = 0;
        resetFeedback();
    }

    IEnumerator SpawnSwordsWithDelay(float delay)
    {
        for (int i = 0; i < swordNumber; i++)
        {
            var randomNumber = Random.Range(0, swordSpawners.Length);
            Transform randomSpawner = swordSpawners[randomNumber];
            var bullet = Instantiate(swordProyectilePrefab, new Vector3(randomSpawner.transform.localPosition.x, randomSpawner.transform.localPosition.y, 0), Quaternion.identity, parentCanvas.transform);
            PointSpriteTowards(heartTarget.localPosition, bullet.transform);
            bullet.transform.DOMove(new Vector3(spawnRotator.transform.position.x, spawnRotator.transform.position.y, 0), 2.2f).SetEase(Ease.OutCirc);

            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator MinigameTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        moveCameraOut();
        gameObject.SetActive(false);
        if (!gameManager.isTutorial())
        {
            enemy.GetComponent<Animator>().Play("Attack_Strong");
            FindObjectOfType<Player>().GetComponent<Animator>().Play("superDMG");
        }
    }

    // Tools
    void rotateSpawners()
    {
        spawnRotator.DOLocalRotate(new Vector3(0, 0, -360), 3);
    }
    void PointSpriteTowards(Vector3 targetPosition, Transform bullet)
    {
        Vector3 direction = targetPosition - bullet.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
    void rotateOnKey()
    {
        if (!BattleSystem.IsPaused)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                float rotationAmount = rotationSpeed * Time.deltaTime;
                shield.transform.Rotate(0f, 0f, -rotationAmount);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                float rotationAmount = rotationSpeed * Time.deltaTime;
                shield.transform.Rotate(0f, 0f, rotationAmount);
            }
        }
    }

    void setEnemySuperDMG()
    {
        if (!gameManager.isTutorial())
        {
            enemy.setSuperDMG(swordCounter);
        }
    }

    
    public void fillSword()
    {
        feedbackContainer.GetChild(swordCounter).GetComponent<Image>().DOFade(1, 0.4f);
        feedbackContainer.GetChild(swordCounter).DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        swordCounter++;
    }
    public void resetFeedback()
    {
        foreach (Transform sword in feedbackContainer.transform)
        {
            sword.GetComponent<Image>().DOFade(0.2f, 0);
        }
    }


    // Effects
    void moveFeedback()
    {
        if (gameManager.isTutorial())
        {
            feedbackContainer.DOLocalMoveY(-2, 0.3f);
        } else
        {
            feedbackContainer.DOLocalMoveY(10, 0.3f);
        }
    }
    void moveCameraIn()
    {
        mainCamera.DOFieldOfView(40, 0.8f);
    }
    void moveCameraOut()
    {
        mainCamera.DOFieldOfView(50, 0.8f);
    }
}

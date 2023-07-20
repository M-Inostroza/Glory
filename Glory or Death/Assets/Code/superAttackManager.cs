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
    [SerializeField] float bulletSpeed, rotationSpeed;
    [SerializeField] int swordNumber;
    [SerializeField] Camera mainCamera;

    cameraManager cameraManager;
    Combat_UI combat_UI;
    AudioManager audioManager;
    Enemy enemy;

    int swordCounter = 0;

    private void Awake()
    {
        combat_UI = FindObjectOfType<Combat_UI>();
        cameraManager = FindObjectOfType<cameraManager>();
        audioManager = FindObjectOfType<AudioManager>();
        enemy = FindObjectOfType<Enemy>();
    }
    private void Update()
    {
        rotateSpawners();
        rotateOnKey();
        setEnemySuperDMG();
    }
    
    private void OnEnable()
    {
        feedbackContainer.DOLocalMoveY(10, 0.5f);
        audioManager.Play("Super_Attack_Enemy_On");
        cameraManager.playChrome();
        combat_UI.move_UI_out();
        moveCameraIn();
        StartCoroutine(MinigameTimer(5f));
        StartCoroutine(SpawnSwordsWithDelay(0.3f));
        StartCoroutine(slowMotion(4.5f, 0.5f));
    }

    private void OnDisable()
    {
        swordCounter = 0;
    }

    IEnumerator SpawnSwordsWithDelay(float delay)
    {
        for (int i = 0; i < swordNumber; i++)
        {
            var randomNumber = Random.Range(0, swordSpawners.Length);
            Transform randomSpawner = swordSpawners[randomNumber];
            var bullet = Instantiate(swordProyectilePrefab, new Vector3(randomSpawner.transform.localPosition.x, randomSpawner.transform.localPosition.y, 0), Quaternion.identity, parentCanvas.transform);
            PointSpriteTowards(heartTarget.localPosition, bullet.transform);
            bullet.transform.DOMove(new Vector3(spawnRotator.transform.position.x, spawnRotator.transform.position.y, 0), 1.4f);

            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator MinigameTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        moveCameraOut();
        gameObject.SetActive(false);
        enemy.GetComponent<Animator>().Play("Attack_Strong");
        FindObjectOfType<Player>().GetComponent<Animator>().Play("superDMG");
    }

    // Tools
    void rotateSpawners()
    {
        spawnRotator.DOLocalRotate(new Vector3(0, 0, 180), 4).SetEase(Ease.Linear);
    }
    void PointSpriteTowards(Vector3 targetPosition, Transform bullet)
    {
        Vector3 direction = targetPosition - bullet.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
    void rotateOnKey()
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

    void setEnemySuperDMG()
    {
        enemy.setSuperDMG(swordCounter);
    }

    
    public void fillSword()
    {
        feedbackContainer.GetChild(swordCounter).GetComponent<Image>().DOFade(1, 0.4f);
        feedbackContainer.GetChild(swordCounter).DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
        swordCounter++;
    }

    

    // Effects
    void moveCameraIn()
    {
        mainCamera.DOFieldOfView(40, 0.8f);
    }
    void moveCameraOut()
    {
        mainCamera.DOFieldOfView(50, 0.8f);
    }

    IEnumerator slowMotion(float seconds, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 1;
    }
}

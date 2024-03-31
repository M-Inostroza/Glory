using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SuperCounterManager : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] Transform spawnRotator;
    [SerializeField] Transform[] swordSpawners;

    [SerializeField] GameObject swordProyectilePrefab, parentCanvas;
    [SerializeField] Transform heartTarget, shield, feedbackContainer;
    
    [SerializeField] int swordNumber;
    [SerializeField] Camera mainCamera;


    [SerializeField] Image[] _comandArrows;

    cameraManager CameraManager;

    Enemy enemy;
    Animator _enemyAnimator;

    [SerializeField] GameObject _doll;
    Animator _dollAnimator;

    Player _player;
    Animator _playerAnimator;

    AudioManager audioManager;
    TutorialManager TutorialManager;
    DialogueManager dialogueManager;

    private int swordCounter = 0;
    private float rotationSpeed;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        CameraManager = FindObjectOfType<cameraManager>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();

        _player = FindObjectOfType<Player>();
        _playerAnimator = _player.GetComponent<Animator>();
        enemy = FindObjectOfType<Enemy>();

        if (!GameManager.isTutorial())
        {
            _enemyAnimator = enemy.GetComponent<Animator>();
        } else
        {
            _dollAnimator = _doll.GetComponent<Animator>();
        }
    }
    private void Update()
    {
        RotateSpawners();
        RotateShieldOnKey();
        SetEnemySuperDMG();
        ShowKeys();
    }
    
    private void OnEnable()
    {
        playAllEffects();
        StartCoroutine(MinigameTimer(7));
        StartCoroutine(SpawnSwordsWithDelay(0.4f));
    }

    private void OnDisable()
    {
        handleTutorial();
        resetVisualSwordFeedback();
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
            bullet.transform.DOMove(new Vector3(spawnRotator.transform.position.x, spawnRotator.transform.position.y, 0), 2.2f).SetEase(Ease.OutCirc);

            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator MinigameTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        moveCameraOut();
        gameObject.SetActive(false);
        if (!GameManager.isTutorial())
        {
            _enemyAnimator.Play("Attack_Strong");
            _playerAnimator.Play("superDMG");
        }
    }


    // Rotates the random position of swords and points the swords to the center
    void RotateSpawners()
    {
        spawnRotator.DOLocalRotate(new Vector3(0, 0, -360), 3);
    }
    void PointSpriteTowards(Vector3 targetPosition, Transform bullet)
    {
        Vector3 direction = targetPosition - bullet.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }


    // Manages minigame in tutorial mode
    void handleTutorial()
    {
        if (GameManager.isTutorial())
        {
            // Check swords
            if (swordCounter > 4 && !TutorialManager.hasShownDetail_superCounter)
            {
                TutorialManager.superCounterDetailTutorial(1);
            } else
            {
                TutorialManager.superCounterDetailTutorial(2);
            }

            TutorialManager.fadeTimer(1);
            TutorialManager.showUI();

            if (TutorialManager._hasPlayedTutorial)
            {
                TutorialManager.showAllInput(1);
            }
        }
    }


    /* Uses the arrow keys to rotate the shield
    Modify rotation speed with: rotationSpeed */
    void RotateShieldOnKey()
    {
        if (!BattleSystem.IsPaused)
        {
            if (GameManager.isTutorial())
            {
                rotationSpeed = 80;
            } else
            {
                rotationSpeed = 70;
            }
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

    void ShowKeys()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _comandArrows[0].DOFade(0.5f, 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _comandArrows[0].DOFade(0.3f, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _comandArrows[1].DOFade(0.5f, 0);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _comandArrows[1].DOFade(0.3f, 0);
        }
    }


    // Sets enemy super damage (Not Tutorial)
    void SetEnemySuperDMG()
    {
        if (!GameManager.isTutorial())
        {
            enemy.setSuperDMG(swordCounter);
        }
    }


    // Fades in, animates and adds 1 to the sword counter
    public void fillSword()
    {
        feedbackContainer.GetChild(swordCounter).GetComponent<Image>().DOFade(1, 0.4f);
        feedbackContainer.GetChild(swordCounter).DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        swordCounter++;
    }


    // Resets the sword feedback to 0.2 Opacity
    public void resetVisualSwordFeedback()
    {
        foreach (Transform sword in feedbackContainer.transform)
        {
            sword.GetComponent<Image>().DOFade(0.2f, 0);
        }
    }


    // Effects
    void playAllEffects()
    {
        // TO DO: Activate left right
        if (GameManager.isTutorial())
        {
            TutorialManager.hideUI();
            if (!TutorialManager.hasShownDetail_superCounter)
            {
                StartCoroutine(TutorialManager.toggleInput(7, 0));
            }
        } else 
        {
            CombatManager.move_UI_out();
        }
        moveFeedback();
        audioManager.Play("Super_Attack_Enemy_On");
        CameraManager.playChrome();
        moveCameraIn();
    }
    void moveFeedback()
    {
        if (GameManager.isTutorial())
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

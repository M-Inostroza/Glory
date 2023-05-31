using UnityEngine;
using DG.Tweening;

public class defendManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;

    private AudioManager audioManager;
    private shieldPool shieldPool;

    // Object that shows the total size of the target
    [SerializeField] private GameObject shadow, playerUnit;
    public GameObject enemyUnit;

    // Control
    bool defendSuccess;
    bool transformControl;

    Tween scaleUP;

    private void OnEnable()
    {
        scaleUP = transform.DOScale(1, 2f).SetEase(Ease.InOutQuad).OnComplete(()=> Fail());
        defendSuccess = false;
        shadow.SetActive(true);
        transformControl = true;
    }

    private void Start()
    {
        shieldPool = FindObjectOfType<shieldPool>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        controlDefend();
    }

    void executeShield(float scaleLimit)
    {
        Debug.Log("The shield scale: " + transform.localScale.x);
        if (transform.localScale.x < scaleLimit )
        {
            transform.DOShakePosition(0.4f, 0.05f, 40);
            Fail();
        }
        else if (transform.localScale.x > scaleLimit && transform.localScale.x < 95)
        {
            audioManager.Play("defend_success");
            scaleUP.Rewind();
            defendSuccess = true;
            shieldPool.AddShield();
            playerUnit.GetComponent<Player>().SetCurrentShield(+1);
            playerAnim.SetBool("skillShieldSuccess", true);
        } 
        closeMinigame();
    }

    // Method to handle enemy's defeat
    void Fail()
    {
        scaleUP.Rewind();
        audioManager.Play("defend_fail");
        playerAnim.SetBool("skillFail", true);
        closeMinigame();
    }

    void closeMinigame()
    {
        shadow.SetActive(false);
        gameObject.SetActive(false);
    }

    void controlDefend()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            executeShield(0.8f);
        }

        if (transform.localScale.x == 1 && transformControl)
        {
            transform.DOScale(0, 0);
            Fail();
        }
    }
    private void OnDisable()
    {
        transformControl = false;
    }
}



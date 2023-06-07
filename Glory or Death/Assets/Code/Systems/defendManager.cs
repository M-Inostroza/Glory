using UnityEngine;
using DG.Tweening;

public class defendManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;

    private AudioManager audioManager;
    
    // Object that shows the total size of the target
    [SerializeField] private GameObject shadow, playerUnit;
    public GameObject enemyUnit;

    // Control
    bool transformControl;
    Tween scaleUP;


    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    private void Update()
    {
        controlDefend();
    }

    void executeShield(float scaleLimit)
    {
        if (transform.localScale.x < scaleLimit )
        {
            transform.DOShakePosition(0.4f, 0.05f, 40);
            Fail();
        }
        else if (transform.localScale.x > scaleLimit && transform.localScale.x < 95)
        {
            audioManager.Play("defend_success");
            scaleUP.Rewind();
            increaseShield();
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

    public void increaseShield()
    {
        playerUnit.GetComponent<Player>().increaseCurrentShield();
    }
    public void decreaseShield()
    {
        playerUnit.GetComponent<Player>().decreaseCurrentShield();
    }

    public void activateShieldMinigame()
    {
        scaleUP = transform.DOScale(1, 2f).SetEase(Ease.InOutQuad).OnComplete(() => Fail());
        shadow.SetActive(true);
        transformControl = true;
        audioManager.Play("Shield_charge");
    }

    void closeMinigame()
    {
        shadow.SetActive(false);
        transformControl = false;
    }
}



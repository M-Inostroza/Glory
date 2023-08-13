using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class comicManager : MonoBehaviour
{
    [SerializeField] Image darkOverlay;

    [SerializeField] Transform mainCamera;

    // Loading Screen
    [SerializeField] Transform enemyPanel;
    [SerializeField] Transform playerPanel;

    [SerializeField] GameObject loadingContainer;
    [SerializeField] GameObject thunder;



    public void playNext(int next)
    {
        GetComponent<Animator>().Play("stripe_" + next);
    }

    public void activateLoadingScreen()
    {
        loadingContainer.SetActive(true);
        enemyPanel.DOLocalMoveX(308, 1).SetEase(Ease.OutBounce);
        playerPanel.DOLocalMoveX(-308, 1).SetEase(Ease.OutBounce);
        Invoke("activateThunder", 0.4f);
        Invoke("changeScene", 3);
    }
    void changeScene()
    {
        SceneManager.LoadScene("Arena");
    }

    public void activateThunder()
    {
        //loadingContainer.transform.DOShakePosition(0.5f, 10, 15, 20);
        thunder.GetComponent<Animator>().Play("thunder");
        AudioManager.Play("clash_glory");
        AudioManager.Play("thunder_title");
    }


    // Sounds
    public void walkingSequence()
    {
        AudioManager.Play("walk_" + Random.Range(1, 4));
    }
    public void keyOpen()
    {
        AudioManager.Play("Key_open");
    }
    public void water()
    {
        AudioManager.Play("water");
    }
}

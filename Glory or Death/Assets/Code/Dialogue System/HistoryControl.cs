using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HistoryControl : MonoBehaviour
{
    TimeManager TimeManager;

    [SerializeField] Transform _panelPlayer;
    [SerializeField] Transform _panelEnemy;

    [SerializeField] Transform _dialogueOverlay;
    Image _overlayImage;

    /* Player */
    [SerializeField] private List<GameObject> _facesPlayer;
    private Image _framePlayer;
    private TMP_Text _textPlayer;

    /* Enemy */
    private Image _frameEnemy;
    private TMP_Text _textEnemy;


    /* Control history */
    bool _storyIndex;
    /*
     - 0 = Presentation
     - 1 = Angry Enemy
     */

    void Start()
    {
        TimeManager = FindObjectOfType<TimeManager>();
        _framePlayer = _panelPlayer.GetChild(0).GetComponent<Image>();
        _textPlayer = _panelPlayer.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();

        _frameEnemy = _panelEnemy.GetChild(0).GetComponent<Image>();
        _textEnemy = _panelEnemy.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();

        _overlayImage = _dialogueOverlay.GetComponent<Image>();

        TimeManager.StopTime();

        Overture();
    }

    /* -----Overture----- */
    public void Overture()
    {
        _dialogueOverlay.gameObject.SetActive(true);
        _overlayImage.DOFade(0.8f, (0.3f - 0.1f));

        StartCoroutine(Interact(1, 4, 1, "Yet another noob..."));
        StartCoroutine(Interact(0, 3, 5f, "", "Such a friendly welcome...", true));
    }


    public IEnumerator Interact(
        int unit, 
        float duration, 
        float delay = 0, 
        string enemyText = "", 
        string playerText = "", 
        bool isEnd = false)
    {
        /*
         * 0 = player
         * 1 = enemy
         */
        float appearSpeed = 0.3f;
        yield return new WaitForSeconds(delay);
        if (unit == 0)
        {
            ActivatePlayer(0.2f, appearSpeed, true);
            _textPlayer.text = playerText;
            yield return new WaitForSeconds(duration);
            ActivatePlayer(0.2f, appearSpeed, false);
        }
        else
        {
            ActivateEnemy(0.2f, appearSpeed, true);
            _textEnemy.text = enemyText;
            yield return new WaitForSeconds(duration);
            ActivateEnemy(0.2f, appearSpeed, false);
        }
        if (isEnd)
        {
            _overlayImage.DOFade(0f, (0.3f - 0.1f)).OnComplete(() => _dialogueOverlay.gameObject.SetActive(false));
        }
    }

    void ActivatePlayer(float fadeSpeed, float moveSpeed, bool inOut)
    {
        if (inOut)
        {
            _panelPlayer.gameObject.SetActive(true);

            _framePlayer.DOFade(1, fadeSpeed);
            _textPlayer.DOFade(1, fadeSpeed);
            _panelPlayer.transform.DOLocalMoveX(210, moveSpeed);
        }
        else
        {
            _framePlayer.DOFade(0, fadeSpeed);
            _textPlayer.DOFade(0, fadeSpeed);
            _panelPlayer.transform.DOLocalMoveX(-260, moveSpeed).OnComplete(() => _panelEnemy.gameObject.SetActive(true));
        }
    }

    void ActivateEnemy(float fadeSpeed, float moveSpeed, bool inOut)
    {
        if (inOut)
        {
            _panelEnemy.gameObject.SetActive(true);

            _frameEnemy.DOFade(1, fadeSpeed);
            _textEnemy.DOFade(1, fadeSpeed);
            _panelEnemy.transform.DOLocalMoveX(0, moveSpeed);
        }
        else
        {
            _frameEnemy.DOFade(0, fadeSpeed);
            _textEnemy.DOFade(0, fadeSpeed);
            _panelEnemy.transform.DOLocalMoveX(560, moveSpeed).OnComplete(()=> _panelEnemy.gameObject.SetActive(true));
        }
    }
}

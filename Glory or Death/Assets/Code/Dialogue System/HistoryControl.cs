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
     */

    void Start()
    {
        TimeManager = FindObjectOfType<TimeManager>();
        _framePlayer = _panelPlayer.GetChild(1).GetComponent<Image>();
        _textPlayer = _panelPlayer.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();

        _frameEnemy = _panelEnemy.GetChild(0).GetComponent<Image>();
        _textEnemy = _panelEnemy.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();

        TimeManager.StopTime();

        StartCoroutine(Interact(1, true, 1));

    }



    public IEnumerator Interact(int unit, bool inOut, float delay = 0, string text = "Test")
    {
        /*
         * 0 = player
         * 1 = enemy
         */
        float speed = 0.3f;
        yield return new WaitForSeconds(delay);
        if (inOut)
        {
            if (unit == 0)
            {
                ActivatePlayer(0.2f, speed);
            }
            else
            {
                ActivateEnemy(0.2f, speed, true);
            }
        }
    }

    void ActivatePlayer(float fadeSpeed, float moveSpeed)
    {
        _panelPlayer.gameObject.SetActive(true);

        _framePlayer.DOFade(1, fadeSpeed);
        _textPlayer.DOFade(1, fadeSpeed);
        _panelPlayer.transform.DOLocalMoveX(250, moveSpeed);
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
            _panelEnemy.transform.DOLocalMoveX(-260, moveSpeed).OnComplete(()=> _panelEnemy.gameObject.SetActive(true));
        }
    }
}

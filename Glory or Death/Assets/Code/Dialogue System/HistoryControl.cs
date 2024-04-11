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

    void Start()
    {
        TimeManager = FindObjectOfType<TimeManager>();
        _framePlayer = _panelPlayer.GetChild(1).GetComponent<Image>();
        _textPlayer = _panelPlayer.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();

        TimeManager.StopTime();

        StartCoroutine(Interact(0, true, 1));
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
                _panelPlayer.gameObject.SetActive(true);

                _framePlayer.DOFade(1, 0.2f);
                _textPlayer.DOFade(1, 0.2f);
                _panelPlayer.transform.DOLocalMoveX(250, speed);
            }
        }
    }

}

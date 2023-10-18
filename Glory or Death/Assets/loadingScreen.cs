using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class loadingScreen : MonoBehaviour
{
    private static GameObject _leftPanel;
    private static GameObject _rightPanel;

    void Start()
    {
        _leftPanel = gameObject.transform.GetChild(0).gameObject;
        _rightPanel = gameObject.transform.GetChild(1).gameObject;
    }

    public static void toggleLoadingScreen(int inOut, float speed)
    {
        // 1 = in
        if (inOut == 1)
        {
            _leftPanel.SetActive(true);
            _rightPanel.SetActive(true);
            _leftPanel.transform.DOLocalMoveX(-250, speed);
            _rightPanel.transform.DOLocalMoveX(250, speed);
        } else
        {
            _leftPanel.transform.DOLocalMoveX(750, speed).OnComplete(()=> _leftPanel.SetActive(false));
            _rightPanel.transform.DOLocalMoveX(-750, speed).OnComplete(() => _rightPanel.SetActive(false));
        }
    }
}

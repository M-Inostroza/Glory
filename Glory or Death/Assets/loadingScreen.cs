using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class loadingScreen : MonoBehaviour
{
    private static GameObject _leftPanel;
    private static GameObject _rightPanel;

    private static Slider _loadingSlider;

    [SerializeField] private Transform _startButton;
    bool isActive = false;
    void Start()
    {
        _leftPanel = gameObject.transform.GetChild(0).gameObject;
        _rightPanel = gameObject.transform.GetChild(1).gameObject;
        _loadingSlider = gameObject.transform.GetChild(2).transform.GetComponent<Slider>();
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

    public static IEnumerator fillLoadingSlider(float showDelay, float fillDelay)
    {
        yield return new WaitForSeconds(showDelay);
        _loadingSlider.gameObject.SetActive(true);

        yield return new WaitForSeconds(fillDelay);
        _loadingSlider.value = 0;
        _loadingSlider.DOValue(_loadingSlider.maxValue, 1.5f);
    }

    public void showButton()
    {
        if (!isActive)
        {
            _startButton.GetComponent<Image>().DOFade(1, 0.05f).OnComplete(()=> _startButton.GetChild(0).gameObject.SetActive(true));
            _startButton.DOShakePosition(0.4f, 5, 80, 90);
            isActive = true;
        } else
        {
            _startButton.GetComponent<Image>().DOFade(0, 0.05f).OnComplete(() => _startButton.GetChild(0).gameObject.SetActive(false));
            isActive = false;
        }
    }

    public void openScreen()
    {
        loadingScreen.toggleLoadingScreen(0, 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private static GameObject _leftPanel;
    private static GameObject _rightPanel;

    private static Slider _loadingSlider;

    [SerializeField] private Transform _startButton;
    bool isActive = false;

    AudioManager _audioManager;
    InputManager _inputManager;
    TimeManager TimeManager;
    cameraManager _cameraManager;
    GameManager _GameManager;

    void Start()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        TimeManager = FindObjectOfType<TimeManager>();
        _cameraManager = FindObjectOfType<cameraManager>();
        _GameManager = FindObjectOfType<GameManager>();

        _leftPanel = gameObject.transform.GetChild(0).gameObject;
        _rightPanel = gameObject.transform.GetChild(1).gameObject;
        _loadingSlider = gameObject.transform.GetChild(2).transform.GetComponent<Slider>();
    }

    public void ToggleLoadingScreen(int inOut, float speed)
    {
        // 1 = in
        if (inOut == 1)
        {
            _leftPanel.SetActive(true);
            _rightPanel.SetActive(true);
            _leftPanel.transform.DOLocalMoveX(-250, speed);
            _rightPanel.transform.DOLocalMoveX(250, speed).OnComplete(()=> _audioManager.Play("Shield_metal"));
        } else
        {
            _leftPanel.transform.DOLocalMoveX(-750, speed).OnComplete(()=> _leftPanel.SetActive(false));
            _rightPanel.transform.DOLocalMoveX(750, speed).OnComplete(() => _rightPanel.SetActive(false));
        }
    }

    public IEnumerator fillLoadingSlider(float showDelay, float fillDelay)
    {
        yield return new WaitForSeconds(showDelay);
        _loadingSlider.gameObject.SetActive(true);

        yield return new WaitForSeconds(fillDelay);
        _loadingSlider.value = 0;
        _loadingSlider.DOValue(_loadingSlider.maxValue, 0.8f).OnComplete(showStartButton);
    }

    public void showStartButton()
    {
        if (!isActive)
        {
            _audioManager.Play("Thunder");
            _startButton.gameObject.SetActive(true);
            _startButton.GetComponent<Image>().DOFade(1, 0.05f).OnComplete(()=> _startButton.GetChild(0).gameObject.SetActive(true));
            _startButton.DOShakePosition(0.2f, 4, 70, 90);
            isActive = true;
        } else
        {
            _startButton.gameObject.SetActive(false);
            isActive = false;
        }
    }

    public void openScreen() // Opens the next day
    {
        StartCoroutine(TimeManager.slowMotion(0.5f, 0.2f));
        _cameraManager.playChrome();
        resetLoadingBar();

        StartCoroutine(_GameManager.DayShow(2));

        ToggleLoadingScreen(0, 0.3f);
        _inputManager.ResetCooldown();

        TimeManager.SelectEnemyAction();
        TimeManager.ResetTimers(true);

        CombatManager.move_UI_in();
        showStartButton();
        _audioManager.Play("Combat_Theme");
    }

    void resetLoadingBar()
    {
        _loadingSlider.gameObject.SetActive(false);
        _loadingSlider.value = 0;
    }
}

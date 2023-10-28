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

    AudioManager _audioManager;
    Input_Manager _inputManager;
    timeManager _timeManager;
    BattleSystem _BS;

    void Start()
    {
        _inputManager = FindObjectOfType<Input_Manager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _timeManager = FindObjectOfType<timeManager>();
        _BS = FindObjectOfType<BattleSystem>();

        _leftPanel = gameObject.transform.GetChild(0).gameObject;
        _rightPanel = gameObject.transform.GetChild(1).gameObject;
        _loadingSlider = gameObject.transform.GetChild(2).transform.GetComponent<Slider>();
    }

    public void toggleLoadingScreen(int inOut, float speed)
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
        _loadingSlider.DOValue(_loadingSlider.maxValue, 1.5f).OnComplete(showButton);
    }

    public void showButton()
    {
        if (!isActive)
        {
            _audioManager.Play("Thunder");
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
        StartCoroutine(timeManager.slowMotion(0.5f, 0.2f));
        cameraManager.playChrome();
        _loadingSlider.gameObject.SetActive(false); ;
        toggleLoadingScreen(0, 0.3f);
        _inputManager.resetCooldown();

        _timeManager.selectEnemyAction();
        _BS.resetTimers(80);

        Combat_UI.move_UI_in();
        showButton();
        _audioManager.Play("Combat_Theme");
    }
}

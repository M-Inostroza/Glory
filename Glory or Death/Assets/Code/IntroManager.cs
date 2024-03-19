using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class IntroManager : MonoBehaviour
{
    private VideoPlayer VideoPlayer;
    private cameraManager CameraManager;
    private AudioManager AudioManager;

    [Header("Loading screen")]
    [SerializeField] GameObject _loadingScreen;
    [SerializeField] Transform _playerPanel;
    [SerializeField] Transform _dollPanel;

    
    [SerializeField] private GameObject _glory, _death, _or, _thunder, _BG, _playButton, _overlay;
    [SerializeField] private ParticleSystem _electricity;
    [SerializeField] private Transform _logoContainer;

    [Header("Skip button")]
    [SerializeField] private Button _skipButton;
    [SerializeField] private TMP_Text _skipText;

    bool _hasShownPanel = false;

    void Start()
    {
        _overlay.GetComponent<Image>().DOFade(0, 0.5f).OnComplete(()=> _overlay.SetActive(false));

        CameraManager = FindObjectOfType<cameraManager>();
        AudioManager = FindObjectOfType<AudioManager>();

        VideoPlayer = GetComponent<VideoPlayer>();

        _BG.transform.DOScale(new Vector3(1, 1, 1), 2);
        StartCoroutine(introLogo(0.8f));
    }

    private void Update()
    {
        StopVideo();
    }

    void StopVideo()
    {
        if (VideoPlayer.time >= 48)
        {
            if (!_hasShownPanel)
            {
                StartCoroutine(CloseTutorialPanels(4));
            }
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

    IEnumerator introLogo(float delay)
    {
        float shakeIntensity = 2;
        float shakeDuration = 0.2f;

        yield return new WaitForSeconds(delay);
        _glory.transform.DOLocalMoveX(-54.6f, .6f).SetEase(Ease.InCubic);
        _death.transform.DOLocalMoveX(60, .6f).SetEase(Ease.InCubic).OnComplete(appear);

        void appear()
        {
            CameraManager.PlayBloom(1);
            AudioManager.Play("Thunder_Title");
            _electricity.Play();

            _glory.transform.DOShakePosition(shakeDuration, shakeIntensity, 25);
            _death.transform.DOShakePosition(shakeDuration, shakeIntensity, 25);

            _or.GetComponent<Image>().DOFade(1, 0.3f).SetDelay(0.2f);
            _thunder.GetComponent<Image>().DOFade(1, 0.3f).SetDelay(0.2f);

            _playButton.transform.DOLocalMoveY(-80, 0.5f).SetDelay(0.5f);
        }
    }

    public void StartGame()
    {
        _logoContainer.DOLocalMoveY(265, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(prepareVideo);
        _playButton.transform.DOLocalMoveY(-224, 0.5f).SetEase(Ease.InBack).SetDelay(0.05f);

        void prepareVideo()
        {
            _overlay.SetActive(true);
            _overlay.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(()=>_BG.SetActive(false));
            PlayVideo(2);
        }
    }
    void PlayVideo(float delay)
    {
        _overlay.GetComponent<Image>().DOFade(0, 1).SetDelay(delay);
        VideoPlayer.Play();
        AppearSkipButton();
    }

    public IEnumerator CloseTutorialPanels(float sceneDelay) // Deals with the panels showing at the begining of the tutorial
    {
        float panelSpeed = 0.4f;
        Transform screenContainer = _loadingScreen.transform.GetChild(0).transform;

        _hasShownPanel = true;
        _dollPanel.DOLocalMoveX(308, panelSpeed).SetEase(Ease.InCubic).OnComplete(()=> AudioManager.Play("Open_Tutorial"));
        _playerPanel.DOLocalMoveX(-308, panelSpeed).SetEase(Ease.InCubic).OnComplete(() => screenContainer.DOShakePosition(0.6f, 6, 80, 20));

        yield return new WaitForSeconds(sceneDelay);
        VideoPlayer.Stop();
        ChangeScene();
    }

    public void SkipVideo()
    {
        double targetTime = VideoPlayer.length - 11;
        if (targetTime < 0)
        {
            targetTime = 0;
        }
        VideoPlayer.time = targetTime;
    }

    void AppearSkipButton()
    {
        int delay = 4;
        _skipButton.gameObject.SetActive(true);
        _skipText.DOFade(1, 1).SetDelay(delay);
        _skipButton.GetComponent<Image>().DOFade(1, 1).SetDelay(delay);
    }

    public void DissapearSkipButton()
    {
        _skipText.DOFade(0, 0.3f);
        _skipButton.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(()=> {
            _skipButton.gameObject.SetActive(false);
        });
    }
}

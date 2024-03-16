using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class comicVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private cameraManager _cameraManager;
    private AudioManager _audioManager;

    // Logo Elements
    [SerializeField] private GameObject Glory, Death, Or, Thunder, BG, playButton, _overlay;
    [SerializeField] private ParticleSystem Electricity;

    [SerializeField] private Transform _logoContainer;

    void Start()
    {
        _overlay.GetComponent<Image>().DOFade(0, 0.5f).OnComplete(()=> _overlay.SetActive(false));

        _cameraManager = FindObjectOfType<cameraManager>();
        _audioManager = FindObjectOfType<AudioManager>();

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinished;

        BG.transform.DOScale(new Vector3(1, 1, 1), 2);
        StartCoroutine(introLogo(0.8f));
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        ChangeScene();
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
        Glory.transform.DOLocalMoveX(-54.6f, .6f).SetEase(Ease.InCubic);
        Death.transform.DOLocalMoveX(60, .6f).SetEase(Ease.InCubic).OnComplete(appear);

        void appear()
        {
            _cameraManager.PlayBloom(1);
            _audioManager.Play("Thunder_Title");
            Electricity.Play();

            Glory.transform.DOShakePosition(shakeDuration, shakeIntensity, 25);
            Death.transform.DOShakePosition(shakeDuration, shakeIntensity, 25);

            Or.GetComponent<Image>().DOFade(1, 0.3f).SetDelay(0.2f);
            Thunder.GetComponent<Image>().DOFade(1, 0.3f).SetDelay(0.2f);

            playButton.transform.DOLocalMoveY(-80, 0.5f).SetDelay(0.5f);
        }
    }

    public void StartGame()
    {
        _logoContainer.DOLocalMoveY(265, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(prepareVideo);
        playButton.transform.DOLocalMoveY(-224, 0.5f).SetEase(Ease.InBack).SetDelay(0.05f);

        void prepareVideo()
        {
            _overlay.SetActive(true);
            _overlay.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(()=>BG.SetActive(false));
            PlayVideo(2);
        }
    }
    void PlayVideo(float delay)
    {
        _overlay.GetComponent<Image>().DOFade(0, 1).SetDelay(delay);
        videoPlayer.Play();
    }
}

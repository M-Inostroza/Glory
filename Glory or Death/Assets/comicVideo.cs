using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class comicVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        ChangeScene();
    }

    void ChangeScene()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }
}

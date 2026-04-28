using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public sealed class SplashController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float fallbackSeconds = 6f;

    private bool hasLoadedNextScene;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    private void Start()
    {
        if (videoPlayer == null)
        {
            LoadBootstrapScene();
            return;
        }

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        StartCoroutine(FallbackLoadRoutine());
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        LoadBootstrapScene();
    }

    private IEnumerator FallbackLoadRoutine()
    {
        yield return new WaitForSeconds(fallbackSeconds);

        if (!hasLoadedNextScene)
        {
            LoadBootstrapScene();
        }
    }

    public void SkipSplash()
    {
        LoadBootstrapScene();
    }

    private void LoadBootstrapScene()
    {
        if (hasLoadedNextScene)
        {
            return;
        }

        hasLoadedNextScene = true;
        SceneManager.LoadScene(SceneNames.Bootstrap);
    }
}
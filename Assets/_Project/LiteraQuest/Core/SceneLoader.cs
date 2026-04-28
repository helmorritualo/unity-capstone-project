using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : MonoBehaviour
{
    [SerializeField] private LoadingScreenController loadingScreen;

    private AppState appState;

    public void Initialize(AppState state)
    {
        appState = state;

        if (loadingScreen != null)
        {
            loadingScreen.Hide();
        }
    }

    public void LoadScene(string sceneName)
    {
        if (appState != null && appState.IsLoading)
        {
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        appState?.SetLoading(true);

        if (loadingScreen != null)
        {
            loadingScreen.Show("Loading...");
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingScreen != null)
            {
                loadingScreen.SetProgress(progress);
            }

            yield return null;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetProgress(1f);
        }

        yield return new WaitForSeconds(0.2f);

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        appState?.SetCurrentScene(sceneName);
        appState?.SetLoading(false);

        if (loadingScreen != null)
        {
            loadingScreen.Hide();
        }
    }
}
using UnityEngine;

public sealed class AppState : MonoBehaviour
{
    public string CurrentSceneName { get; private set; }
    public bool IsLoading { get; private set; }

    public void SetCurrentScene(string sceneName)
    {
        CurrentSceneName = sceneName;
    }

    public void SetLoading(bool isLoading)
    {
        IsLoading = isLoading;
    }

    public void ResetSession()
    {
        CurrentSceneName = string.Empty;
        IsLoading = false;
    }
}
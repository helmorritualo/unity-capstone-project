using UnityEngine;

public sealed class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private string targetSceneName = SceneNames.Login;

    public void LoadTargetScene()
    {
        if (GameRoot.Instance == null)
        {
            Debug.LogError("GameRoot does not exist.");
            return;
        }

        GameRoot.Instance.SceneLoader.LoadScene(targetSceneName);
    }
}
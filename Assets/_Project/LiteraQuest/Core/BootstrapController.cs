using System.Collections;
using UnityEngine;

public sealed class BootstrapController : MonoBehaviour
{
    [SerializeField] private float bootDelaySeconds = 0.5f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(bootDelaySeconds);

        if (GameRoot.Instance == null)
        {
            Debug.LogError("GameRoot is missing in BootstrapScene.");
            yield break;
        }

        if (!HasBasicInternetConnection())
        {
            GameRoot.Instance.SceneLoader.LoadScene(SceneNames.OfflineMode);
            yield break;
        }

        if (!GameRoot.Instance.TokenStore.HasToken)
        {
            GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
            yield break;
        }

        yield return LoadSessionRoutine();
    }

    private IEnumerator LoadSessionRoutine()
    {
        if (GameRoot.Instance.BootstrapService == null)
        {
            Debug.LogError("BootstrapService is missing.");
            GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
            yield break;
        }

        yield return GameRoot.Instance.BootstrapService.LoadBootstrap(
            result =>
            {
                if (result.IsSuccess && result.Data != null)
                {
                    if (!HasValidClassroom(result.Data))
                    {
                        Debug.LogError("No active classroom found. Please contact your teacher.");
                        GameRoot.Instance.AuthService.ClearLocalSession();
                        GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
                        return;
                    }

                    GameRoot.Instance.AppState.SetSession(result.Data);
                    GameRoot.Instance.SceneLoader.LoadScene(SceneNames.MainMenu);
                    return;
                }

                HandleBootstrapFailure(result);
            }
        );
    }

    private void HandleBootstrapFailure(ApiResult<BootstrapResponse> result)
    {
        Debug.LogError(
    $"Bootstrap failed. Status: {result.StatusCode}, Message: {result.ErrorMessage}"
);
        bool isAuthError = result.StatusCode == 401 || result.StatusCode == 419;

        if (isAuthError)
        {
            GameRoot.Instance.AuthService.ClearLocalSession();
            GameRoot.Instance.AppState.ClearSession();
            GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
            return;
        }

        Debug.LogError(result.ErrorMessage);
        GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
    }

    private bool HasValidClassroom(BootstrapResponse response)
    {
        return response.classroom != null
            && response.classroom.grade_section != null
            && !string.IsNullOrWhiteSpace(response.classroom.grade_section.display_name);
    }

    private bool HasBasicInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
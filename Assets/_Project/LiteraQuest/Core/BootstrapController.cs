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

        bool hasConnection = HasBasicInternetConnection();

        if (hasConnection)
        {
            GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
        }
        else
        {
            GameRoot.Instance.SceneLoader.LoadScene(SceneNames.OfflineMode);
        }
    }

    private bool HasBasicInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
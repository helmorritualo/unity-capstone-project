using UnityEngine;

public sealed class ApiDebugTester : MonoBehaviour
{
    private void Start()
    {
        PrintApiClientStatus();
    }

    public void PrintApiClientStatus()
    {
        if (GameRoot.Instance == null)
        {
            Debug.LogError("GameRoot is missing.");
            return;
        }

        if (GameRoot.Instance.ApiClient == null)
        {
            Debug.LogError("ApiClient is missing.");
            return;
        }

        Debug.Log("ApiClient is ready.");
    }
}
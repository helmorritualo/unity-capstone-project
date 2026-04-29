using UnityEngine;

[CreateAssetMenu(menuName = "LiteraQuest/Api Config")]
public sealed class ApiConfig : ScriptableObject
{
    [SerializeField] private string baseUrl = "http://127.0.0.1:8000/api/v1";
    [SerializeField] private int timeoutSeconds = 15;

    public string BaseUrl => baseUrl.TrimEnd('/');
    public int TimeoutSeconds => timeoutSeconds;
}
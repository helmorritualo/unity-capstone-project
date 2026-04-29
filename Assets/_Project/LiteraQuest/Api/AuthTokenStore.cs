using UnityEngine;

public sealed class AuthTokenStore
{
    private const string TokenKey = "literaquest_student_token";

    public string Token => PlayerPrefs.GetString(TokenKey, string.Empty);

    public bool HasToken => !string.IsNullOrWhiteSpace(Token);

    public void Save(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            Debug.LogWarning("Attempted to save an empty auth token.");
            return;
        }

        PlayerPrefs.SetString(TokenKey, token);
        PlayerPrefs.Save();
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey(TokenKey);
        PlayerPrefs.Save();
    }
}
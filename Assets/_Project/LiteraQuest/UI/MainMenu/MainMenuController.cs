using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text welcomeText;
    [SerializeField] private TMP_Text gradeSectionText;
    [SerializeField] private TMP_Text subjectText;

    [Header("Buttons")]
    [SerializeField] private Button logoutButton;

    private void Start()
    {
        RenderSession();

        if (logoutButton != null)
        {
            logoutButton.onClick.AddListener(OnLogoutClicked);
        }
    }

    private void OnDestroy()
    {
        if (logoutButton != null)
        {
            logoutButton.onClick.RemoveListener(OnLogoutClicked);
        }
    }

    private void RenderSession()
    {
        if (GameRoot.Instance == null || !GameRoot.Instance.AppState.HasActiveSession)
        {
            GameRoot.Instance?.SceneLoader.LoadScene(SceneNames.Bootstrap);
            return;
        }

        AppState appState = GameRoot.Instance.AppState;

        if (welcomeText != null)
        {
            welcomeText.text = $"Welcome, {appState.Student.name}";
        }

        if (gradeSectionText != null)
        {
            gradeSectionText.text = appState.GradeSectionDisplayName;
        }

        if (subjectText != null)
        {
            subjectText.text = appState.Subject.name;
        }
    }

    private void OnLogoutClicked()
    {
        StartCoroutine(LogoutRoutine());
    }

    private System.Collections.IEnumerator LogoutRoutine()
    {
        if (GameRoot.Instance == null || GameRoot.Instance.AuthService == null)
        {
            yield break;
        }

        yield return GameRoot.Instance.AuthService.Logout(
            result =>
            {
                GameRoot.Instance.AppState.ClearSession();
                GameRoot.Instance.SceneLoader.LoadScene(SceneNames.Login);
            }
        );
    }
}
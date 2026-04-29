using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoginController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private TMP_InputField lrnInput;
    [SerializeField] private TMP_InputField pinInput;

    [Header("UI")]
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text loadingText;

    private bool isLoggingIn;

    private void Start()
    {
        HideError();
        SetLoading(false);

        if (loginButton != null)
        {
            loginButton.onClick.AddListener(OnLoginClicked);
        }
    }

    private void OnDestroy()
    {
        if (loginButton != null)
        {
            loginButton.onClick.RemoveListener(OnLoginClicked);
        }
    }

    private void OnLoginClicked()
    {
        if (isLoggingIn)
        {
            return;
        }

        string lrn = lrnInput.text.Trim();
        string pin = pinInput.text.Trim();

        if (!ValidateInput(lrn, pin))
        {
            return;
        }

        StartCoroutine(LoginRoutine(lrn, pin));
    }

    private System.Collections.IEnumerator LoginRoutine(string lrn, string pin)
    {
        isLoggingIn = true;
        SetLoading(true);
        HideError();

        if (GameRoot.Instance == null || GameRoot.Instance.AuthService == null)
        {
            ShowError("Login system is not ready.");
            SetLoading(false);
            isLoggingIn = false;
            yield break;
        }

        yield return GameRoot.Instance.AuthService.Login(
            lrn,
            pin,
            result =>
            {
                if (result.IsSuccess)
                {
                    GameRoot.Instance.SceneLoader.LoadScene(SceneNames.MainMenu);
                    return;
                }

                ShowError(result.ErrorMessage);
            }
        );

        SetLoading(false);
        isLoggingIn = false;
    }

    private bool ValidateInput(string lrn, string pin)
    {
        if (string.IsNullOrWhiteSpace(lrn))
        {
            ShowError("Please enter your LRN.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(pin))
        {
            ShowError("Please enter your PIN.");
            return false;
        }

        if (lrn.Length < 6)
        {
            ShowError("Please enter a valid LRN.");
            return false;
        }

        return true;
    }

    private void ShowError(string message)
    {
        if (errorText == null)
        {
            return;
        }

        errorText.text = string.IsNullOrWhiteSpace(message)
            ? "Something went wrong. Please try again."
            : message;

        errorText.gameObject.SetActive(true);
    }

    private void HideError()
    {
        if (errorText == null)
        {
            return;
        }

        errorText.text = string.Empty;
        errorText.gameObject.SetActive(false);
    }

    private void SetLoading(bool value)
    {
        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(value);
            loadingText.text = value ? "Logging in..." : string.Empty;
        }

        if (loginButton != null)
        {
            loginButton.interactable = !value;
        }

        if (lrnInput != null)
        {
            lrnInput.interactable = !value;
        }

        if (pinInput != null)
        {
            pinInput.interactable = !value;
        }
    }
}
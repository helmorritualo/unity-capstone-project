using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text loadingText;

    public void Show(string message = "Loading...")
    {
        gameObject.SetActive(true);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        if (loadingText != null)
        {
            loadingText.text = message;
        }

        SetProgress(0f);
    }

    public void Hide()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        gameObject.SetActive(false);
    }

    public void SetProgress(float value)
    {
        if (progressBar != null)
        {
            progressBar.value = Mathf.Clamp01(value);
        }
    }
}
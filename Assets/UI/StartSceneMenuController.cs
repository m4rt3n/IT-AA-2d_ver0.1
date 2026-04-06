using System.Collections;
using UnityEngine;

public class StartSceneMenuController : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject backgroundDim;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float panelStartScale = 0.85f;
    [SerializeField] private float panelEndScale = 1f;
    [SerializeField] private float dimTargetAlpha = 0.65f;

    private CanvasGroup loginCanvasGroup;
    private CanvasGroup backgroundCanvasGroup;

    private Coroutine currentAnimation;
    private bool isOpen = false;
    private bool isAnimating = false;

    private void Awake()
    {
        if (loginPanel != null)
        {
            loginCanvasGroup = loginPanel.GetComponent<CanvasGroup>();

            if (loginCanvasGroup == null)
            {
                loginCanvasGroup = loginPanel.AddComponent<CanvasGroup>();
            }
        }

        if (backgroundDim != null)
        {
            backgroundCanvasGroup = backgroundDim.GetComponent<CanvasGroup>();

            if (backgroundCanvasGroup == null)
            {
                backgroundCanvasGroup = backgroundDim.AddComponent<CanvasGroup>();
            }
        }
    }

    private void Start()
    {
        if (loginPanel == null)
        {
            Debug.LogError("StartMenuController: loginPanel ist nicht zugewiesen.");
            return;
        }

        if (backgroundDim == null)
        {
            Debug.LogError("StartMenuController: backgroundDim ist nicht zugewiesen.");
            return;
        }

        PrepareClosedStateInstant();
    }

    public void OpenMenu()
    {
        Debug.Log("OpenMenu called");

        if (isOpen || isAnimating)
            return;

        isOpen = true;

        if (playerController != null)
        {
            playerController.SetPlayerControlEnabled(false);
        }

        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation = StartCoroutine(OpenMenuRoutine());
    }

    public void CloseMenu()
    {
        Debug.Log("CloseMenu called");

        if (!isOpen || isAnimating)
            return;

        isOpen = false;

        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation = StartCoroutine(CloseMenuRoutine());
    }

    public void OnClickSignIn()
    {
        Debug.Log("Sign In clicked");
    }

    public void OnClickLogin()
    {
        Debug.Log("Login clicked");
    }

    public void OnClickGuest()
    {
        Debug.Log("Guest clicked");
        CloseMenu();
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    private IEnumerator OpenMenuRoutine()
    {
        isAnimating = true;

        backgroundDim.SetActive(true);
        loginPanel.SetActive(true);

        backgroundCanvasGroup.alpha = 0f;
        backgroundCanvasGroup.blocksRaycasts = false;
        backgroundCanvasGroup.interactable = false;

        loginCanvasGroup.alpha = 0f;
        loginCanvasGroup.blocksRaycasts = false;
        loginCanvasGroup.interactable = false;

        loginPanel.transform.localScale = Vector3.one * panelStartScale;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float eased = EaseOutBack(t);

            backgroundCanvasGroup.alpha = Mathf.Lerp(0f, dimTargetAlpha, t);
            loginCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            float scale = Mathf.Lerp(panelStartScale, panelEndScale, eased);
            loginPanel.transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        backgroundCanvasGroup.alpha = dimTargetAlpha;
        loginCanvasGroup.alpha = 1f;
        loginPanel.transform.localScale = Vector3.one * panelEndScale;

        backgroundCanvasGroup.blocksRaycasts = true;
        backgroundCanvasGroup.interactable = true;

        loginCanvasGroup.blocksRaycasts = true;
        loginCanvasGroup.interactable = true;

        isAnimating = false;
        currentAnimation = null;
    }

    private IEnumerator CloseMenuRoutine()
    {
        isAnimating = true;

        backgroundCanvasGroup.blocksRaycasts = false;
        backgroundCanvasGroup.interactable = false;

        loginCanvasGroup.blocksRaycasts = false;
        loginCanvasGroup.interactable = false;

        float elapsed = 0f;
        float closeStartScale = loginPanel.transform.localScale.x;
        float closeEndScale = 0.92f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float eased = EaseInCubic(t);

            backgroundCanvasGroup.alpha = Mathf.Lerp(dimTargetAlpha, 0f, t);
            loginCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            float scale = Mathf.Lerp(closeStartScale, closeEndScale, eased);
            loginPanel.transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        PrepareClosedStateInstant();

        if (playerController != null)
        {
            playerController.SetPlayerControlEnabled(true);
        }

        isAnimating = false;
        currentAnimation = null;
    }

    private void PrepareClosedStateInstant()
    {
        if (backgroundCanvasGroup != null)
        {
            backgroundCanvasGroup.alpha = 0f;
            backgroundCanvasGroup.blocksRaycasts = false;
            backgroundCanvasGroup.interactable = false;
        }

        if (loginCanvasGroup != null)
        {
            loginCanvasGroup.alpha = 0f;
            loginCanvasGroup.blocksRaycasts = false;
            loginCanvasGroup.interactable = false;
        }

        if (loginPanel != null)
        {
            loginPanel.transform.localScale = Vector3.one * panelEndScale;
            loginPanel.SetActive(false);
        }

        if (backgroundDim != null)
        {
            backgroundDim.SetActive(false);
        }
    }

    private float EaseInCubic(float t)
    {
        return t * t * t;
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
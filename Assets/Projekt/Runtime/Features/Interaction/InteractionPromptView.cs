/*
 * Datei: InteractionPromptView.cs
 * Zweck: Zeigt einen einfachen Interaktionshinweis im UI an.
 * Verantwortung: Blendet einen Root ein/aus und setzt den Prompt-Text des aktuellen Interaktionsziels.
 * Abhaengigkeiten: TMPro, Unity UI GameObject.
 * Verwendung: Wird vom InteractionController referenziert, um z. B. "E druecken" anzuzeigen.
 */

using TMPro;
using UnityEngine;

namespace ITAA.Features.Interaction
{
    public class InteractionPromptView : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text promptText;

        [Header("Text")]
        [SerializeField] private string fallbackPrompt = "E druecken";

        #endregion

        #region Unity

        private void Awake()
        {
            ResolveReferences();
            Hide();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ResolveReferences();
        }
#endif

        #endregion

        #region Public API

        public void Show(string prompt)
        {
            ResolveReferences();

            if (promptText != null)
            {
                promptText.text = string.IsNullOrWhiteSpace(prompt)
                    ? fallbackPrompt
                    : prompt;
            }

            if (root != null)
            {
                root.SetActive(true);
            }
        }

        public void Hide()
        {
            ResolveReferences();

            if (root != null)
            {
                root.SetActive(false);
            }
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (root == null)
            {
                root = gameObject;
            }

            if (promptText == null)
            {
                promptText = GetComponentInChildren<TMP_Text>(true);
            }
        }

        #endregion
    }
}

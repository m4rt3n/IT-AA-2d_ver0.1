/*
 * Datei: KnowledgeArticleListItemUI.cs
 * Zweck: Stellt einen einzelnen Lexikonartikel in der Artikelliste dar.
 * Verantwortung: Zeigt Titel, Thema und Kurzbeschreibung und meldet Auswahl per Callback.
 * Abhaengigkeiten: KnowledgeArticle, TextMeshPro, Unity UI.
 * Verwendung: Wird vom KnowledgeBasePanel fuer Suchergebnisse und Demo-Artikel verwendet.
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.Features.KnowledgeBase
{
    [DisallowMultipleComponent]
    public class KnowledgeArticleListItemUI : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Button selectButton;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text topicText;
        [SerializeField] private TMP_Text shortDescriptionText;

        [Header("Debug")]
        [SerializeField] private bool autoResolveReferences = true;
        [SerializeField] private bool enableDebugLogs;

        #endregion

        private KnowledgeArticle article;
        private Action<KnowledgeArticle> selectedCallback;

        #region Unity

        private void Awake()
        {
            ResolveReferences();
            WireButton();
        }

        private void OnDestroy()
        {
            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(HandleSelected);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (autoResolveReferences)
            {
                ResolveReferences();
            }
        }
#endif

        #endregion

        #region Public API

        public void Setup(KnowledgeArticle sourceArticle, Action<KnowledgeArticle> onSelected)
        {
            ResolveReferences();
            WireButton();

            article = sourceArticle;
            selectedCallback = onSelected;

            if (article == null)
            {
                SetText(titleText, "Unbekannter Artikel");
                SetText(topicText, KnowledgeTopic.Other.ToString());
                SetText(shortDescriptionText, string.Empty);
                return;
            }

            SetText(titleText, article.Title);
            SetText(topicText, article.Topic.ToString());
            SetText(shortDescriptionText, article.ShortDescription);
        }

        #endregion

        #region Private

        private void ResolveReferences()
        {
            if (!autoResolveReferences)
            {
                return;
            }

            if (selectButton == null)
            {
                selectButton = GetComponent<Button>();

                if (selectButton == null)
                {
                    selectButton = GetComponentInChildren<Button>(true);
                }
            }

            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);

            if (titleText == null)
            {
                titleText = FindTextByName(texts, "TitleText", "ArticleTitleText", "Label");
            }

            if (topicText == null)
            {
                topicText = FindTextByName(texts, "TopicText", "ArticleTopicText");
            }

            if (shortDescriptionText == null)
            {
                shortDescriptionText = FindTextByName(texts, "ShortDescriptionText", "DescriptionText", "SummaryText");
            }
        }

        private void WireButton()
        {
            if (selectButton == null)
            {
                return;
            }

            selectButton.onClick.RemoveListener(HandleSelected);
            selectButton.onClick.AddListener(HandleSelected);
        }

        private void HandleSelected()
        {
            if (article == null)
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[{nameof(KnowledgeArticleListItemUI)}] Kein Artikel gesetzt.", this);
                }

                return;
            }

            selectedCallback?.Invoke(article);
        }

        private static TMP_Text FindTextByName(TMP_Text[] texts, params string[] candidateNames)
        {
            if (texts == null)
            {
                return null;
            }

            for (int i = 0; i < candidateNames.Length; i++)
            {
                string candidate = candidateNames[i];

                for (int j = 0; j < texts.Length; j++)
                {
                    TMP_Text text = texts[j];

                    if (text != null && text.name.Equals(candidate, StringComparison.OrdinalIgnoreCase))
                    {
                        return text;
                    }
                }
            }

            return null;
        }

        private static void SetText(TMP_Text target, string value)
        {
            if (target != null)
            {
                target.text = value ?? string.Empty;
            }
        }

        #endregion
    }
}

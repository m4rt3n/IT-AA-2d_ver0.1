/*
 * Datei: KnowledgeBasePanel.cs
 * Zweck: Zeigt das interne IT-Lexikon als einfaches UI-Panel an.
 * Verantwortung: Baut bei Bedarf UI, zeigt Artikel-Liste, oeffnet Artikel, sucht nach Titel und meldet Close-Ereignisse.
 * Abhaengigkeiten: BasePanel, KnowledgeBaseRepository, KnowledgeArticleListItemUI, TextMeshPro, Unity UI.
 * Verwendung: Kann in ein Canvas gelegt oder als Runtime-Panel genutzt werden; Quiz-/Szenario-Integration bleibt vorbereitet.
 */

using System.Collections.Generic;
using System;
using ITAA.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ITAA.Features.KnowledgeBase
{
    public class KnowledgeBasePanel : BasePanel
    {
        #region Inspector

        [Header("Optional UI References")]
        [SerializeField] private RectTransform panelRoot;
        [SerializeField] private RectTransform listContentRoot;
        [SerializeField] private KnowledgeArticleListItemUI listItemPrefab;
        [SerializeField] private TMP_InputField searchInput;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text topicText;
        [SerializeField] private TMP_Text shortDescriptionText;
        [SerializeField] private TMP_Text contentText;
        [SerializeField] private TMP_Text emptyStateText;
        [SerializeField] private Button closeButton;

        [Header("Generated UI")]
        [SerializeField] private bool createMissingUi = true;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        private readonly List<KnowledgeArticleListItemUI> spawnedItems = new();
        private KnowledgeBaseRepository repository;
        private KnowledgeArticle currentArticle;
        private Action closedCallback;

        #region Unity

        private void Awake()
        {
            repository = new KnowledgeBaseRepository();

            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            WireUi();
            RefreshArticleList();
            OpenFirstArticleIfAvailable();
        }

        private void OnDestroy()
        {
            UnwireUi();
        }

        #endregion

        #region Public API

        public void RefreshArticleList()
        {
            EnsureRepository();

            string searchTerm = searchInput != null ? searchInput.text : string.Empty;
            IReadOnlyList<KnowledgeArticle> results = repository.SearchByTitle(searchTerm);

            ClearList();

            bool hasResults = results != null && results.Count > 0;
            SetText(emptyStateText, hasResults ? string.Empty : "Kein Artikel gefunden.");

            if (!hasResults)
            {
                return;
            }

            for (int i = 0; i < results.Count; i++)
            {
                CreateListItem(results[i]);
            }
        }

        public void OpenArticle(KnowledgeArticle article)
        {
            if (article == null)
            {
                Debug.LogWarning($"[{nameof(KnowledgeBasePanel)}] Artikel fehlt.", this);
                return;
            }

            currentArticle = article;

            SetText(titleText, article.Title);
            SetText(topicText, article.Topic.ToString());
            SetText(shortDescriptionText, article.ShortDescription);
            SetText(contentText, article.Content);

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(KnowledgeBasePanel)}] Artikel geoeffnet: {article.Title}", this);
            }
        }

        public void OpenArticleById(string articleId)
        {
            EnsureRepository();

            KnowledgeArticle article = repository.GetById(articleId);

            if (article == null)
            {
                Debug.LogWarning($"[{nameof(KnowledgeBasePanel)}] Artikel nicht gefunden: {articleId}", this);
                return;
            }

            OpenArticle(article);
        }

        public KnowledgeArticle GetCurrentArticle()
        {
            return currentArticle;
        }

        public void SetClosedCallback(Action callback)
        {
            closedCallback = callback;
        }

        public override void Open()
        {
            if (createMissingUi)
            {
                EnsureGeneratedUi();
            }

            EnsureRepository();
            WireUi();
            RefreshArticleList();

            if (currentArticle == null)
            {
                OpenFirstArticleIfAvailable();
            }

            base.Open();
        }

        public override void Close()
        {
            if (!IsOpen)
            {
                return;
            }

            base.Close();
            closedCallback?.Invoke();
        }

        #endregion

        #region Private

        private void EnsureRepository()
        {
            repository ??= new KnowledgeBaseRepository();
        }

        private void WireUi()
        {
            if (searchInput != null)
            {
                searchInput.onValueChanged.RemoveListener(HandleSearchChanged);
                searchInput.onValueChanged.AddListener(HandleSearchChanged);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Close);
                closeButton.onClick.AddListener(Close);
            }
        }

        private void UnwireUi()
        {
            if (searchInput != null)
            {
                searchInput.onValueChanged.RemoveListener(HandleSearchChanged);
            }

            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Close);
            }
        }

        private void HandleSearchChanged(string searchTerm)
        {
            RefreshArticleList();

            IReadOnlyList<KnowledgeArticle> results = repository.SearchByTitle(searchTerm);

            if (results == null || results.Count == 0)
            {
                ClearArticleDetail();
                return;
            }

            OpenArticle(results[0]);
        }

        private void OpenFirstArticleIfAvailable()
        {
            EnsureRepository();

            IReadOnlyList<KnowledgeArticle> articles = repository.GetAllArticles();

            if (articles == null || articles.Count == 0)
            {
                ClearArticleDetail();
                return;
            }

            OpenArticle(articles[0]);
        }

        private void CreateListItem(KnowledgeArticle article)
        {
            if (listContentRoot == null)
            {
                return;
            }

            KnowledgeArticleListItemUI item = null;

            if (listItemPrefab != null)
            {
                item = Instantiate(listItemPrefab, listContentRoot);
            }
            else
            {
                item = CreateRuntimeListItem(listContentRoot);
            }

            item.Setup(article, OpenArticle);
            spawnedItems.Add(item);
        }

        private void ClearList()
        {
            for (int i = spawnedItems.Count - 1; i >= 0; i--)
            {
                KnowledgeArticleListItemUI item = spawnedItems[i];

                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }

            spawnedItems.Clear();
        }

        private void ClearArticleDetail()
        {
            currentArticle = null;
            SetText(titleText, "Kein Artikel");
            SetText(topicText, string.Empty);
            SetText(shortDescriptionText, string.Empty);
            SetText(contentText, "Bitte waehle einen Artikel aus.");
        }

        private void EnsureGeneratedUi()
        {
            if (panelRoot != null &&
                listContentRoot != null &&
                searchInput != null &&
                titleText != null &&
                topicText != null &&
                shortDescriptionText != null &&
                contentText != null &&
                closeButton != null)
            {
                return;
            }

            RectTransform parent = transform as RectTransform;

            if (parent == null)
            {
                return;
            }

            panelRoot = CreateRect("KnowledgeBasePanelRoot", parent);
            Stretch(panelRoot);

            Image overlay = panelRoot.gameObject.AddComponent<Image>();
            overlay.color = new Color(0f, 0f, 0f, 0.58f);

            RectTransform card = CreateRect("KnowledgeBaseCard", panelRoot);
            card.anchorMin = new Vector2(0.5f, 0.5f);
            card.anchorMax = new Vector2(0.5f, 0.5f);
            card.pivot = new Vector2(0.5f, 0.5f);
            card.anchoredPosition = Vector2.zero;
            card.sizeDelta = new Vector2(980f, 620f);

            Image cardImage = card.gameObject.AddComponent<Image>();
            cardImage.color = new Color(0.1f, 0.12f, 0.16f, 0.98f);

            TMP_Text headingText = CreateText("HeadingText", card, 34f, FontStyles.Bold);
            headingText.text = "Knowledge Base";
            SetRect(headingText.rectTransform, 0f, 1f, 1f, 1f, 0f, -30f, -48f, 42f);

            searchInput = CreateSearchInput(card);
            SetRect(searchInput.GetComponent<RectTransform>(), 0f, 1f, 0f, 1f, 200f, -88f, 330f, 42f);

            RectTransform listViewport = CreateRect("ArticleListViewport", card);
            SetRect(listViewport, 0f, 0f, 0f, 1f, 200f, -344f, 330f, 440f);

            Image listImage = listViewport.gameObject.AddComponent<Image>();
            listImage.color = new Color(0.15f, 0.18f, 0.23f, 1f);

            Mask listMask = listViewport.gameObject.AddComponent<Mask>();
            listMask.showMaskGraphic = true;

            listContentRoot = CreateRect("ArticleListContent", listViewport);
            listContentRoot.anchorMin = new Vector2(0f, 1f);
            listContentRoot.anchorMax = new Vector2(1f, 1f);
            listContentRoot.pivot = new Vector2(0.5f, 1f);
            listContentRoot.anchoredPosition = Vector2.zero;
            listContentRoot.sizeDelta = new Vector2(0f, 0f);

            VerticalLayoutGroup listLayout = listContentRoot.gameObject.AddComponent<VerticalLayoutGroup>();
            listLayout.padding = new RectOffset(8, 8, 8, 8);
            listLayout.spacing = 8f;
            listLayout.childAlignment = TextAnchor.UpperCenter;
            listLayout.childForceExpandWidth = true;
            listLayout.childForceExpandHeight = false;

            ContentSizeFitter listFitter = listContentRoot.gameObject.AddComponent<ContentSizeFitter>();
            listFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ScrollRect scrollRect = listViewport.gameObject.AddComponent<ScrollRect>();
            scrollRect.content = listContentRoot;
            scrollRect.viewport = listViewport;
            scrollRect.horizontal = false;

            titleText = CreateText("TitleText", card, 32f, FontStyles.Bold);
            titleText.alignment = TextAlignmentOptions.Left;
            SetRect(titleText.rectTransform, 0f, 1f, 1f, 1f, 250f, -92f, -430f, 44f);

            topicText = CreateText("TopicText", card, 20f, FontStyles.Bold);
            topicText.color = new Color(0.48f, 0.74f, 1f, 1f);
            topicText.alignment = TextAlignmentOptions.Left;
            SetRect(topicText.rectTransform, 0f, 1f, 1f, 1f, 250f, -132f, -430f, 30f);

            shortDescriptionText = CreateText("ShortDescriptionText", card, 21f, FontStyles.Normal);
            shortDescriptionText.textWrappingMode = TextWrappingModes.Normal;
            shortDescriptionText.alignment = TextAlignmentOptions.Left;
            SetRect(shortDescriptionText.rectTransform, 0f, 1f, 1f, 1f, 250f, -178f, -430f, 64f);

            contentText = CreateText("ContentText", card, 23f, FontStyles.Normal);
            contentText.textWrappingMode = TextWrappingModes.Normal;
            contentText.alignment = TextAlignmentOptions.TopLeft;
            SetRect(contentText.rectTransform, 0f, 0f, 1f, 1f, 250f, -342f, -430f, 250f);

            emptyStateText = CreateText("EmptyStateText", card, 18f, FontStyles.Italic);
            emptyStateText.color = new Color(1f, 0.76f, 0.4f, 1f);
            SetRect(emptyStateText.rectTransform, 0f, 0f, 0f, 0f, 200f, 38f, 330f, 28f);

            RectTransform closeRect = CreateButton("CloseButton", card, out closeButton, out TMP_Text closeLabel);
            closeLabel.text = "Schliessen";
            SetRect(closeRect, 1f, 0f, 1f, 0f, -106f, 34f, 150f, 44f);
        }

        private static KnowledgeArticleListItemUI CreateRuntimeListItem(Transform parent)
        {
            RectTransform rect = CreateRect("KnowledgeArticleListItem", parent);
            rect.sizeDelta = new Vector2(0f, 92f);

            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.22f, 0.27f, 0.35f, 1f);

            Button button = rect.gameObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.highlightedColor = new Color(0.29f, 0.36f, 0.47f, 1f);
            colors.pressedColor = new Color(0.15f, 0.2f, 0.28f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            LayoutElement layoutElement = rect.gameObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 92f;

            TMP_Text title = CreateText("TitleText", rect, 20f, FontStyles.Bold);
            title.alignment = TextAlignmentOptions.Left;
            SetRect(title.rectTransform, 0f, 1f, 1f, 1f, 0f, -18f, -22f, 24f);

            TMP_Text topic = CreateText("TopicText", rect, 15f, FontStyles.Bold);
            topic.color = new Color(0.55f, 0.78f, 1f, 1f);
            topic.alignment = TextAlignmentOptions.Left;
            SetRect(topic.rectTransform, 0f, 1f, 1f, 1f, 0f, -42f, -22f, 20f);

            TMP_Text description = CreateText("ShortDescriptionText", rect, 15f, FontStyles.Normal);
            description.textWrappingMode = TextWrappingModes.Normal;
            description.alignment = TextAlignmentOptions.Left;
            SetRect(description.rectTransform, 0f, 0f, 1f, 0f, 0f, 20f, -22f, 34f);

            return rect.gameObject.AddComponent<KnowledgeArticleListItemUI>();
        }

        private static TMP_InputField CreateSearchInput(Transform parent)
        {
            RectTransform rect = CreateRect("SearchInput", parent);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.92f, 0.94f, 0.98f, 1f);

            TMP_InputField input = rect.gameObject.AddComponent<TMP_InputField>();

            TMP_Text text = CreateText("Text", rect, 20f, FontStyles.Normal);
            text.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            text.alignment = TextAlignmentOptions.Left;
            SetRect(text.rectTransform, 0f, 0f, 1f, 1f, 10f, 0f, -20f, 0f);

            TMP_Text placeholder = CreateText("Placeholder", rect, 20f, FontStyles.Italic);
            placeholder.color = new Color(0.4f, 0.45f, 0.52f, 1f);
            placeholder.text = string.Empty;
            placeholder.alignment = TextAlignmentOptions.Left;
            SetRect(placeholder.rectTransform, 0f, 0f, 1f, 1f, 10f, 0f, -20f, 0f);

            input.textComponent = text;
            input.placeholder = placeholder;

            return input;
        }

        private static RectTransform CreateButton(string objectName, Transform parent, out Button button, out TMP_Text label)
        {
            RectTransform rect = CreateRect(objectName, parent);
            Image image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.86f, 0.89f, 0.94f, 1f);

            button = rect.gameObject.AddComponent<Button>();
            label = CreateText("Label", rect, 20f, FontStyles.Bold);
            label.color = new Color(0.08f, 0.1f, 0.14f, 1f);
            label.alignment = TextAlignmentOptions.Center;
            Stretch(label.rectTransform);

            return rect;
        }

        private static RectTransform CreateRect(string objectName, Transform parent)
        {
            GameObject instance = new GameObject(objectName, typeof(RectTransform));
            instance.transform.SetParent(parent, false);
            return instance.GetComponent<RectTransform>();
        }

        private static TMP_Text CreateText(string objectName, Transform parent, float fontSize, FontStyles fontStyle)
        {
            RectTransform rect = CreateRect(objectName, parent);
            TextMeshProUGUI text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.color = Color.white;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false;
            return text;
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private static void SetRect(RectTransform rectTransform, float minX, float minY, float maxX, float maxY, float x, float y, float width, float height)
        {
            rectTransform.anchorMin = new Vector2(minX, minY);
            rectTransform.anchorMax = new Vector2(maxX, maxY);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(x, y);
            rectTransform.sizeDelta = new Vector2(width, height);
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

/*
 * Datei: BerndNameTag.cs
 * Zweck: Zeigt Bernds Namen als Welt-UI ueber seinem Kopf an.
 * Verantwortung: Erstellt bei Bedarf ein einfaches NameTag und richtet es zur Kamera aus.
 * Abhaengigkeiten: TextMeshPro, Unity UI Canvas.
 * Verwendung: Wird auf Bernds GameObject gesetzt und benoetigt keine manuelle UI-Zuweisung.
 */

using TMPro;
using UnityEngine;

namespace ITAA.NPC.Bernd
{
    [DisallowMultipleComponent]
    public class BerndNameTag : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private string displayName = "Bernd";
        [SerializeField] private Vector3 worldOffset = new(0f, 1.6f, 0f);

        private Transform labelRoot;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
            EnsureNameTag();
            SetName(displayName);
        }

        private void LateUpdate()
        {
            if (labelRoot == null)
            {
                return;
            }

            labelRoot.position = transform.position + worldOffset;

            if (mainCamera != null)
            {
                labelRoot.rotation = mainCamera.transform.rotation;
            }
        }

        public void SetName(string newName)
        {
            displayName = string.IsNullOrWhiteSpace(newName) ? "Bernd" : newName;

            if (nameText != null)
            {
                nameText.text = displayName;
            }
        }

        private void EnsureNameTag()
        {
            if (nameText != null)
            {
                labelRoot = nameText.transform;
                return;
            }

            GameObject canvasObject = new GameObject("BerndNameTagCanvas", typeof(RectTransform));
            canvasObject.transform.SetParent(transform, false);
            canvasObject.transform.localScale = Vector3.one * 0.01f;

            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 120;

            RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(180f, 48f);

            GameObject textObject = new GameObject("NameText", typeof(RectTransform));
            textObject.transform.SetParent(canvasObject.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            TextMeshProUGUI generatedText = textObject.AddComponent<TextMeshProUGUI>();
            generatedText.alignment = TextAlignmentOptions.Center;
            generatedText.fontSize = 32f;
            generatedText.fontStyle = FontStyles.Bold;
            generatedText.color = Color.white;
            generatedText.raycastTarget = false;

            nameText = generatedText;
            labelRoot = canvasObject.transform;
        }
    }
}

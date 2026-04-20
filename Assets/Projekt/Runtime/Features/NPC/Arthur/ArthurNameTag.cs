/*
 * Datei: ArthurNameTag.cs
 * Zweck: Zeigt Arthurs Nametag nur bei Nähe zum Player an.
 * Verantwortung: Folgt Arthur, richtet sich zur Kamera aus und blendet den Namen bei Distanz ein/aus.
 * Abhängigkeiten: TMP_Text, Transform, Camera.
 * Verwendet von: Arthur NameTagCanvas.
 */

using TMPro;
using UnityEngine;

namespace ITAA.NPC.Arthur
{
    public class ArthurNameTag : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private Transform player;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Canvas canvas;

        [Header("Display")]
        [SerializeField] private string displayName = "Arthur";
        [SerializeField] private Vector3 worldOffset = new(0f, 1.5f, 0f);
        [SerializeField] private float visibleDistance = 2.5f;

        private Camera mainCamera;

        private void Awake()
        {
            if (nameText == null)
            {
                nameText = GetComponentInChildren<TMP_Text>();
            }

            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
            }

            if (nameText != null)
            {
                nameText.text = displayName;
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;

            if (target == null)
            {
                Debug.LogWarning($"[{nameof(ArthurNameTag)}] Target ist nicht gesetzt.", this);
            }

            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            SetVisible(false);
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            transform.position = target.position + worldOffset;

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (mainCamera != null)
            {
                transform.forward = mainCamera.transform.forward;
            }

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            if (player == null)
            {
                SetVisible(false);
                return;
            }

            float distance = Vector3.Distance(player.position, target.position);
            SetVisible(distance <= visibleDistance);
        }

        private void SetVisible(bool isVisible)
        {
            if (canvas != null)
            {
                canvas.enabled = isVisible;
            }
            else
            {
                gameObject.SetActive(isVisible);
            }
        }
    }
}
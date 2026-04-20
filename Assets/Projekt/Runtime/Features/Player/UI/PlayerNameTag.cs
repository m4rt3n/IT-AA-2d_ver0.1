/*
 * Datei: PlayerNameTag.cs
 * Zweck: Positioniert ein Namensschild über einer Ziel-Figur in der Welt.
 * Verantwortung: Folgt einem Transform-Ziel und zeigt den Spielernamen aus der PlayerSession an.
 * Abhängigkeiten: PlayerSession, Transform, TMP_Text, Camera.
 * Verwendet von: Player mit World-Space-Namensanzeige.
 */

using ITAA.Player.Session;
using TMPro;
using UnityEngine;

namespace ITAA.Player.UI
{
    public class PlayerNameTag : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private TMP_Text nameText;

        [Header("Position")]
        [SerializeField] private Vector3 worldOffset = new(0f, 1.5f, 0f);

        [Header("Fallback")]
        [SerializeField] private string fallbackName = "Spieler";

        #endregion

        #region Private Fields

        private Camera mainCamera;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (nameText == null)
            {
                nameText = GetComponentInChildren<TMP_Text>();
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
            UpdateName();
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
        }

        #endregion

        #region Public Methods

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void SetName(string playerName)
        {
            if (nameText != null)
            {
                nameText.text = string.IsNullOrWhiteSpace(playerName) ? fallbackName : playerName;
            }
        }

        public void UpdateName()
        {
            if (nameText == null)
            {
                return;
            }

            string playerName = fallbackName;

            if (PlayerSession.Instance != null &&
                !string.IsNullOrWhiteSpace(PlayerSession.Instance.Username))
            {
                playerName = PlayerSession.Instance.Username;
            }

            nameText.text = playerName;
        }

        #endregion
    }
}
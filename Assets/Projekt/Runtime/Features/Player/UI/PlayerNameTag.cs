/*
 * Datei: PlayerNameTag.cs
 * Zweck: Positioniert ein Namensschild über einer Ziel-Figur in der Welt.
 * Verantwortung: Folgt einem Transform-Ziel und aktualisiert optional den sichtbaren Namen.
 * Abhängigkeiten: Transform, TMP_Text, Camera.
 * Verwendet von: Player oder andere Figuren mit World-Space-Namensanzeige.
 */

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
        [SerializeField] private Vector3 worldOffset = new(0f, 1.25f, 0f);

        #endregion

        #region Private Fields

        private Camera mainCamera;

        #endregion

        #region Unity Methods

        private void Start()
        {
            mainCamera = Camera.main;

            if (nameText == null)
            {
                nameText = GetComponentInChildren<TMP_Text>();
            }
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            transform.position = target.position + worldOffset;

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
                nameText.text = playerName;
            }
        }

        #endregion
    }
}
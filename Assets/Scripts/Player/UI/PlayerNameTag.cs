using TMPro;
using UnityEngine;

namespace ITAA.Player.UI
{
    public class PlayerNameTag : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 worldOffset = new(0f, 1.25f, 0f);
        [SerializeField] private TMP_Text nameText;

        private Camera mainCamera;

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
    }
}
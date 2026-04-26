/*
 * Datei: SimpleCameraFollow2D.cs
 * Zweck: Bietet eine kleine 2D-Kamera-Verfolgung fuer die GameScene-Testwelt.
 * Verantwortung: Folgt einem Ziel-Transform mit optionaler Glaettung und Offset.
 * Abhaengigkeiten: Unity Transform, Camera.
 * Verwendung: Wird auf die MainCamera gesetzt, falls kein anderes Kamerasystem vorhanden ist.
 */

using UnityEngine;

namespace ITAA.Gameplay.World
{
    [DisallowMultipleComponent]
    public sealed class SimpleCameraFollow2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;
        [SerializeField] private string targetTag = "Player";

        [Header("Follow")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
        [SerializeField] private bool smoothFollow = true;
        [SerializeField] private float smoothTime = 0.12f;

        private Vector3 velocity;

        private void LateUpdate()
        {
            ResolveTarget();

            if (target == null)
            {
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            transform.position = smoothFollow
                ? Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, Mathf.Max(0.01f, smoothTime))
                : desiredPosition;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void ResolveTarget()
        {
            if (target != null)
            {
                return;
            }

            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
            target = targetObject != null ? targetObject.transform : null;
        }
    }
}

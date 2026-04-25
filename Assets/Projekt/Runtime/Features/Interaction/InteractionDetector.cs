/*
 * Datei: InteractionDetector.cs
 * Zweck: Erkennt interagierbare Objekte in der Naehe des Spielers.
 * Verantwortung: Verwaltet Kandidaten aus Trigger-Kontakten und stellt das naechste verfuegbare IInteractable bereit.
 * Abhaengigkeiten: IInteractable, Collider2D, Unity Physik.
 * Verwendung: Wird auf dem Player oder einem Player-Child mit Trigger-Collider2D eingesetzt.
 */

using System.Collections.Generic;
using UnityEngine;

namespace ITAA.Features.Interaction
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class InteractionDetector : MonoBehaviour
    {
        #region Inspector

        [Header("Interactor")]
        [SerializeField] private Transform interactorRoot;

        [Header("Filtering")]
        [SerializeField] private LayerMask interactableLayers = ~0;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs;

        #endregion

        #region State

        private readonly List<Candidate> candidates = new List<Candidate>();

        public event global::System.Action<IInteractable> CurrentInteractableChanged;

        public IInteractable CurrentInteractable { get; private set; }

        #endregion

        #region Unity

        private void Reset()
        {
            Collider2D triggerCollider = GetComponent<Collider2D>();

            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = true;
            }
        }

        private void Awake()
        {
            if (interactorRoot == null)
            {
                interactorRoot = transform;
            }
        }

        private void OnDisable()
        {
            candidates.Clear();
            SetCurrentInteractable(null);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || !IsLayerAllowed(other.gameObject.layer))
            {
                return;
            }

            if (!TryFindInteractable(other, out IInteractable interactable, out Transform targetTransform))
            {
                return;
            }

            AddCandidate(other, interactable, targetTransform);
            RefreshCurrentInteractable();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null)
            {
                return;
            }

            RemoveCandidatesForCollider(other);
            RefreshCurrentInteractable();
        }

        #endregion

        #region Public API

        public void RefreshCurrentInteractable()
        {
            RemoveInvalidCandidates();

            IInteractable nearest = null;
            float nearestDistanceSqr = float.MaxValue;
            Vector3 origin = interactorRoot != null ? interactorRoot.position : transform.position;

            for (int i = 0; i < candidates.Count; i++)
            {
                Candidate candidate = candidates[i];

                if (candidate.Interactable == null || !candidate.Interactable.CanInteract(interactorRoot))
                {
                    continue;
                }

                Vector3 targetPosition = candidate.TargetTransform != null
                    ? candidate.TargetTransform.position
                    : origin;

                float distanceSqr = (targetPosition - origin).sqrMagnitude;

                if (distanceSqr < nearestDistanceSqr)
                {
                    nearestDistanceSqr = distanceSqr;
                    nearest = candidate.Interactable;
                }
            }

            SetCurrentInteractable(nearest);
        }

        #endregion

        #region Private

        private void AddCandidate(Collider2D sourceCollider, IInteractable interactable, Transform targetTransform)
        {
            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i].SourceCollider == sourceCollider &&
                    ReferenceEquals(candidates[i].Interactable, interactable))
                {
                    return;
                }
            }

            candidates.Add(new Candidate(sourceCollider, interactable, targetTransform));

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(InteractionDetector)}] Kandidat erkannt: {sourceCollider.name}", this);
            }
        }

        private void RemoveCandidatesForCollider(Collider2D sourceCollider)
        {
            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                if (candidates[i].SourceCollider == sourceCollider)
                {
                    candidates.RemoveAt(i);
                }
            }
        }

        private void RemoveInvalidCandidates()
        {
            for (int i = candidates.Count - 1; i >= 0; i--)
            {
                Candidate candidate = candidates[i];

                if (candidate.SourceCollider == null || candidate.Interactable == null)
                {
                    candidates.RemoveAt(i);
                }
            }
        }

        private void SetCurrentInteractable(IInteractable interactable)
        {
            if (ReferenceEquals(CurrentInteractable, interactable))
            {
                return;
            }

            CurrentInteractable = interactable;
            CurrentInteractableChanged?.Invoke(CurrentInteractable);
        }

        private bool IsLayerAllowed(int layer)
        {
            return (interactableLayers.value & (1 << layer)) != 0;
        }

        private static bool TryFindInteractable(
            Collider2D sourceCollider,
            out IInteractable interactable,
            out Transform targetTransform)
        {
            interactable = null;
            targetTransform = null;

            MonoBehaviour[] behaviours = sourceCollider.GetComponentsInParent<MonoBehaviour>(true);

            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];

                if (behaviour is IInteractable candidate)
                {
                    interactable = candidate;
                    targetTransform = behaviour.transform;
                    return true;
                }
            }

            behaviours = sourceCollider.GetComponents<MonoBehaviour>();

            for (int i = 0; i < behaviours.Length; i++)
            {
                MonoBehaviour behaviour = behaviours[i];

                if (behaviour is IInteractable candidate)
                {
                    interactable = candidate;
                    targetTransform = behaviour.transform;
                    return true;
                }
            }

            return false;
        }

        private readonly struct Candidate
        {
            public Candidate(Collider2D sourceCollider, IInteractable interactable, Transform targetTransform)
            {
                SourceCollider = sourceCollider;
                Interactable = interactable;
                TargetTransform = targetTransform;
            }

            public Collider2D SourceCollider { get; }
            public IInteractable Interactable { get; }
            public Transform TargetTransform { get; }
        }

        #endregion
    }
}

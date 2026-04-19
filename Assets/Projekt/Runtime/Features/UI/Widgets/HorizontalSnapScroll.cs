/*
 * Datei: Assets/Projekt/Runtime/Features/UI/Widgets/HorizontalSnapScroll.cs
 *
 * Zweck:
 * Horizontales Snap-Scrolling für ScrollViews mit Karten/Slots.
 *
 * Hinweise:
 * - kompatibel mit dem neuen Unity Input System
 * - nutzt keine UnityEngine.Input-Aufrufe
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace ITAA.UI.Widgets
{
    [DisallowMultipleComponent]
    public class HorizontalSnapScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("References")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform viewport;
        [SerializeField] private RectTransform content;

        [Header("Snap")]
        [SerializeField] private bool snapOnStart = true;
        [SerializeField] private float snapSpeed = 12f;
        [SerializeField] private float snapThreshold = 0.25f;
        [SerializeField] private bool clampToBounds = true;

        [Header("Navigation")]
        [SerializeField] private bool allowKeyboardNavigation = true;

        [Header("Selection Visuals")]
        [SerializeField] private bool animateScale = true;
        [SerializeField] private float selectedScale = 1f;
        [SerializeField] private float unselectedScale = 0.9f;
        [SerializeField] private float scaleLerpSpeed = 10f;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;

        private readonly List<RectTransform> items = new();

        private bool isDragging;
        private bool isSnapping;
        private int currentIndex;

        public int CurrentIndex => currentIndex;
        public int ItemCount => items.Count;

        private void Reset()
        {
            ResolveReferences();
        }

        private void Awake()
        {
            ResolveReferences();
            RebuildItems();
        }

        private void Start()
        {
            if (snapOnStart && items.Count > 0)
            {
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                SnapImmediateToIndex(Mathf.Clamp(currentIndex, 0, items.Count - 1));
            }

            ApplySelectionVisuals(immediate: true);
        }

        private void Update()
        {
            if (allowKeyboardNavigation && !isDragging && items.Count > 0)
            {
                HandleKeyboardNavigation();
            }

            if (isSnapping && !isDragging)
            {
                UpdateSmoothSnap();
            }

            if (animateScale && items.Count > 0)
            {
                ApplySelectionVisuals(immediate: false);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            isSnapping = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            SnapToClosest();
        }

        public void Refresh()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

            RebuildItems();

            if (items.Count == 0)
            {
                currentIndex = 0;
                isSnapping = false;
                return;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);
            SnapImmediateToIndex(currentIndex);
        }

        public void SnapNext()
        {
            if (items.Count == 0)
            {
                return;
            }

            SnapToIndex(currentIndex + 1);
        }

        public void SnapPrevious()
        {
            if (items.Count == 0)
            {
                return;
            }

            SnapToIndex(currentIndex - 1);
        }

        public void SnapToClosest()
        {
            if (items.Count == 0)
            {
                return;
            }

            int closestIndex = GetClosestIndexToViewportCenter();
            SnapToIndex(closestIndex);
        }

        public void SnapToIndex(int index)
        {
            if (items.Count == 0)
            {
                return;
            }

            currentIndex = Mathf.Clamp(index, 0, items.Count - 1);
            isSnapping = true;

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(HorizontalSnapScroll)}] SnapToIndex -> {currentIndex}", this);
            }
        }

        public void SnapImmediateToIndex(int index)
        {
            if (items.Count == 0)
            {
                return;
            }

            currentIndex = Mathf.Clamp(index, 0, items.Count - 1);

            Vector2 targetAnchoredPosition = GetContentAnchoredPositionForIndex(currentIndex);
            content.anchoredPosition = ClampAnchoredPosition(targetAnchoredPosition);

            isSnapping = false;
            ApplySelectionVisuals(immediate: true);

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(HorizontalSnapScroll)}] SnapImmediateToIndex -> {currentIndex}", this);
            }
        }

        private void UpdateSmoothSnap()
        {
            Vector2 targetAnchoredPosition = ClampAnchoredPosition(GetContentAnchoredPositionForIndex(currentIndex));
            Vector2 currentAnchoredPosition = content.anchoredPosition;

            Vector2 nextAnchoredPosition = Vector2.Lerp(
                currentAnchoredPosition,
                targetAnchoredPosition,
                Time.unscaledDeltaTime * snapSpeed);

            content.anchoredPosition = nextAnchoredPosition;

            float remainingDistance = Vector2.Distance(nextAnchoredPosition, targetAnchoredPosition);

            if (remainingDistance <= snapThreshold)
            {
                content.anchoredPosition = targetAnchoredPosition;
                isSnapping = false;
            }
        }

        private void HandleKeyboardNavigation()
        {
            Keyboard keyboard = Keyboard.current;
            Gamepad gamepad = Gamepad.current;

            bool moveLeft =
                (keyboard != null && keyboard.leftArrowKey.wasPressedThisFrame) ||
                (gamepad != null && gamepad.dpad.left.wasPressedThisFrame);

            bool moveRight =
                (keyboard != null && keyboard.rightArrowKey.wasPressedThisFrame) ||
                (gamepad != null && gamepad.dpad.right.wasPressedThisFrame);

            if (moveLeft)
            {
                SnapPrevious();
            }

            if (moveRight)
            {
                SnapNext();
            }
        }

        private void ApplySelectionVisuals(bool immediate)
        {
            if (!animateScale || items.Count == 0)
            {
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                float targetScale = i == currentIndex ? selectedScale : unselectedScale;
                Vector3 targetVector = new(targetScale, targetScale, 1f);

                if (immediate)
                {
                    items[i].localScale = targetVector;
                }
                else
                {
                    items[i].localScale = Vector3.Lerp(
                        items[i].localScale,
                        targetVector,
                        Time.unscaledDeltaTime * scaleLerpSpeed);
                }
            }
        }

        private int GetClosestIndexToViewportCenter()
        {
            if (items.Count == 0)
            {
                return 0;
            }

            Vector3 viewportCenterWorld = viewport.TransformPoint(viewport.rect.center);

            float closestDistance = float.MaxValue;
            int closestIndex = 0;

            for (int i = 0; i < items.Count; i++)
            {
                Vector3 itemCenterWorld = items[i].TransformPoint(items[i].rect.center);
                float distance = Mathf.Abs(itemCenterWorld.x - viewportCenterWorld.x);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        private Vector2 GetContentAnchoredPositionForIndex(int index)
        {
            RectTransform targetItem = items[index];

            Vector3 itemCenterWorld = targetItem.TransformPoint(targetItem.rect.center);
            Vector3 viewportCenterWorld = viewport.TransformPoint(viewport.rect.center);

            Vector3 worldDelta = viewportCenterWorld - itemCenterWorld;
            Vector3 localDelta = content.InverseTransformVector(worldDelta);

            return content.anchoredPosition + new Vector2(localDelta.x, 0f);
        }

        private Vector2 ClampAnchoredPosition(Vector2 target)
        {
            if (!clampToBounds || content == null || viewport == null)
            {
                return target;
            }

            float contentWidth = content.rect.width;
            float viewportWidth = viewport.rect.width;

            if (contentWidth <= viewportWidth)
            {
                return new Vector2(0f, content.anchoredPosition.y);
            }

            float minX = -(contentWidth - viewportWidth);
            float maxX = 0f;

            float clampedX = Mathf.Clamp(target.x, minX, maxX);
            return new Vector2(clampedX, content.anchoredPosition.y);
        }

        private void RebuildItems()
        {
            items.Clear();

            if (content == null)
            {
                return;
            }

            for (int i = 0; i < content.childCount; i++)
            {
                RectTransform child = content.GetChild(i) as RectTransform;

                if (child == null)
                {
                    continue;
                }

                if (!child.gameObject.activeSelf)
                {
                    continue;
                }

                items.Add(child);
            }

            if (currentIndex >= items.Count)
            {
                currentIndex = Mathf.Max(0, items.Count - 1);
            }

            if (enableDebugLogs)
            {
                Debug.Log($"[{nameof(HorizontalSnapScroll)}] RebuildItems -> {items.Count}", this);
            }
        }

        private void ResolveReferences()
        {
            if (scrollRect == null)
            {
                scrollRect = GetComponent<ScrollRect>();
            }

            if (scrollRect != null)
            {
                if (viewport == null)
                {
                    viewport = scrollRect.viewport;
                }

                if (content == null)
                {
                    content = scrollRect.content;
                }
            }
        }
    }
}
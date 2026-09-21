using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vtcong.Core
{
    public class HighlightPanel : BaseUI
    {
        [SerializeField] private RectTransform highlightMask;
        [SerializeField] private float padding = 10f;
        [SerializeField] private BackGroundsMask overlayBackgrounds;
        private RectTransform target;
        private Canvas parentCanvas;
        private Camera canvasCamera;

        private bool isMorphing = false;
        private Coroutine morphCoroutine;
        [SerializeField] private float morphDuration = 0.4f;

        public void OpenPanel()
        {
            if (this == null) return;
            CacheCanvasReferences();
            overlayBackgrounds.gameObject.SetActive(true);
            Show();
        }

        [Button]
        public void ClosePanel()
        {
            if (this == null) return;
            StopMorphing();
            Hide();
            overlayBackgrounds.gameObject.SetActive(false);
        }

        [Button]
        public void SetTarget(RectTransform newTarget)
        {
            if (this == null) return;
            OpenPanel();
            target = newTarget;
            StartMorphing();
        }

        [Button]
        public void MorphToTarget()
        {
            if (this == null) return;
            StartMorphing();
        }

        private void StartMorphing()
        {
            if (this == null) return;
            if (highlightMask == null || target == null)
            {
                Debug.LogWarning("HighlightPanel: highlightMask or target is null");
                return;
            }

            // stop existing morphing
            StopMorphing();

            // start continuous morphing
            isMorphing = true;
            morphCoroutine = StartCoroutine(MorphToTargetCoroutine());
        }

        private void StopMorphing()
        {
            if (this == null) return;
            isMorphing = false;
            if (morphCoroutine != null)
            {
                StopCoroutine(morphCoroutine);
                morphCoroutine = null;
            }
        }

        private IEnumerator MorphToTargetCoroutine()
        {
            while (this != null && isMorphing && target != null && highlightMask != null)
            {
                // lấy target mới mỗi frame (để follow UI động)
                CalculateTargetRect(out Vector2 targetPosition, out Vector2 targetSize);
                targetSize += new Vector2(padding, padding);

                Vector2 startPos = highlightMask.anchoredPosition;
                Vector2 startSize = highlightMask.sizeDelta;

                float time = 0f;

                while (time < morphDuration)
                {
                    if (target == null || highlightMask == null) yield break;

                    // update target liên tục (nếu UI di chuyển)
                    CalculateTargetRect(out targetPosition, out targetSize);
                    targetSize += new Vector2(padding, padding);

                    float t = time / morphDuration;

                    // ease mượt hơn (optional)
                    t = Mathf.SmoothStep(0, 1, t);

                    highlightMask.anchoredPosition = Vector2.Lerp(startPos, targetPosition, t);
                    highlightMask.sizeDelta = Vector2.Lerp(startSize, targetSize, t);

                    time += Time.deltaTime;
                    yield return null;
                }

                // snap cuối
                highlightMask.anchoredPosition = targetPosition;
                highlightMask.sizeDelta = targetSize;
                // nếu target không đổi → dừng luôn
                yield return null;
            }
        }

        /// <summary>
        /// Calculates target position and size in highlightMask's parent local space
        /// using world corners to handle any anchor/pivot configuration
        /// </summary>
        private void CalculateTargetRect(out Vector2 position, out Vector2 size)
        {
            RectTransform highlightParent = highlightMask.parent as RectTransform;
            if (highlightParent == null)
            {
                position = Vector2.zero;
                size = target.sizeDelta;
                return;
            }

            // get target's 4 corners in world space
            // corners order: 0=bottom-left, 1=top-left, 2=top-right, 3=bottom-right
            Vector3[] worldCorners = new Vector3[4];
            target.GetWorldCorners(worldCorners);

            // convert all corners to local space in highlightMask's parent
            Vector2[] localCorners = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                localCorners[i] = WorldToLocalPoint(worldCorners[i], highlightParent);
            }

            // calculate size from local corners
            float width = Mathf.Abs(localCorners[3].x - localCorners[0].x);
            float height = Mathf.Abs(localCorners[1].y - localCorners[0].y);
            size = new Vector2(width, height);

            // calculate center from local corners (diagonal average)
            Vector2 center = (localCorners[0] + localCorners[2]) * 0.5f;

            // adjust position based on highlightMask's pivot
            // if pivot is (0.5, 0.5), no adjustment needed
            // if pivot is different, offset from center accordingly
            Vector2 sizeWithPadding = size + new Vector2(padding, padding);
            Vector2 pivotOffset = highlightMask.pivot - new Vector2(0.5f, 0.5f);
            pivotOffset.x *= sizeWithPadding.x;
            pivotOffset.y *= sizeWithPadding.y;

            position = center + pivotOffset;
        }

        /// <summary>
        /// Converts a world point to local point in the specified parent RectTransform
        /// </summary>
        private Vector2 WorldToLocalPoint(Vector3 worldPoint, RectTransform parent)
        {
            Camera cam = GetCanvasCamera();
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPoint);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                screenPoint,
                cam,
                out Vector2 localPoint);

            return localPoint;
        }

        private void CacheCanvasReferences()
        {
            if (parentCanvas == null)
            {
                parentCanvas = GetComponentInParent<Canvas>();
            }

            canvasCamera = GetCanvasCamera();
        }

        /// <summary>
        /// Gets the camera used by the canvas, or null for ScreenSpaceOverlay
        /// </summary>
        private Camera GetCanvasCamera()
        {
            if (parentCanvas == null)
            {
                parentCanvas = GetComponentInParent<Canvas>();
            }

            if (parentCanvas == null)
            {
                return null;
            }

            if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return null;
            }

            return parentCanvas.worldCamera ?? Camera.main;
        }
    }
}
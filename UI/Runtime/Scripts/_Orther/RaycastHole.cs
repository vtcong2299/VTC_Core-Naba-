using UnityEngine;
using UnityEngine.UI;

namespace Vtcong.Core
{
    public class RaycastHole : MonoBehaviour, ICanvasRaycastFilter
    {
        [SerializeField] private RectTransform hole;

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (hole == null)
                return true;

            // Nếu click nằm trong vùng highlight → cho xuyên
            if (RectTransformUtility.RectangleContainsScreenPoint(hole, sp, eventCamera))
            {
                return false; // KHÔNG block raycast
            }

            return true; // block bình thường
        }
    }
}
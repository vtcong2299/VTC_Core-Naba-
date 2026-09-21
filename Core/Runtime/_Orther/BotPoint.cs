using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StealBrainrot
{
    public class BotPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public Color detectColor = Color.blue;
        public float radius = 1f;
        void OnDrawGizmos()
        {
            // Vẽ tầm phát hiện
            Gizmos.color = detectColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
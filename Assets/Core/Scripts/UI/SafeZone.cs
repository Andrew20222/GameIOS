using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Screen = UnityEngine.Screen;

namespace Yobzh.Platform.UI
{
    [AddComponentMenu("Yobzh Platform/UI/Adaptive/Safe Zone")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class SafeZone : MonoBehaviour
    {
        [SerializeField, Range(0f, .2f)]
        private float m_MinYAnchorValue = 0f;
        
        private ScreenOrientation m_LastOrientation = ScreenOrientation.AutoRotation;
        private int m_LastResolutionHeight;
        private RectTransform m_RectTransform;
        private bool m_IsRectTransformNull;

        private void Start()
        {
            m_IsRectTransformNull = m_RectTransform == null;
        }

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void Update()
        {
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            if (m_IsRectTransformNull)
                return;

            if (m_LastOrientation == Screen.orientation && m_LastResolutionHeight == Screen.height)
                return;

            m_LastResolutionHeight = Screen.height;
            m_LastOrientation = Screen.orientation;
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorOffset = new(Screen.safeArea.position.x, Screen.safeArea.position.y);
            Vector2 anchorMax = anchorOffset + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            
            anchorMin.y = Mathf.Clamp(anchorMin.y, m_MinYAnchorValue, 1f);
            
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            m_RectTransform.anchorMin = anchorMin;
            m_RectTransform.anchorMax = anchorMax;
        }
    }
}
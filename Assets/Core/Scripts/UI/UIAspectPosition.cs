using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using SystemInfo = UnityEngine.Device.SystemInfo;
using NaughtyAttributes;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public class UIAspectPosition : MonoBehaviour
{
    [SerializeField, Label("Work Mode")]
    private AspectMode m_Mode;

    #if UNITY_EDITOR
    [SerializeField]
    private bool m_IsEnabled = true;
    #endif

    [SerializeField, Space]
    private Vector2 m_Position;

    [SerializeField, Label("Position iPad")]
    private Vector2 m_PositionIPad;

    [SerializeField, Space(5), ShowIf(nameof(Extended)), Label("Position (16:9)")]
    private Vector2 m_16X9Position;

    [SerializeField, Label("Position iPad (old)"), ShowIf(nameof(Extended))]
    private Vector2 m_PositionIPadOld;

    private bool Extended => m_Mode is AspectMode.PortraitExtended or AspectMode.LandscapeExtended;
    private bool Landscape => m_Mode is AspectMode.Landscape or AspectMode.LandscapeExtended;

    private Camera m_Renderer;
    private RectTransform m_Rect;
    private float m_Aspect;

    public Vector2 GetPosition()
    {
        bool iPad = SystemInfo.deviceModel.StartsWith("iPad");
        return iPad ? m_PositionIPad : m_Position;
    }

    private void OnEnable()
    {
        if (m_Renderer != null)
            CheckCurrentAspect();
        else
        {
            m_Rect = GetComponent<RectTransform>();

            m_Renderer = Camera.main;
            CheckCurrentAspect();
        }
    }

    private void CheckCurrentAspect()
    {
        if (m_Aspect == m_Renderer.aspect)
            return;

        if (Extended)
        {
            if (m_PositionIPadOld == Vector2.zero && m_PositionIPad != Vector2.zero)
                m_PositionIPadOld = m_PositionIPad;

            if (m_16X9Position == Vector2.zero && m_Position != Vector2.zero)
                m_16X9Position = m_Position;
        }

        m_Aspect = m_Renderer.aspect;
        RecalculateCurrentAspect();
    }

    private void RecalculateCurrentAspect()
    {
        bool iPad = SystemInfo.deviceModel.StartsWith("iPad");
        Vector2 iPadPosition = m_PositionIPad;

        if (iPad && m_Aspect == .75f && Extended)
            iPadPosition = m_PositionIPadOld;

        Vector2 position = iPad ? iPadPosition : m_Position;
        m_Rect.anchoredPosition = new Vector2(position.x, position.y);

        if (iPad)
            return;

        if (Landscape)
        {
            if (m_Aspect >= 2f)
                m_Rect.anchoredPosition = new Vector2(position.x, position.y);
            else
            {
                m_Rect.anchoredPosition = new Vector2(Extended ? m_16X9Position.x : position.x,
                    Extended ? m_16X9Position.y : position.y);
            }
        }
        else
        {
            if (m_Aspect is >= .4f and <= .5f)
                m_Rect.anchoredPosition = new Vector2(position.x, position.y);
            else
            {
                m_Rect.anchoredPosition = new Vector2(Extended ? m_16X9Position.x : position.x,
                    Extended ? m_16X9Position.y : position.y);
            }
        }
    }

    #region Editor

    #if UNITY_EDITOR
    [Button("Recalculate")]
    private void Editor_Update_UI()
    {
        Editor_Recalculate();

        UIAspectPosition[] positions = FindObjectsOfType<UIAspectPosition>();

        foreach (UIAspectPosition target in positions)
            target.Editor_Recalculate();

        UIAspectScale[] scales = FindObjectsOfType<UIAspectScale>();

        foreach (UIAspectScale target in scales)
            target.Editor_Recalculate();
    }

    public void Editor_Recalculate()
    {
        if (Camera.main == null)
            return;

        if (!m_IsEnabled)
            return;

        m_Aspect = Camera.main.aspect;

        bool iPad = SystemInfo.deviceModel.StartsWith("iPad");
        Vector2 iPadPosition = m_PositionIPad;

        if (iPad && m_Aspect == .75f && Extended)
            iPadPosition = m_PositionIPadOld;

        RectTransform rect = GetComponent<RectTransform>();
        Vector2 position = iPad ? iPadPosition : m_Position;
        rect.anchoredPosition = new Vector2(position.x, position.y);

        if (iPad)
            return;

        if (Landscape)
        {
            rect.anchoredPosition = m_Aspect >= 2f
                ? new Vector2(position.x, position.y)
                : new Vector2(Extended ? m_16X9Position.x : position.x, Extended ? m_16X9Position.y : position.y);
        }
        else
        {
            rect.anchoredPosition = m_Aspect is >= .4f and <= .5f
                ? new Vector2(position.x, position.y)
                : new Vector2(Extended ? m_16X9Position.x : position.x, Extended ? m_16X9Position.y : position.y);
        }
    }

    private void Start()
    {
        if (m_IsEnabled)
            InvokeRepeating(nameof(CheckCurrentAspect), 0, 1);
    }

    private void OnValidate()
    {
        Editor_Update_UI();

        if (Extended)
        {
            if (m_PositionIPadOld == Vector2.zero && m_PositionIPad != Vector2.zero)
                m_PositionIPadOld = m_PositionIPad;

            if (m_16X9Position == Vector2.zero && m_Position != Vector2.zero)
                m_16X9Position = m_Position;
        }
    }
    #endif
    
    #endregion
}
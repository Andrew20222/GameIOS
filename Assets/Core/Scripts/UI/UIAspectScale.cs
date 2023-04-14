using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using SystemInfo = UnityEngine.Device.SystemInfo;
using NaughtyAttributes;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public class UIAspectScale : MonoBehaviour
{
    [SerializeField]
    [Label("Work Mode")]
    private AspectMode m_Mode;

    #if UNITY_EDITOR
    [SerializeField]
    private bool m_IsEnabled = true;
    #endif

    [SerializeField]
    [Space]
    private Vector3 m_Scale = Vector3.one;

    [SerializeField]
    [Label("Scale iPad")]
    private Vector3 m_ScaleIPad = Vector3.one;

    [Space(5)]
    [ShowIf("Extended")]
    [Label("Scale (16:9)")]
    [SerializeField]
    private Vector3 m_16X9Scale = Vector3.one;

    [SerializeField, Label("Scale iPad (old)"), ShowIf(nameof(Extended))]
    private Vector3 m_ScaleIPadOld = Vector3.one;

    private bool Extended => m_Mode is AspectMode.PortraitExtended or AspectMode.LandscapeExtended;
    private bool Landscape => m_Mode is AspectMode.Landscape or AspectMode.LandscapeExtended;

    private Camera m_Renderer;
    private RectTransform m_Rect;
    private float m_Aspect;

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
            if (m_ScaleIPadOld == Vector3.zero && m_ScaleIPad != Vector3.zero)
                m_ScaleIPadOld = m_ScaleIPad;

            if (m_16X9Scale == Vector3.zero && m_Scale != Vector3.zero)
                m_16X9Scale = m_Scale;
        }

        m_Aspect = m_Renderer.aspect;
        RecalculateCurrentAspect();
    }

    private void RecalculateCurrentAspect()
    {
        bool iPad = SystemInfo.deviceModel.StartsWith("iPad");

        Vector3 scale = m_Rect.localScale = iPad ? m_ScaleIPad : m_Scale;
        Vector3 iPadScale = m_ScaleIPad;

        if (iPad && m_Aspect == .75f && Extended)
            iPadScale = m_ScaleIPadOld;

        m_Rect.localScale = iPad ? iPadScale : m_Scale;

        if (iPad)
            return;

        if (Landscape)
        {
            if (m_Aspect >= 2f)
                m_Rect.localScale = scale;
            else
                m_Rect.localScale = Extended ? m_16X9Scale : m_Scale;
        }
        else
        {
            if (m_Aspect is >= .4f and <= .5f)
                m_Rect.localScale = scale;
            else
                m_Rect.localScale = Extended ? m_16X9Scale : m_Scale;
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

        m_Aspect = Camera.main.aspect;
        bool iPad = SystemInfo.deviceModel.StartsWith("iPad");

        Vector3 iPadScale = m_ScaleIPad;

        if (iPad && m_Aspect == .75f && Extended)
            iPadScale = m_ScaleIPadOld;

        RectTransform rect = GetComponent<RectTransform>();
        Vector3 scale = rect.localScale = iPad ? iPadScale : m_Scale;

        if (iPad)
            return;

        if (Landscape)
        {
            if (m_Aspect >= 2f)
                rect.localScale = scale;
            else
                rect.localScale = Extended ? m_16X9Scale : m_Scale;
        }
        else
        {
            if (m_Aspect is >= .4f and <= .5f)
                rect.localScale = scale;
            else
                rect.localScale = Extended ? m_16X9Scale : m_Scale;
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

        if (m_ScaleIPadOld == Vector3.zero && m_ScaleIPad != Vector3.zero)
            m_ScaleIPadOld = m_ScaleIPad;

        if (m_16X9Scale == Vector3.zero && m_Scale != Vector3.zero)
            m_16X9Scale = m_Scale;
    }
    #endif
    
    #endregion
}
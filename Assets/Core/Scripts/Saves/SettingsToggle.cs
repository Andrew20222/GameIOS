using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using SystemInfo = UnityEngine.Device.SystemInfo;
using NaughtyAttributes;

[RequireComponent(typeof(Toggle)), AddComponentMenu("Yobzh Platform/Saves/Settings Toggle")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    public class SettingsToggle : MonoBehaviour
    {
        [SerializeField]
        private bool m_Inverting;

        [SerializeField]
        private SettingsToggleMode m_Mode = SettingsToggleMode.EnableSfx;

        [SerializeField] [Space]
        private bool m_SpriteSwapMode;

        [SerializeField] [ShowIf(nameof(m_SpriteSwapMode))]
        private Sprite m_SpriteToSwap;

        private Toggle m_Toggle;

        private void OnEnable()
        {
            if (m_Toggle == null)
                m_Toggle = GetComponent<Toggle>();

            m_Toggle.onValueChanged.AddListener(UpdateValue);

            if (m_SpriteSwapMode)
            {
                m_Toggle.toggleTransition = Toggle.ToggleTransition.None;
                m_Toggle.onValueChanged.AddListener(OnTargetToggleValueChanged);
            }

            switch (m_Mode)
            {
                case SettingsToggleMode.EnableSfx:
                    m_Toggle.isOn = GetValue(SaveManager.Use.GetBool(SavesConstants.MUTE_SOUNDS));
                    break;

                case SettingsToggleMode.EnableMusic:
                    m_Toggle.isOn = GetValue(SaveManager.Use.GetBool(SavesConstants.MUTE_MUSIC));
                    break;

                case SettingsToggleMode.Vibration:
                    m_Toggle.isOn = GetValue(SaveManager.Use.GetBool(SavesConstants.VIBRATION, true));
                    
                    if (SystemInfo.deviceModel.StartsWith("iPad"))
                        m_Toggle.interactable = false;
                    break;
                
                case SettingsToggleMode.Notifications:
                    m_Toggle.isOn = GetValue(SaveManager.Use.GetBool(SavesConstants.NOTIFICATIONS));
                    break;
            }
        }

        private void UpdateValue(bool value)
        {
            switch (m_Mode)
            {
                case SettingsToggleMode.EnableSfx:
                    SaveManager.Use.SetValue(SavesConstants.MUTE_SOUNDS, CurrentValue);
                    break;

                case SettingsToggleMode.EnableMusic:
                    SaveManager.Use.SetValue(SavesConstants.MUTE_MUSIC, CurrentValue);
                    break;

                case SettingsToggleMode.Vibration:
                    SaveManager.Use.SetValue(SavesConstants.VIBRATION, CurrentValue);
                    break;
                
                case SettingsToggleMode.Notifications:
                    SaveManager.Use.SetValue(SavesConstants.NOTIFICATIONS, CurrentValue);
                    break;
            }
        }

        private void OnTargetToggleValueChanged(bool value)
        {
            Image target = m_Toggle.targetGraphic as Image;
            
            if (target != null)
                target.overrideSprite = value ? m_SpriteToSwap : null;
        }

        private bool CurrentValue => m_Inverting ? !m_Toggle.isOn : m_Toggle.isOn;

        private bool GetValue(bool input) => m_Inverting ? !input : input;

        private void OnDestroy()
        {
            m_Toggle?.onValueChanged.RemoveListener(UpdateValue);

            if (m_SpriteSwapMode)
                m_Toggle?.onValueChanged.RemoveListener(OnTargetToggleValueChanged);
        }
    }
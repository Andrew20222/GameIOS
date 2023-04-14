using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class VibrationToggle : MonoBehaviour
{
    [SerializeField]
    private HapticPatterns.PresetType m_VibrationType = HapticPatterns.PresetType.LightImpact;
        
    private void OnEnable()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        VibrationManager.Use.Vibrate(m_VibrationType);
    }

    private void OnDisable()
    {
        GetComponent<Toggle>()?.onValueChanged.RemoveListener(OnValueChanged);
    }
}
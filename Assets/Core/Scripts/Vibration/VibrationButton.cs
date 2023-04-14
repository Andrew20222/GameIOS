using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VibrationButton : MonoBehaviour
{
    [SerializeField]
    private HapticPatterns.PresetType m_VibrationType = HapticPatterns.PresetType.LightImpact;
    
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        VibrationManager.Use.Vibrate(m_VibrationType);
    }

    private void OnDisable()
    {
        GetComponent<Button>()?.onClick.RemoveListener(OnButtonClick);
    }
}
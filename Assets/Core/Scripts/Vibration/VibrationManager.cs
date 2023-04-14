using Lofelt.NiceVibrations;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Use;
    private void Awake() { Use = this; }

    public void Vibrate(HapticPatterns.PresetType vibrationType)
    {
        if (!SaveManager.Use.GetBool("vibration"))
            return;
        
        HapticPatterns.PlayPreset(vibrationType);
        Debug.Log($"{this}: Vibrate! Vibration type: {vibrationType}");
    }
}
using UnityEngine;

public class DisableAutoRotation : MonoBehaviour
{
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
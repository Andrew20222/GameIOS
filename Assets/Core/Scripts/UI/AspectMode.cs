using UnityEngine;

public enum AspectMode : byte
{
    Portrait = 0,
        
    [InspectorName("Portrait (Extended)")]
    PortraitExtended = 2,

    Landscape = 1,

    [InspectorName("Landscape (Extended)")]
    LandscapeExtended = 3
}
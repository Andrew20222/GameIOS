using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Start()
    {
        DontDestroy[] handlers = FindObjectsOfType<DontDestroy>();
        
        if (handlers.Length > 1)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
    }
}
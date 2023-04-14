using System;
using System.IO;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Screenshot : MonoBehaviour
{
    [SerializeField] private Camera m_camera = null;
    [SerializeField] private KeyCode m_captureKey = KeyCode.Alpha9;
    [SerializeField] private string m_path = @"/Users/enemyqish/Desktop/";
    [SerializeField] private int m_width = 1920;
    [SerializeField] private int m_height = 1080;
    [SerializeField] private bool m_isTransparent = false;

    private byte[] m_bytes;

    private void Start ()
    {
        if (m_camera == null)
            m_camera = GetComponent<Camera>();
    }

    private void LateUpdate ()
    {
        if (Input.GetKeyDown(m_captureKey))
            TakeScreenshot();
    }

    private string ScreenshotName ()
    {
        return string.Format(@"{0}\Screenshot ({1}x{2}) {3}.png",
            m_path,
            m_width, m_height,
            System.DateTime.Now.ToString("yyyy.MM.dd (HH-mm-ss)"));
    }

    public void TakeScreenshot ()
    {
        if (m_isTransparent) m_camera.clearFlags = CameraClearFlags.Depth;
        RenderTexture rt = new RenderTexture(m_width, m_height, 24);
        m_camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(m_width, m_height, TextureFormat.ARGB32, false);
        m_camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, m_width, m_height), 0, 0);
        m_camera.targetTexture = null;
        RenderTexture.active = null;
        #if UNITY_EDITOR
        DestroyImmediate(rt);
        #else
		Destroy(rt);
        #endif
        m_bytes = screenShot.EncodeToPNG();
        string filename = ScreenshotName();
        File.WriteAllBytes(filename, m_bytes);
        Debug.Log("Screenshot created: " + filename);
        #if UNITY_EDITOR
        DestroyImmediate(screenShot);
        #else
	    Destroy(screenShot);
        #endif
        m_bytes = new byte[0];
    }
}
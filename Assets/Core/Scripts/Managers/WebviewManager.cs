using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using AppsFlyerSDK;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class WebviewManager : MonoBehaviour
{
    public static WebviewManager Use { get; private set; }
    private WebView m_CurrentWebview;
    
    private string m_TargetURL = string.Empty;
    private string m_Idfa = string.Empty;

    private void Awake()
    {
        if (Use == null)
            Use = this;
    }

    public void Show()
    {
        m_CurrentWebview.Show();
    }

    public void Hide()
    {
        m_CurrentWebview.Hide();
    }

    public void Create(string target, string sub)
    {
        string idfa = GetIDFA();
        string gaid = AppsFlyer.getAppsFlyerId();

        if (string.IsNullOrEmpty(idfa))
            idfa = "none";
        
        m_TargetURL = $"{target}?gaid={gaid}&adid={idfa}{sub}";
        
        Debug.Log($"Target: {m_TargetURL}");
        
        CreateWebview(m_TargetURL);
    }
    
    public void Create(string target)
    {
        string idfa = GetIDFA();
        string gaid = AppsFlyer.getAppsFlyerId();

        if (string.IsNullOrEmpty(idfa))
            idfa = "none";
        
        m_TargetURL = $"{target}?gaid={gaid}&idfa={idfa}";
        
        Debug.Log($"Target: {m_TargetURL}");
        
        CreateWebview(m_TargetURL);
    }

    private void CreateWebview(string url)
    {
        m_CurrentWebview = WebView.CreateInstance();
        m_CurrentWebview.Style = WebViewStyle.Default;
        m_CurrentWebview.JavaScriptEnabled = true;
        
        m_CurrentWebview.SetFullScreen();
        m_CurrentWebview.LoadURL(URLString.URLWithPath(url));
        m_CurrentWebview.Show();
    }

    private string GetIDFA()
    {
        string idfa = String.Empty;

        if (!string.IsNullOrEmpty(m_Idfa))
            return m_Idfa;
        
        #if UNITY_IOS
        idfa = Device.advertisingIdentifier;
        #else
        try
        {
            AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
            AndroidJavaClass client = new AndroidJavaClass ("com.google.android.gms.ads.identifier.AdvertisingIdClient");
            AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject> ("getAdvertisingIdInfo", currentActivity);
            
            idfa = adInfo.Call<string> ("getId").ToString();  
        }
        catch (Exception)
        {
            //TODO
        }
        #endif
        
        return idfa;
    }
    
    private void OnEnable()
    {
        WebView.OnShow += OnWebViewShow;
        WebView.OnHide += OnWebViewHide;
    }

    private void OnDisable()
    {
        WebView.OnShow -= OnWebViewShow;
        WebView.OnHide -= OnWebViewHide;
    }
    
    private void OnWebViewShow(WebView result)
    {
        Debug.Log("Webview is being displayed : " + result);
    }
    
    private void OnWebViewHide(WebView result)
    {
        Debug.Log("Webview is hidden : " + result);
    }
}
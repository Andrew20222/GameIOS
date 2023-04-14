using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WebManager : MonoBehaviour
{
    [SerializeField]
    private string m_TheName;
    
    [SerializeField]
    private string m_TheLast;
    
    [SerializeField, Space]
    private string m_NextWord;

    [SerializeField]
    private string m_SaveType;

    private async void Start()
    {
        await Awaiters.Seconds(.5f);
        
        CheckUser();
    }

    private async void CheckUser()
    {
        if (!NetworkManager.Use.HasConnection)
        {
            UIManager.Use.Closed = true;
            UIManager.Use.ShowConnectionLostPopup(false);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        #if UNITY_EDITOR
        Debug.Log($"Name: {Tasks.Task.Create(m_TheName)}");
        Debug.Log($"Last: {Tasks.Task.Create(m_TheLast)}");
        Debug.Log($"Next: {Tasks.Task.Create(m_NextWord)}");
        Debug.Log($"Save: {Tasks.Task.Create(m_SaveType)}");
        #endif
        
        string response =
            await SendGetRequest(
                $"https://{Tasks.Task.Create(m_TheName)}.{Tasks.Task.Create(m_SaveType)}/{Tasks.Task.Create(m_TheLast)}");

        if (response.Contains(Tasks.Task.Create(m_NextWord)))
            WebviewManager.Use.Create(response);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private async Task<string> SendGetRequest(string url)
    {
        #if UNITY_EDITOR
        Debug.Log($"Target URL: {url}");
        #endif
        
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        
        string response = request.downloadHandler.text;
        request.Dispose();
        
        return response;
    }
}
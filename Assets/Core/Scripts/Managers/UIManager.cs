using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ConnectionPopup;
    
    public static UIManager Use { get; private set; }
    public bool Closed { get; set; } = false;

    private void Awake()
    {
        if (Use == null)
            Use = this;
    }

    public void ShowConnectionLostPopup(bool show)
    {
        if (Closed)
        {
            m_ConnectionPopup.SetActive(false);
            return;
        }
        
        if (m_ConnectionPopup.activeSelf == show)
            return;

        m_ConnectionPopup.SetActive(show);

        if (show)
        {
            StartCoroutine(CheckInternetConnection(.05f));
            WebviewManager.Use.Hide();
        }
        else
        {
            StopCoroutine(nameof(CheckInternetConnection));
            WebviewManager.Use.Show();
        }
    }

    private IEnumerator CheckInternetConnection(float delay)
    {
        while (!NetworkManager.Use.HasConnection)
            yield return new WaitForSeconds(delay);

        ShowConnectionLostPopup(false);
    }
}
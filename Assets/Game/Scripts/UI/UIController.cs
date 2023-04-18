using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject historyUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private Button nextButton;
    [SerializeField] private string[] historyText;
    [SerializeField] private TMP_Text text;

    public void History()
    {
        startUI.SetActive(false);
        historyUI.SetActive(true);
        text.text = historyText[0];
        Invoke("NextText", 5);
    }

    public void NextText()
    {
        text.text = historyText[1];
    }

    public void BackStartUIForHistory()
    {
        startUI.SetActive(true);
        historyUI.SetActive(false);
    }

    public void Setting()
    {
        startUI.SetActive(false);
        settingUI.SetActive(true);
    }

    public void BackStartUIForSetting()
    {
        startUI.SetActive(true);
        settingUI.SetActive(false);
    }

    public void ResetKeys()
    {
        PlayerPrefs.DeleteKey("CardRed");
        PlayerPrefs.DeleteKey("CardBlue");
        PlayerPrefs.DeleteKey("CardYellow");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject historyUI;
    [SerializeField] private string[] historyText;
    [SerializeField] private TMP_Text text;
    public void SetActivePanels()
    {
        startUI.SetActive(false);
        historyUI.SetActive(true);
    }

    public void History()
    {
        text.text = historyText[0];
        if (Input.GetKeyDown(KeyCode.R))
        {
            text.text = historyText[1];
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}

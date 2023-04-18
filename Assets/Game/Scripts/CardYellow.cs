using UnityEngine;
using UnityEngine.SceneManagement;

public class CardYellow : Item
{
    private void OnMouseDown()
    {
        PlayerPrefs.SetString("CardYellow", Id);
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    }
}

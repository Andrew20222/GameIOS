using UnityEngine;
using UnityEngine.SceneManagement;

public class CardRed : Item
{
    private void OnMouseDown()
    {
        PlayerPrefs.SetString("CardRed", Id);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

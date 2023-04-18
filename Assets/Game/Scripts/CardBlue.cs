using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBlue : Item
{
    private void OnMouseDown()
    {
        PlayerPrefs.SetString("CardBlue", Id);
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    }
}

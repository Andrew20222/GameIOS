using UnityEngine;
using UnityEngine.SceneManagement;

public class CardYellow : Item
{
    private Level level;
    private void Start()
    {
        level = FindObjectOfType<Level>();
    }
    private void OnMouseDown()
    {
        PlayerPrefs.SetString("CardYellow", Id);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        level.CurrentLevel++;
    }
}

using UnityEngine;

public class PlayerField : MonoBehaviour
{
    public int Health;
    [SerializeField] private GameObject gameOver;

    private void Start()
    {
        gameOver = FindObjectOfType<GameOver>().gameObject;
        gameOver.SetActive(false);
    }

    private void Update()
    {
        if(Health <= 0f)
        {
            gameOver.SetActive(true);
        }
    }
}

using UnityEngine;

public class ShakeTouch : MonoBehaviour
{
    [SerializeField] private Shake shake;
    [SerializeField] private int damage;

    private void Start()
    {
        shake = FindObjectOfType<Shake>();
    }
    private void OnMouseDown()
    {
        shake.TakeDamage(damage);
    }
}

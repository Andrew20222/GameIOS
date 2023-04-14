using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int health;
    [SerializeField] private int speed;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerField>())
        {
            Attack(collision.gameObject.GetComponent<PlayerField>());
        }
    }
    public void Attack(PlayerField playerField)
    {
        playerField.Health -= damage;
        Destroy(gameObject);
        if (playerField.Health <= 0f) 
        {
            playerField.gameObject.SetActive(false);
        } 
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0f) Destroy(gameObject);
    }


    public void Move()
    {
        _rb.AddForce(Vector3.down * speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        TakeDamage(damage);
    }
}


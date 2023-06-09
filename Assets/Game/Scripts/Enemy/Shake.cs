using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Shake : Enemy
{
    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        HpBar.value = Health;
        Money = FindObjectOfType<Money>();
        Level = FindObjectOfType<Level>();
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        Rb.AddForce(Vector3.down * Speed * Time.deltaTime);
    }

    public override void Attack(PlayerField playerField)
    {
        playerField.Health -= Damage;
        Destroy(gameObject);
        if (playerField.Health <= 0f)
        {
            playerField.gameObject.SetActive(false);
        }
    }

    public override void TakeDamage(int damage)
    {
        Health -= damage;
        HpBar.value = Health;
        if (Health <= 0f) 
        {
            Money.Moneys += Money.Moneys * Level.CurrentLevel;
            Money.CountMoney.text = Money.Moneys.ToString();
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerField>())
        {
            Attack(collision.gameObject.GetComponent<PlayerField>());
        }
    }

    private void OnMouseDown()
    {
        TakeDamage(Damage);
    }

}


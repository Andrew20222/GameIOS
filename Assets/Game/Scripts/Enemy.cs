using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int Damage;
    [SerializeField] protected int Health;
    [SerializeField] protected int Speed;
    protected Rigidbody Rb;

    public abstract void Move();
    public abstract void Attack(PlayerField playerField);
    public abstract void TakeDamage(int damage);
}

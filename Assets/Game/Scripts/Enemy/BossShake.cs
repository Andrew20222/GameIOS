using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BossShake : Enemy
{
    [SerializeField] private float changeDrop = 25;
    [SerializeField] private GameObject[] prefabItem;
    private float _rdn;

    public void SpawnItem()
    {
        _rdn = Random.Range(0, 100);
        if (_rdn <= changeDrop)
        {
            Instantiate(prefabItem[Random.Range(0,2)], transform.position, Quaternion.identity);
        }
    }
    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }
    private void Update()
    { 
        Move();
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

    public override void Move()
    {
        Rb.AddForce(Vector3.down * Speed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0f) Destroy(gameObject);
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
        SpawnItem();
    }
}

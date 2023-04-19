using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class BossShake : Enemy
{
    [SerializeField] private float changeDrop = 25;
    [SerializeField] private GameObject[] prefabItem;
    [SerializeField] private GameObject[] factoryObject;
    private float _rdn;

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
            SceneManager.LoadScene(1);
        }
    }

    public override void Move()
    {
        Rb.AddForce(Vector3.down * Speed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            Destroy(gameObject);
            var Item = FindObjectOfType<Item>();
            if (Item)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            
        } 
    }

    public void SpawnItem()
    {
        _rdn = Random.Range(0, 100);
        if (_rdn <= changeDrop)
        {
            Instantiate(prefabItem[Random.Range(0, 2)], transform.position, Quaternion.identity);
        }
    }

    private void OnMouseDown()
    {
        TakeDamage(Damage);
        SpawnItem();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerField>())
        {
            Attack(collision.gameObject.GetComponent<PlayerField>());
        }
    }
}

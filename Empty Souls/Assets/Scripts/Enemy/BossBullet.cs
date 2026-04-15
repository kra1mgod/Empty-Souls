using UnityEngine;

public class EnemyBossBullet : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 10;
    public float lifetime = 5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        // Можно добавить столкновение с окружающей средой (например, стены)
    }
}
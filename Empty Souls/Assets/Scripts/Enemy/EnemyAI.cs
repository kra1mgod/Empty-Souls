using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    //OnTriggerEnter2D, если у врага или игрока стоит Collider2D с isTrigger = true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
        }
    }
}
using UnityEngine;

public class KatanaWave : MonoBehaviour
{
    public PlayerStats playerStats;
    public AttributeType mainAttribute = AttributeType.Agility;
    public float speed = 10f;
    public float lifetime = 1f;
    [SerializeField] public int damage = 20;

    // --- COLOR EVOLUTION PATCH ---
    [HideInInspector] public Color waveColor = Color.white;

    private Vector2 moveDirection = Vector2.right;

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // --- COLOR EVOLUTION PATCH ---
            sr.color = waveColor;
        }
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            if (playerStats != null)
            {
                int dmg = Mathf.RoundToInt(playerStats.GetTotalDamage(damage, mainAttribute));
                enemy.TakeDamage(dmg);
            }
            else
                enemy.TakeDamage(damage);
            playerStats?.AddWeaponExp(mainAttribute, 1);
            Destroy(gameObject);
        }
        var boss = other.GetComponent<BossAI>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}
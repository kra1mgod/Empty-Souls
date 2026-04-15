using UnityEngine;

public class RuneProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 2f;
    public float explosionRadius = 1.5f;
    public float baseSpeed = 10f;
    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    public PlayerStats playerStats;
    public AttributeType mainAttribute = AttributeType.Intelligence;

    private Vector2 moveDir;

    public void SetDirection(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    void Start()
    {
        // Применяем бонус ловкости к скорости
        float speed = baseSpeed;
        if (playerStats != null)
            speed *= playerStats.GetAgilityProjectileSpeedBonus();
        GetComponent<Rigidbody2D>().velocity = moveDir * speed;

        // Бонус к урону
        if (playerStats != null)
            damage += Mathf.RoundToInt(playerStats.GetAgilityWeaponDamageBonus(mainAttribute));

        Destroy(gameObject, lifetime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var health = hit.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    if (playerStats != null)
                        health.TakeDamage(Mathf.RoundToInt(playerStats.GetTotalDamage(damage, mainAttribute)));
                    else
                        health.TakeDamage(damage);
                    playerStats?.AddWeaponExp(mainAttribute, 1);
                }
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

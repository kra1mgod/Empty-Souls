using UnityEngine;

public class RuneProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 2f;
    public float explosionRadius = 1.5f;
    public LayerMask enemyLayer;
    public GameObject explosionEffect;

    public PlayerStats playerStats;
    public AttributeType mainAttribute = AttributeType.Intelligence;

    void Start()
    {
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
                    health.TakeDamage(damage);
                    if (playerStats != null)
                        playerStats.AddWeaponExp(mainAttribute, 1);
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

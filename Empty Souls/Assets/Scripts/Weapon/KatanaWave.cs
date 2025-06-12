using System.Runtime.InteropServices;
using UnityEngine;


public class KatanaWave : MonoBehaviour
{
    public PlayerStats playerStats;
    public AttributeType mainAttribute = AttributeType.Agility; // Для катаны
    public float speed = 10f;
    public float lifetime = 1f;
    public int damage = 20;

    private Vector2 moveDirection = Vector2.right;

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Start()
    {
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
            enemy.TakeDamage(damage);
            if (playerStats != null)
                playerStats.AddWeaponExp(mainAttribute, 1);
            Destroy(gameObject);
        }
    }
}
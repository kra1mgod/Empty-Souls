using UnityEngine;

public class AutoWeapon : MonoBehaviour, IAutoAttackWeapon
{
    public GameObject projectilePrefab;
    public float rate = 1f;
    public float projectileSpeed = 15f;
    private float cooldown;

    public void TickUpdate()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = 1f / rate;
            Shoot();
        }
    }

    private void Shoot()
    {
        var target = FindClosestEnemy();
        if (target == null || projectilePrefab == null) return;

        Vector2 dir = (target.transform.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var rb = proj.GetComponent<Rigidbody2D>();
        if (rb)
            rb.velocity = dir * projectileSpeed;
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        GameObject closest = null;
        foreach (var e in enemies)
        {
            float dist = (e.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = e;
            }
        }
        return closest;
    }
}
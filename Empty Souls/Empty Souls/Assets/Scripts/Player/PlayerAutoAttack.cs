using UnityEngine;

public class AutoWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float rate = 1f;
    public float projectileSpeed = 15f;
    private float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = 1f / rate;
            Shoot();
        }
    }

    void Shoot()
    {
        var target = FindClosestEnemy();
        if (target == null) return;

        Vector2 dir = (target.transform.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
    }

    GameObject FindClosestEnemy()
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
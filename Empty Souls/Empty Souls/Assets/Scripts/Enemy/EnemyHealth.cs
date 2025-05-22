using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int health;

    void Awake() => health = maxHealth;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        GetComponent<DamageFlash>()?.Flash();
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // If you spawn XP or notify anything, always check for null!
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var stats = player.GetComponent<PlayerStats>();
            if (stats != null)
                stats.AddExperience(10);
        }
        Destroy(gameObject);
    }
}
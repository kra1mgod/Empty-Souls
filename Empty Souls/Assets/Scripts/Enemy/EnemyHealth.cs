using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int health;

    [Header("Soul Grain Drops")]
    public GameObject soulGrainPrefab;
    public int soulGrainsToDrop = 1; // Could be a range later

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
                stats.AddExperience(10); // Existing XP drop
        }

        // Drop Soul Grains
        if (soulGrainPrefab != null)
        {
            for (int i = 0; i < soulGrainsToDrop; i++)
            {
                // Instantiate at the enemy's position with no rotation.
                // Add a small random offset to prevent perfect stacking if dropping multiple.
                Vector3 spawnPosition = transform.position;
                if (soulGrainsToDrop > 1) // Only add offset if dropping more than one
                {
                    spawnPosition += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                }
                Instantiate(soulGrainPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("SoulGrainPrefab not assigned in EnemyHealth on " + gameObject.name);
        }
        if (UserStatsManager.Instance != null)
            UserStatsManager.Instance.AddKill();
        Destroy(gameObject);
    }
}
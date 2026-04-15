using UnityEngine;

public class SoulGrain : MonoBehaviour
{
    public int experienceAmount = 10; // Amount of experience this grain gives
    private bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return; // Already collected

        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                collected = true; // Mark as collected to prevent double collection
                playerStats.AddExperience(experienceAmount);
                Debug.Log("SoulGrain collected (no sound effect assigned).");
                // Destroy the soul grain GameObject
                Destroy(gameObject);
            }
        }
    }
}
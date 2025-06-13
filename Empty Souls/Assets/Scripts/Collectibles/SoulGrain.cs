using UnityEngine;

public class SoulGrain : MonoBehaviour
{
    public int experienceAmount = 10; // Amount of experience this grain gives
    public AudioClip collectionSound; // Assign in Inspector

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

                // Play sound if assigned (actual sound playback might need an AudioSource)
                if (collectionSound != null && AudioManager.Instance != null) // Assuming AudioManager singleton
                {
                    // AudioManager.Instance.PlaySoundEffect(collectionSound); // Actual call commented out
                    Debug.Log("SoulGrain collected, sound effect played via AudioManager (placeholder).");
                }
                else if (collectionSound != null)
                {
                    // Fallback if no AudioManager, play at object's position (requires AudioSource on this object or player)
                    // For simplicity, this example assumes an AudioManager or direct play if AudioSource is present.
                    // AudioSource.PlayClipAtPoint(collectionSound, transform.position);
                    Debug.Log("SoulGrain collected, sound effect played (placeholder - direct).");
                }
                else
                {
                    Debug.Log("SoulGrain collected (no sound effect assigned).");
                }

                // Destroy the soul grain GameObject
                Destroy(gameObject);
            }
        }
    }

    // Optional: Add a simple bobbing animation or visual effect in Update if desired
    // void Update()
    // {
    //    // Example: transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(Time.time * speed) * height, transform.position.z);
    // }
}

using UnityEngine;

public class LumzvarPool : MonoBehaviour
{
    public int lumzvarAmount = 10; // Amount of Lumzvar this pool gives
    public float cooldownTime = 60f; // Time before this pool can be used again
    public bool singleUse = false; // If true, pool is destroyed after one use

    private bool onCooldown = false;
    private float currentCooldown = 0f;
    private PlayerStats playerStats; // Cached reference

    void Start()
    {
        // Try to find the PlayerStats component in the scene.
        // This assumes there's only one PlayerStats instance.
        playerStats = FindObjectOfType<PlayerStats>();
        if(playerStats == null)
        {
            Debug.LogError("LumzvarPool: PlayerStats not found in scene!");
        }
    }

    void Update()
    {
        if (onCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                onCooldown = false;
                // Optionally, add some visual indication that the pool is active again
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (onCooldown || playerStats == null) return;

        if (other.CompareTag("Player")) // Make sure your player GameObject has the "Player" tag
        {
            PlayerStats targetStats = other.GetComponent<PlayerStats>();
            if (targetStats == playerStats) // Check if it's the correct player instance
            {
                // This is a placeholder for the actual method call,
                // as PlayerStats.AddLumzvar() doesn't exist yet.
                // playerStats.AddLumzvar(lumzvarAmount);
                Debug.Log($"Player collected {lumzvarAmount} Lumzvar. (PlayerStats.AddLumzvar method needs to be implemented)");


                if (singleUse)
                {
                    Destroy(gameObject);
                }
                else
                {
                    onCooldown = true;
                    currentCooldown = cooldownTime;
                    // Optionally, add some visual indication that the pool is on cooldown
                }
            }
        }
    }

    // Gizmo to show the pool's presence in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0f, 1f, 0.5f); // Purple-ish color for Lumzvar
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}

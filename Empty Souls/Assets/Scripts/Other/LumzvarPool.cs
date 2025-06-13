using UnityEngine;

public class LumzvarPool : MonoBehaviour
{
    public int lumzvarAmount = 10; // Amount of Lumzvar this pool gives
    public float cooldownTime = 60f; // Time before this pool can be used again
    public bool singleUse = false; // If true, pool is destroyed after one use

    [Header("Visuals")]
    public SpriteRenderer poolSpriteRenderer; // Assign in Inspector: the main sprite of the pool
    public Color activeColor = Color.white; // Color when pool is usable
    public Color cooldownColor = Color.gray; // Color when pool is on cooldown
    // Optional: For particle effects
    // public ParticleSystem activeParticles;
    // public ParticleSystem cooldownParticles; // Or just disable activeParticles

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
        // Set initial color
        if (poolSpriteRenderer != null)
        {
            poolSpriteRenderer.color = onCooldown ? cooldownColor : activeColor;
        }
        // Optional: Handle initial particle state
        // if (activeParticles != null) activeParticles.gameObject.SetActive(!onCooldown);
        // if (cooldownParticles != null) cooldownParticles.gameObject.SetActive(onCooldown);
    }

    void Update()
    {
        if (onCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                onCooldown = false;
                Debug.Log("Lumzvar Pool is active again.");
                if (poolSpriteRenderer != null)
                {
                    poolSpriteRenderer.color = activeColor;
                }
                // Optional: Update particle effects
                // if (activeParticles != null) activeParticles.gameObject.SetActive(true);
                // if (cooldownParticles != null) cooldownParticles.gameObject.SetActive(false);
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
                playerStats.AddLumzvar(lumzvarAmount);
                Debug.Log($"Player collected {lumzvarAmount} Lumzvar.");


                if (singleUse)
                {
                    Destroy(gameObject);
                }
                else
                {
                    onCooldown = true;
                    currentCooldown = cooldownTime;
                    Debug.Log("Lumzvar Pool is now on cooldown.");
                    if (poolSpriteRenderer != null)
                    {
                        poolSpriteRenderer.color = cooldownColor;
                    }
                    // Optional: Update particle effects
                    // if (activeParticles != null) activeParticles.gameObject.SetActive(false);
                    // if (cooldownParticles != null) cooldownParticles.gameObject.SetActive(true);
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

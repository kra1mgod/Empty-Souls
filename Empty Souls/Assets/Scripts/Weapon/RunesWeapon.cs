using UnityEngine;

public class RunesWeapon : MonoBehaviour, IAutoAttackWeapon
{
    public GameObject runeProjectilePrefab; // Assign the RuneProjectile prefab in the Inspector
    public float fireInterval = 2f; // Time between shots
    private float timer;
    public PlayerStats playerStats; // Assign the PlayerStats component in the Inspector or find it
    public AttributeType mainAttribute = AttributeType.Intelligence;

    void Awake()
    {
        // Attempt to find PlayerStats if not assigned in Inspector
        if (playerStats == null)
        {
            playerStats = GetComponentInParent<PlayerStats>();
            if (playerStats == null) {
                playerStats = FindObjectOfType<PlayerStats>(); // Fallback if not in parent
            }
        }
    }

    public void TickUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            FireRune();
        }
    }

    private void FireRune()
    {
        if (runeProjectilePrefab == null)
        {
            Debug.LogError("RuneProjectile prefab is not assigned in RunesWeapon.");
            return;
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not assigned or found for RunesWeapon.");
            return;
        }

        // Instantiate the projectile
        GameObject projectileInstance = Instantiate(runeProjectilePrefab, transform.position, Quaternion.identity);
        RuneProjectile runeProjectile = projectileInstance.GetComponent<RuneProjectile>();

        if (runeProjectile != null)
        {
            // Pass PlayerStats to the projectile if it needs it (e.g., for exp gain)
            runeProjectile.playerStats = this.playerStats;
            // The projectile already has its mainAttribute set, but you could override if needed
            // runeProjectile.mainAttribute = this.mainAttribute;
        }
        else
        {
            Debug.LogError("Instantiated projectile does not have a RuneProjectile component.");
        }
    }
}

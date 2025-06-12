using UnityEngine;

public class RunesWeapon : MonoBehaviour, IAutoAttackWeapon
{
    public GameObject runeProjectilePrefab; // Assign the RuneProjectile prefab in the Inspector
    public float fireInterval = 2f; // Time between shots
    private float timer;
    public PlayerStats playerStats; // Assign the PlayerStats component in the Inspector or find it
    public AttributeType mainAttribute = AttributeType.Intelligence;
    public bool isEvolved = false;

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
            runeProjectile.playerStats = this.playerStats;
            // runeProjectile.mainAttribute = this.mainAttribute; // Already set on projectile prefab typically

            if (isEvolved)
            {
                // Enhance the projectile if the weapon is evolved
                runeProjectile.explosionRadius *= 1.5f; // Example: Increase explosion radius by 50%
                // runeProjectile.damage = Mathf.CeilToInt(runeProjectile.damage * 1.25f); // Example: Increase damage by 25%
                Debug.Log("RunesWeapon is evolved! Firing enhanced projectile.");
            }
        }
        else
        {
            Debug.LogError("Instantiated projectile does not have a RuneProjectile component.");
        }
    }

    public void Evolve()
    {
        if (!isEvolved)
        {
            isEvolved = true;
            Debug.Log($"{gameObject.name} has been evolved!");
            // Optional: You could also slightly increase stats here directly, e.g.
            // fireInterval *= 0.8f; // Faster firing
            // Or increase projectile damage/radius if the projectile script is easily accessible and modifiable
        }
        else
        {
            Debug.Log($"{gameObject.name} is already evolved.");
        }
    }
}

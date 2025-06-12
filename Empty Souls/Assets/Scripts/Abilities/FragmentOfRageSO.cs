using UnityEngine;

[CreateAssetMenu(fileName = "FragmentOfRage", menuName = "Abilities/Fragments/FragmentOfRage", order = 1)]
public class FragmentOfRageSO : BaseAbilitySO
{
    [Header("Fragment of Rage Specifics")]
    public float effectRadius = 5f;
    public int damageAmount = 10; // Example damage
    public float stunDuration = 0.5f; // Example stun
    public LayerMask enemyLayerMask; // To specify what to hit

    public override void Activate(PlayerStats playerStats)
    {
        Debug.Log($"'{abilityName}' (Fleshed Out) activated by {playerStats.gameObject.name} at position {playerStats.transform.position}!");

        // Example: Visual/Audio cue for activation (actual implementation would require AudioManager/EffectManager)
        // if (activationSound != null && AudioManager.Instance != null) AudioManager.Instance.PlaySound(activationSound);
        // if (activationEffectPrefab != null && EffectManager.Instance != null) EffectManager.Instance.SpawnEffect(activationEffectPrefab, playerStats.transform.position, Quaternion.identity);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerStats.transform.position, effectRadius, enemyLayerMask);

        if (hitEnemies.Length > 0)
        {
            Debug.Log($"Fragment of Rage hit {hitEnemies.Length} object(s) on the enemy layer within a {effectRadius}m radius.");
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                Debug.Log($"- Hit: {enemyCollider.gameObject.name}");
                EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    Debug.Log($"  Applying {damageAmount} damage and {stunDuration}s stun to {enemyCollider.gameObject.name}. (Placeholder for actual effect)");
                    // TODO: Implement actual damage application:
                    // enemyHealth.TakeDamage(damageAmount);
                    // TODO: Implement actual stun effect (e.g., call a Stun() method on an enemy script):
                    // var enemyMovement = enemyCollider.GetComponent<EnemyMovement>();
                    // if (enemyMovement != null) enemyMovement.ApplyStun(stunDuration);
                }
                else
                {
                    Debug.Log($"  {enemyCollider.gameObject.name} does not have an EnemyHealth component.");
                }
            }
        }
        else
        {
            Debug.Log("Fragment of Rage: No enemies found within the effect radius.");
        }

        // The user's original idea: "дающий 0.2% за каждого задетого врага"
        // This sounds like it might grant some resource or buff to the player.
        // For now, let's calculate this value and log it.
        if (hitEnemies.Length > 0)
        {
            float bonusPercentage = hitEnemies.Length * 0.002f; // 0.2% per enemy
            Debug.Log($"Fragment of Rage: Player gains a {bonusPercentage:P2} bonus (placeholder).");
            // TODO: Implement what this bonus does (e.g., heal player, increase stat, add to a special meter)
            // Example: playerStats.Heal(Mathf.CeilToInt(playerStats.maxHP * bonusPercentage));
        }
    }

    // Optional: Override OnEquip or OnLearn for specific setup if needed
    public override void OnEquip(PlayerStats playerStats)
    {
        base.OnEquip(playerStats); // Calls the base class logging
        // Add any specific logic when Fragment of Rage is equipped
        // For example, maybe it enables a UI element or changes a player property
    }
}

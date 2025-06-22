using UnityEngine;

[CreateAssetMenu(fileName = "FragmentOfRage", menuName = "Abilities/Fragments/FragmentOfRage", order = 1)]
public class FragmentOfRageSO : BaseAbilitySO
{
    [Header("Fragment of Rage Specifics")]
    public float effectRadius = 5f;
    public int damageAmount = 4; // Меньше урона
    public float stunDuration = 0.5f;
    public LayerMask enemyLayerMask;
    public GameObject roarEffectPrefab;

    [Header("Audio")]
    public AudioClip roarSfx;

    private void OnEnable()
    {
        cooldown = 7f; // Фиксированный КД
    }

    public override void Activate(PlayerStats playerStats)
    {
        // Звук
        if (roarSfx != null)
            AudioSource.PlayClipAtPoint(roarSfx, playerStats.transform.position);

        // Визуал
        if (roarEffectPrefab != null)
            GameObject.Instantiate(roarEffectPrefab, playerStats.transform.position, Quaternion.identity);

        Debug.Log($"'{abilityName}' activated by {playerStats.gameObject.name} at position {playerStats.transform.position}!");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerStats.transform.position, effectRadius, enemyLayerMask);

        if (hitEnemies.Length > 0)
        {
            Debug.Log($"Fragment of Rage hit {hitEnemies.Length} enemies.");
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                Debug.Log($"- Hit: {enemyCollider.gameObject.name}");
                EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    Debug.Log($"  Applying {damageAmount} damage and {stunDuration}s stun to {enemyCollider.gameObject.name}.");
                    enemyHealth.TakeDamage(Mathf.RoundToInt(playerStats.GetTotalDamage(damageAmount, AttributeType.Strength)));
                    // TODO: Реализовать стан, если нужно!
                    var enemyAI = enemyCollider.GetComponent<EnemyAI>();
                    if (enemyAI != null)
                    {
                        // enemyAI.ApplyStun(stunDuration); // Реализуй метод если нужно!
                    }
                }
            }
            float bonusPercentage = hitEnemies.Length * 0.002f; // 0.2% per enemy
            Debug.Log($"Fragment of Rage: Player heals {bonusPercentage:P2} от макс. хп.");
            playerStats.Heal(Mathf.CeilToInt(playerStats.maxHP * bonusPercentage));
        }
        else
        {
            Debug.Log("Fragment of Rage: No enemies found within the effect radius.");
        }
    }
}
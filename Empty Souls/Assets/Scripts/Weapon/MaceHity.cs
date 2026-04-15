using UnityEngine;

public class MaceHity : MonoBehaviour
{
    [SerializeField]
    private int damageAmount;

    [HideInInspector]
    public PlayerStats playerStats;
    [HideInInspector]
    public AttributeType mainAttribute = AttributeType.Strength;

    void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            if (playerStats != null)
                enemy.TakeDamage(Mathf.RoundToInt(playerStats.GetTotalDamage(damageAmount, mainAttribute)));
            else
                enemy.TakeDamage(damageAmount);
            playerStats?.AddWeaponExp(mainAttribute, 1);
        }
    }
}
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
            enemy.TakeDamage(damageAmount);
            if (playerStats != null)
                playerStats.AddWeaponExp(mainAttribute, 1);
        }
    }
}
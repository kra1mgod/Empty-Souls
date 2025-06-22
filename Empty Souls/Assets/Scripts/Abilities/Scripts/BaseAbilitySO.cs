using UnityEngine;

public abstract class BaseAbilitySO : ScriptableObject
{
    [Header("Base Ability Info")]
    public string abilityName = "New Ability";
    [TextArea(3, 5)]
    public string description = "Ability description.";
    public Sprite icon;
    public float cooldown = 1f;
    [HideInInspector] public float lastUseTime = -999f;
    public AbilityType type;

    public bool IsOffCooldown()
    {
        return Time.time >= lastUseTime + cooldown;
    }
    public bool TryActivate(PlayerStats playerStats)
    {
        float actualCooldown = cooldown;
        if (playerStats != null)
            actualCooldown *= playerStats.GetIntelligenceAbilityCooldownMultiplier();
        if (Time.time < lastUseTime + actualCooldown)
            return false;

        lastUseTime = Time.time;
        Activate(playerStats);
        return true;
    }

    public virtual void OnLearn(PlayerStats playerStats) { }
    public virtual void OnEquip(PlayerStats playerStats) { }
    public virtual void OnUnequip(PlayerStats playerStats) { }
    public abstract void Activate(PlayerStats playerStats);
    public virtual void ApplyPassiveEffects(GameObject player) { }
}
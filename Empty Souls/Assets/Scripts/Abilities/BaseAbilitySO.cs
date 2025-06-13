using UnityEngine;

public abstract class BaseAbilitySO : ScriptableObject
{
    [Header("Base Ability Info")]
    public string abilityName = "New Ability";
    [TextArea(3, 5)]
    public string description = "Ability description.";
    public Sprite icon;
    public float cooldown = 1f;
    // public float manaCost = 10f; // Example if you add a mana system

    // Called when the ability is learned by PlayerStats
    public virtual void OnLearn(PlayerStats playerStats)
    {
        Debug.Log($"{abilityName} learned by {playerStats.gameObject.name}.");
    }

    // Called when the ability is equipped to the active slot
    public virtual void OnEquip(PlayerStats playerStats)
    {
        Debug.Log($"{abilityName} equipped by {playerStats.gameObject.name}.");
    }

    // Called when the ability is unequipped from the active slot
    public virtual void OnUnequip(PlayerStats playerStats)
    {
        Debug.Log($"{abilityName} unequipped by {playerStats.gameObject.name}.");
    }

    // Abstract method to be implemented by each specific ability
    // PlayerStats is passed to allow abilities to interact with the player's state/position
    public abstract void Activate(PlayerStats playerStats);
}

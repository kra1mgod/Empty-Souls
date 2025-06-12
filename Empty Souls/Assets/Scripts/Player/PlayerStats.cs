using System.Collections.Generic;
using UnityEngine;

    public class PlayerStats : MonoBehaviour
{

    public AttributeStat strength = new AttributeStat { type = AttributeType.Strength };
    public AttributeStat agility = new AttributeStat { type = AttributeType.Agility };
    public AttributeStat intelligence = new AttributeStat { type = AttributeType.Intelligence };


    [Header("HP")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("Damage")]
    public float damage = 10f;

    [Header("Speed")]
    public float moveSpeed = 5f;

    [Header("Experience")]
    public int experience = 0;
    public int expToNextLevel = 100;
    public int level = 1;

    [Header("UI")]
    public AnimatedHPBar animatedHPBar;
    public GameObject deathPanel;

    public AutoWeaponManager autoWeaponManager;

    public delegate void OnHPChanged(int hp, int maxHP);
    public event OnHPChanged onHPChanged;

    [Header("Lumzvar Evolution")]
    public int currentLumzvarPoints = 0;
    public int baseLumzvarRequired = 50;
    public float lumzvarScalingFactor = 1.5f;
    public int evolutionCount = 0;
    public int lumzvarForNextEvolution;

    public delegate void LumzvarChangedDelegate(int current, int max);
    public event LumzvarChangedDelegate OnLumzvarChanged;

    [Header("Soul Fragments")] // Or keep under Lumzvar header if preferred
    public int soulFragments = 0;
    public event System.Action<int> OnSoulFragmentsChanged;

    [Header("Abilities")]
    public List<BaseAbilitySO> learnedAbilities = new List<BaseAbilitySO>();
    public BaseAbilitySO activeAbility;


    private Dictionary<string, GameObject> weapons = new Dictionary<string, GameObject>();
    private bool isDead = false;
    private LumzvarBar lumzvarBar;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<IAutoAttackWeapon>() != null)
                weapons[child.name] = child.gameObject;
            else if (child.GetComponent<MaceWeapon>() != null)
                weapons[child.name] = child.gameObject;
            child.gameObject.SetActive(false);
        }
        if (!autoWeaponManager)
            autoWeaponManager = GetComponent<AutoWeaponManager>();
    }
        public void AddWeaponExp(AttributeType main, int amount)
        {
            switch (main)
            {
                case AttributeType.Strength:
                    strength.AddExp(amount * 3);
                    agility.AddExp(amount);
                    intelligence.AddExp(amount);
                    break;
                case AttributeType.Agility:
                    strength.AddExp(amount);
                    agility.AddExp(amount * 3);
                    intelligence.AddExp(amount);
                    break;
                case AttributeType.Intelligence:
                    strength.AddExp(amount);
                    agility.AddExp(amount);
                    intelligence.AddExp(amount * 3);
                    break;
            }
        }
        void Start()
    {
        currentHP = maxHP;
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
        onHPChanged?.Invoke(currentHP, maxHP);
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }
    public Vector2 lastMoveDirection { get; private set; } = Vector2.right; // по умолчанию вправо

    public void SetMoveDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = dir.normalized;
        }
    }

    public void LearnAbility(BaseAbilitySO ability)
    {
        if (ability == null) return;
        if (!learnedAbilities.Contains(ability))
        {
            learnedAbilities.Add(ability);
            Debug.Log($"Learned ability: {ability.abilityName}");
            // Optionally, auto-equip if it's the first one or an active type
            if (activeAbility == null && ability.type == AbilityType.Active)
            {
                EquipAbility(ability);
            }
            // Apply passive effects immediately upon learning
            if (ability.type == AbilityType.Passive)
            {
                ability.ApplyPassiveEffects(gameObject);
            }
        }
    }

    public void EquipAbility(BaseAbilitySO ability)
    {
        if (ability == null || !learnedAbilities.Contains(ability))
        {
            Debug.LogWarning($"Cannot equip ability: {(ability != null ? ability.abilityName : "null")}. Not learned or invalid.");
            return;
        }

        if (ability.type == AbilityType.Active)
        {
            activeAbility = ability;
            Debug.Log($"Equipped active ability: {ability.abilityName}");
        }
        else
        {
            Debug.LogWarning($"Cannot equip ability: {ability.abilityName}. It is not an Active type ability.");
        }
    }

    public void UseActiveAbility()
    {
        if (activeAbility != null)
        {
            // TODO: Add cooldown check here if BaseAbilitySO tracks last use time
            Debug.Log($"Attempting to use active ability: {activeAbility.abilityName}");
            activeAbility.Activate(gameObject);
        }
        else
        {
            Debug.Log("No active ability equipped.");
        }
    }

    public Vector2 GetMoveDirection()
    {
        return lastMoveDirection;
    }
    public void AddWeapon(string weaponName)
    {
        if (weapons.ContainsKey(weaponName))
        {
            GameObject weapon = weapons[weaponName];

            var autoAttack = weapon.GetComponent<IAutoAttackWeapon>();
            if (autoAttack != null)
            {
                autoWeaponManager.AddAutoWeapon(autoAttack);
            }
            else if (weapon.GetComponent<MaceWeapon>() != null)
            {
                weapon.SetActive(true);
            }
            Debug.Log("Оружие выдано: " + weaponName);
        }
        else
        {
            Debug.LogWarning("Оружие с именем " + weaponName + " не найдено!");
        }
    }

    public void GiveBaseWeapon(string weaponName)
    {
        AddWeapon(weaponName);
    }

    public float GetDamage() => damage;
    public void SetMoveSpeed(float newSpeed) => moveSpeed = newSpeed;

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        onHPChanged?.Invoke(currentHP, maxHP);
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
        if (currentHP <= 0) Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        onHPChanged?.Invoke(currentHP, maxHP);
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
    }

    public void AddExperience(int amount)
    {
        if (isDead) return;
        experience += amount;
        while (experience >= expToNextLevel)
        {
            experience -= expToNextLevel;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        LevelUpUI.Instance.ShowUpgradeChoices();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Игрок погиб!");
        Time.timeScale = 0f;
        if (deathPanel != null)
            deathPanel.SetActive(true);
    }

    public int GetCurrentLumzvar()
    {
        return currentLumzvarPoints;
    }

    public int GetLumzvarForNextEvolution()
    {
        return lumzvarForNextEvolution;
    }

    private int CalculateLumzvarForNextEvolution()
    {
        return Mathf.FloorToInt(baseLumzvarRequired * Mathf.Pow(lumzvarScalingFactor, evolutionCount));
    }

    public void AddLumzvar(int amount)
    {
        if (isDead) return;
        currentLumzvarPoints += amount;
        if (currentLumzvarPoints >= lumzvarForNextEvolution)
        {
            currentLumzvarPoints -= lumzvarForNextEvolution; // Leftover points carry over
            evolutionCount++;
            int previousLumzvarRequired = lumzvarForNextEvolution; // Store for logging
            lumzvarForNextEvolution = CalculateLumzvarForNextEvolution();
            Debug.Log($"Evolution criteria met! Evolution count: {evolutionCount}. {currentLumzvarPoints} Lumzvar carried over. Next evolution needs {lumzvarForNextEvolution} Lumzvar (previously {previousLumzvarRequired}).");
            InitiateWeaponEvolutionChoice(); // New method call
        }
        UpdateLumzvarUI();
    }

    public void AddSoulFragments(int amount)
    {
        if (isDead) return;
        soulFragments += amount;
        OnSoulFragmentsChanged?.Invoke(soulFragments);
        // Debug.Log($"Collected {amount} Soul Fragments. Total: {soulFragments}");
    }

    private void UpdateLumzvarUI()
    {
        OnLumzvarChanged?.Invoke(currentLumzvarPoints, lumzvarForNextEvolution);
        if (lumzvarBar != null)
        {
            lumzvarBar.UpdateBar(currentLumzvarPoints, lumzvarForNextEvolution);
        }
    }

    private void InitiateWeaponEvolutionChoice()
    {
        Debug.Log("Player has earned a weapon evolution! Displaying evolution choices...");
        // Placeholder: In a real implementation, this would open a UI screen.
        // For now, let's simulate choosing to evolve the "Runes" weapon as an example.
        // This choice would normally come from player input via the UI.

        string chosenWeaponToEvolve = "RunesWeapon"; // Example: player chooses Runes
        EvolveWeapon(chosenWeaponToEvolve);

        // Actual evolution UI and choice mechanism will be implemented in a later step.
        // For now, we just log that the point for choice has been reached.
        // The LevelUpUI.Instance.ShowUpgradeChoices(); from the original LevelUp might be a good place to adapt later.
        if (LevelUpUI.Instance != null)
        {
            // We can potentially reuse or adapt the LevelUpUI for weapon evolution choices.
            // For this initial phase, we won't trigger it directly to avoid complexity,
            // but it's a good candidate for future integration.
            Debug.Log("Weapon Evolution: Consider adapting LevelUpUI or creating a new UI for weapon evolution choices.");
        }
        else
        {
            Debug.LogWarning("Weapon Evolution: LevelUpUI instance not found. Cannot suggest UI adaptation.");
        }
    }

    // Placeholder for actual weapon evolution logic
    public void EvolveWeapon(string weaponName)
    {
        if (weapons.TryGetValue(weaponName, out GameObject weaponInstance))
        {
            // Attempt to get the RunesWeapon component
            var runesWeapon = weaponInstance.GetComponent<RunesWeapon>();
            if (runesWeapon != null)
            {
                runesWeapon.Evolve(); // Call Evolve method on RunesWeapon script
                Debug.Log($"PlayerStats: Weapon '{weaponName}' (Runes) has been evolved via EvolveWeapon method.");
                return; // Exit after handling
            }

            // Placeholder for Katana evolution
            // var katanaWeapon = weaponInstance.GetComponent<KatanaWeapon>();
            // if (katanaWeapon != null)
            // {
            //     katanaWeapon.Evolve(); // Assuming KatanaWeapon will have an Evolve() method
            //     Debug.Log($"PlayerStats: Weapon '{weaponName}' (Katana) has been evolved.");
            //     return;
            // }

            // Placeholder for Mace evolution
            // var maceWeapon = weaponInstance.GetComponent<MaceWeapon>();
            // if (maceWeapon != null)
            // {
            //     maceWeapon.Evolve(); // Assuming MaceWeapon will have an Evolve() method
            //     Debug.Log($"PlayerStats: Weapon '{weaponName}' (Mace) has been evolved.");
            //     return;
            // }

            Debug.LogWarning($"PlayerStats: Weapon '{weaponName}' found, but does not have a recognized evolution script component (e.g., RunesWeapon).");
        }
        else
        {
            Debug.LogWarning($"PlayerStats: Attempted to evolve weapon '{weaponName}', but it's not found in the player's active/known weapons dictionary.");
        }
    }
}

public enum AttributeType { Strength, Agility, Intelligence }
[System.Serializable]
public class AttributeStat
{
    public AttributeType type;
    public int level = 1;
    public int exp = 0;
    public int expToLevel = 2;

    public void AddExp(int amount)
    {
        exp += amount;
        while (exp >= expToLevel)
        {
            exp -= expToLevel;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        expToLevel = Mathf.CeilToInt(expToLevel * 1.25f); // Прогрессия
        Debug.Log($"{type} leveled up! New level: {level}");
    }
}


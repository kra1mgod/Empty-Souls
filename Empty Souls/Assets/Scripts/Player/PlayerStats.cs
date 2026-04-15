using System;
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

    [Header("Урон")]
    public float baseDamage = 10f;
    public float damageBonusPercent = 0f;

    [Header("Speed")]
    public float moveSpeed = 20f;

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

    [Header("Soul Fragments")]
    public int soulFragments = 0;
    public event System.Action<int> OnSoulFragmentsChanged;

    [Header("Abilities")]
    public List<BaseAbilitySO> learnedAbilities = new List<BaseAbilitySO>();
    public BaseAbilitySO activeAbility;

    private Dictionary<string, GameObject> weapons = new Dictionary<string, GameObject>();
    private bool isDead = false;

    public Vector2 lastMoveDirection { get; private set; } = Vector2.right;

    [Header("User Settings")]
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambientVolume = 1f;
    public bool autoSaveEnabled = true;
    public string difficultyLevel = "Normal";

    private float playTimeTimer = 0f;

    public DeathPanelUI deathPanelUI;
    public GameEndPanel gameEndPanel;

    public event Action OnPlayerDeath;
    private float abilityCooldownTimer = 0f;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<IAutoAttackWeapon>() != null)
            {
                weapons[child.name] = child.gameObject;
            }
            else if (child.GetComponent<MaceWeapon>() != null)
            {
                weapons[child.name] = child.gameObject;
            }
            child.gameObject.SetActive(false);
            if (child.GetComponent<ChestSpawner>() != null)
                child.gameObject.SetActive(true);
        }
        if (!autoWeaponManager)
            autoWeaponManager = GetComponent<AutoWeaponManager>();

        lumzvarForNextEvolution = CalculateLumzvarForNextEvolution();
        ApplyProfileFromSaveManager();
    }

    void Start()
    {
        currentHP = maxHP;
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
        onHPChanged?.Invoke(currentHP, maxHP);
        if (deathPanel != null)
            deathPanel.SetActive(false);
        if (gameEndPanel != null && gameEndPanel.panelRoot != null)
            gameEndPanel.panelRoot.SetActive(false);

        ApplyProfileFromSaveManager();
        lumzvarForNextEvolution = CalculateLumzvarForNextEvolution();
        UpdateLumzvarUI();
        Debug.Log($"[PlayerStats] UpdateLumzvarUI вызван: {currentLumzvarPoints} / {lumzvarForNextEvolution}");
        if (GameData.selectedFragment != null)
        {
            LearnAbility(GameData.selectedFragment);
            EquipAbility(GameData.selectedFragment);
        }
    }

    void Update()
    {
        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile != null && !isDead)
        {
            playTimeTimer += Time.unscaledDeltaTime;
            if (playTimeTimer >= 1f)
            {
                int secondsToAdd = Mathf.FloorToInt(playTimeTimer);
                profile.gameStatistics.totalPlayTimeSec += secondsToAdd;
                playTimeTimer -= secondsToAdd;
                SaveProfile();
            }
        }
        if (abilityCooldownTimer > 0f)
            abilityCooldownTimer -= Time.unscaledDeltaTime;
    }

    public void ApplyProfileFromSaveManager()
    {
        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile == null) return;

        level = profile.gameStatistics.bestLevel;
        evolutionCount = profile.gameStatistics.evolutionCount;
        soulFragments = profile.resources.totalSoulFragments;

        if (profile.progression != null)
        {
            foreach (var weapon in weapons)
                weapon.Value.SetActive(false);
            foreach (var w in profile.progression.weaponProgress.Keys)
            {
                if (weapons.ContainsKey(w))
                    weapons[w].SetActive(true);
            }

            learnedAbilities.Clear();
            foreach (var abName in profile.progression.unlockedAbilities)
            {
                var ab = AbilityDatabase.GetAbilityByName(abName);
                if (ab != null) learnedAbilities.Add(ab);
            }

            if (!string.IsNullOrEmpty(profile.currentSelection.selectedFragment))
                activeAbility = AbilityDatabase.GetAbilityByName(profile.currentSelection.selectedFragment);
            else
                activeAbility = null;
        }

        if (profile.userSettings != null)
        {
            musicVolume = profile.userSettings.audioSettings.musicVolume;
            sfxVolume = profile.userSettings.audioSettings.sfxVolume;
            ambientVolume = profile.userSettings.audioSettings.ambientVolume;
            autoSaveEnabled = profile.userSettings.gameplaySettings.autoSaveEnabled;
            difficultyLevel = profile.userSettings.gameplaySettings.difficultyLevel;
        }
    }

    public void ExtractToProfile()
    {
        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile == null) return;

        profile.gameStatistics.bestLevel = Mathf.Max(profile.gameStatistics.bestLevel, level);
        profile.gameStatistics.evolutionCount = evolutionCount;
        profile.resources.totalSoulFragments = soulFragments;

        if (profile.progression != null)
        {
            profile.progression.unlockedAbilities = new List<string>();
            foreach (var ab in learnedAbilities)
                if (ab != null)
                    profile.progression.unlockedAbilities.Add(ab.abilityName);

            profile.currentSelection.selectedFragment = activeAbility != null ? activeAbility.abilityName : "";

            profile.progression.weaponProgress = new Dictionary<string, WeaponProgress>();
            foreach (var kv in weapons)
            {
                if (kv.Value.activeSelf)
                {
                    profile.progression.weaponProgress[kv.Key] = new WeaponProgress
                    {
                        level = 1,
                        evolved = false,
                        experiencePoints = 0
                    };
                }
            }
        }

        if (profile.userSettings == null)
            profile.userSettings = new UserSettings();
        profile.userSettings.audioSettings.musicVolume = musicVolume;
        profile.userSettings.audioSettings.sfxVolume = sfxVolume;
        profile.userSettings.audioSettings.ambientVolume = ambientVolume;
        profile.userSettings.gameplaySettings.autoSaveEnabled = autoSaveEnabled;
        profile.userSettings.gameplaySettings.difficultyLevel = difficultyLevel;
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
        SaveProfile();
    }

    public void AddBonusHP(int value)
    {
        maxHP += value;
        currentHP += value;
        animatedHPBar?.SetHP(currentHP, maxHP);
        SaveProfile();
    }

    public void LearnAbility(BaseAbilitySO ability)
    {
        if (ability == null) return;
        if (!learnedAbilities.Contains(ability))
        {
            learnedAbilities.Add(ability);
            if (activeAbility == null && ability.type == AbilityType.Active)
            {
                EquipAbility(ability);
            }
            if (ability.type == AbilityType.Passive)
            {
                ability.ApplyPassiveEffects(gameObject);
            }
            SaveProfile();
        }
    }

    public void EquipAbility(BaseAbilitySO ability)
    {
        if (ability == null || !learnedAbilities.Contains(ability))
            return;
        if (ability.type == AbilityType.Active)
        {
            activeAbility = ability;
            SaveProfile();
        }
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
            SaveProfile();
        }
    }

    public void GiveBaseWeapon(string weaponName)
    {
        AddWeapon(weaponName);
    }

    public float GetTotalDamage(float baseWeaponDamage, AttributeType mainAttribute)
    {
        float statBonus = 0f;
        switch (mainAttribute)
        {
            case AttributeType.Strength:
                statBonus = strength.level * 0.5f;
                break;
            case AttributeType.Agility:
                statBonus = agility.level * 0.5f;
                break;
            case AttributeType.Intelligence:
                statBonus = intelligence.level * 0.5f;
                break;
        }
        float total = baseWeaponDamage + baseDamage + statBonus;
        return total * (1f + damageBonusPercent);
    }

    public float GetPlayerBaseDamage() => baseDamage;
    public void AddBonusDamage(float value) { baseDamage += value; SaveProfile(); }
    public void AddDamagePercent(float percent) { damageBonusPercent += percent; SaveProfile(); }

    public float GetDamage() => GetPlayerBaseDamage();
    public void SetMoveSpeed(float newSpeed) { moveSpeed = newSpeed; SaveProfile(); }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        onHPChanged?.Invoke(currentHP, maxHP);
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
        if (currentHP <= 0) Die();
        SaveProfile();
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        onHPChanged?.Invoke(currentHP, maxHP);
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
        SaveProfile();
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
        SaveProfile();
    }

    void LevelUp()
    {
        level++;
        LevelUpUI.Instance?.ShowUpgradeChoices();
        SaveProfile();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (gameEndPanel != null)
        {
            gameEndPanel.ShowDefeat();
        }
        else if (deathPanel != null)
        {
            Time.timeScale = 0f;
            deathPanel.SetActive(true);
        }

        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile != null)
            profile.gameStatistics.totalDeaths++;

        OnPlayerDeath?.Invoke();

        SaveProfile();
    }

    public int GetCurrentLumzvar() => currentLumzvarPoints;
    public int GetLumzvarForNextEvolution() => lumzvarForNextEvolution;

    private int CalculateLumzvarForNextEvolution()
    {
        return Mathf.FloorToInt(baseLumzvarRequired * Mathf.Pow(lumzvarScalingFactor, evolutionCount));
    }

    public void AddLumzvar(int amount)
    {
        if (isDead) return;

        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile != null)
            profile.resources.totalLumzvar += amount;

        currentLumzvarPoints += amount;
        Debug.Log($"[AddLumzvar] Now: {currentLumzvarPoints} / {lumzvarForNextEvolution}");

        bool evolved = false;
        while (currentLumzvarPoints >= lumzvarForNextEvolution)
        {
            currentLumzvarPoints -= lumzvarForNextEvolution;
            evolutionCount++;
            lumzvarForNextEvolution = CalculateLumzvarForNextEvolution();
            evolved = true;
        }
        UpdateLumzvarUI();
        if (evolved)
        {
            MonoBehaviour chosenWeapon = null;
            foreach (var weaponObj in weapons.Values)
            {
                if (weaponObj.activeInHierarchy)
                {
                    chosenWeapon = weaponObj.GetComponent<MonoBehaviour>();
                    if (chosenWeapon != null) break;
                }
            }
            if (chosenWeapon == null && weapons.Count > 0)
            {
                foreach (var weaponObj in weapons.Values)
                {
                    chosenWeapon = weaponObj.GetComponent<MonoBehaviour>();
                    if (chosenWeapon != null) break;
                }
            }

            if (EvolutionManager.Instance != null && chosenWeapon != null)
            {
                EvolutionManager.Instance.ShowEvolutionOptions(chosenWeapon);
            }
        }
        SaveProfile();
    }

    public void AddSoulFragments(int amount)
    {
        if (isDead) return;
        soulFragments += amount;

        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile != null)
            profile.resources.totalSoulFragments = soulFragments;

        OnSoulFragmentsChanged?.Invoke(soulFragments);
        SaveProfile();
    }

    private void UpdateLumzvarUI()
    {
        Debug.Log($"[PlayerStats] UpdateLumzvarUI: подписчиков {OnLumzvarChanged?.GetInvocationList().Length ?? 0}");
        OnLumzvarChanged?.Invoke(currentLumzvarPoints, lumzvarForNextEvolution);
    }

    public float GetAgilityProjectileSpeedBonus()
    {
        return 1f + agility.level * 0.02f;
    }
    public float GetAgilityWeaponDamageBonus(AttributeType weaponType)
    {
        if (weaponType == AttributeType.Agility)
            return agility.level * 0.5f;
        return 0f;
    }

    public float GetStrengthWeaponDamageBonus(AttributeType weaponType)
    {
        if (weaponType == AttributeType.Strength)
            return strength.level * 0.5f;
        return 0f;
    }

    public float GetStrengthTouchDamageBonus()
    {
        return strength.level * 2f;
    }

    public float GetIntelligenceAbilityCooldownMultiplier()
    {
        return Mathf.Max(0.2f, 1f - intelligence.level * 0.01f);
    }

    public Vector2 GetMoveDirection()
    {
        return lastMoveDirection;
    }

    public void SetMoveDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = dir.normalized;
        }
    }

    public void UseActiveAbility()
    {
        if (activeAbility != null)
        {
            float abilityBaseCooldown = activeAbility.cooldown;
            float cooldownMultiplier = GetIntelligenceAbilityCooldownMultiplier();
            float finalCooldown = abilityBaseCooldown * cooldownMultiplier;

            if (abilityCooldownTimer > 0f)
            {
                Debug.Log($"Ability on cooldown! {abilityCooldownTimer:F1} сек.");
                return;
            }

            activeAbility.Activate(this);
            abilityCooldownTimer = finalCooldown;
            Debug.Log($"Ability used! Next use in {finalCooldown:F1} сек.");
        }
        else
        {
            Debug.Log("No active ability equipped.");
        }
    }

    private void SaveProfile()
    {
        ExtractToProfile();
        _ = SaveManager.Instance.SaveAsync();
    }
}

public enum AttributeType { Strength, Agility, Intelligence }

[Serializable]
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
        expToLevel = Mathf.CeilToInt(expToLevel * 1.25f);
        Debug.Log($"{type} leveled up! New level: {level}");
    }
}

public static class AbilityDatabase
{
    private static Dictionary<string, BaseAbilitySO> abilityDict;
    public static BaseAbilitySO GetAbilityByName(string name)
    {
        if (abilityDict == null)
        {
            abilityDict = new Dictionary<string, BaseAbilitySO>();
            var allAbilities = UnityEngine.Resources.LoadAll<BaseAbilitySO>("Abilities");
            foreach (var ab in allAbilities)
                if (!abilityDict.ContainsKey(ab.abilityName))
                    abilityDict.Add(ab.abilityName, ab);
        }
        if (abilityDict.ContainsKey(name))
            return abilityDict[name];
        return null;
    }
}
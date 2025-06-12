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

    private Dictionary<string, GameObject> weapons = new Dictionary<string, GameObject>();
    private bool isDead = false;

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


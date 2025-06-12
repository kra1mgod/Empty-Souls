using UnityEngine;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;

    public List<UpgradeOption> allUpgrades;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public List<UpgradeOption> GetRandomOptions(int count)
    {
        var options = new List<UpgradeOption>(allUpgrades);
        var result = new List<UpgradeOption>();
        for (int i = 0; i < count && options.Count > 0; i++)
        {
            int idx = Random.Range(0, options.Count);
            result.Add(options[idx]);
            options.RemoveAt(idx);
        }
        return result;
    }

    public void ApplyUpgrade(UpgradeOption option)
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.LogError("PlayerStats not found!");
            return;
        }

        switch (option.type)
        {
            case UpgradeType.MaxHP:
                player.maxHP += Mathf.RoundToInt(option.value);
                player.currentHP += Mathf.RoundToInt(option.value); // чтобы сразу восстановить хп при увеличении максимума
                player.animatedHPBar.SetHP(player.currentHP, player.maxHP);
                break;
            case UpgradeType.Damage:
                player.damage += option.value;
                break;
            case UpgradeType.MoveSpeed:
                player.moveSpeed += option.value;
                break;
            case UpgradeType.NewWeapon:
                player.AddWeapon(option.name); // реализуй метод AddWeapon в PlayerStats или отдельном скрипте
                break;
                // добавь другие типы по необходимости
        }
    }
}
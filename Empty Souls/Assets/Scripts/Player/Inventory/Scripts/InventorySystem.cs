using UnityEngine;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    public List<ItemData> items = new List<ItemData>();

    [Header("Настройки дропа")]
    public int experienceOnDuplicate = 20; // Сколько опыта давать за повторку (можно сделать публичным)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        var player = FindObjectOfType<PlayerStats>();

        if (items.Contains(item))
        {
            Debug.LogWarning("Этот предмет уже есть в инвентаре! Вместо него даём опыт.");
            if (player != null)
            {
                int exp = experienceOnDuplicate;
                // Можно сделать exp = item.duplicateExpReward; если есть такое поле
                player.AddExperience(exp);
                // Можно показать всплывающее сообщение, эффект и т.д.
            }
            return;
        }

        items.Add(item);

        // Применить бонусы
        if (player != null)
        {
            if (item.bonusHP != 0)
            {
                player.maxHP += item.bonusHP;
                player.currentHP += item.bonusHP;
                player.animatedHPBar?.SetHP(player.currentHP, player.maxHP);
            }
            if (item.bonusDamage != 0)
            {
                player.AddBonusDamage(item.bonusDamage);
            }
            // Добавь другие бонусы тут
        }

        InventoryUI.Instance?.AddItem(item);
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}
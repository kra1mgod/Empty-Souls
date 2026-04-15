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
                player.currentHP += Mathf.RoundToInt(option.value);
                player.animatedHPBar.SetHP(player.currentHP, player.maxHP);
                break;
            case UpgradeType.Damage:
                player.AddBonusDamage(option.value);
                break;
            case UpgradeType.MoveSpeed:
                player.moveSpeed += option.value;
                break;
            case UpgradeType.SizeUp:
                player.transform.localScale += new Vector3(option.value, option.value, 0);
                break;
            case UpgradeType.SizeDown:
                player.transform.localScale -= new Vector3(option.value, option.value, 0);
                break;
            case UpgradeType.AttackSpeed:
                // Пример: ускорить все авто-оружия
                var weapons = player.GetComponentsInChildren<IAutoAttackWeapon>();
                foreach (var weapon in weapons)
                {
                    var runesWeapon = weapon as RunesWeapon;
                    if (runesWeapon != null)
                        runesWeapon.fireInterval *= (1 - option.value); // value = 0.1f → ускорение на 10%
                    var katanaWeapon = weapon as KatanaWeapon;
                    if (katanaWeapon != null)
                        katanaWeapon.fireInterval *= (1 - option.value);
                    // Аналогично для других оружий
                }
                break;
            case UpgradeType.NewWeapon:
                player.AddWeapon(option.name);
                break;
                // можно добавить и другие типы
        }
    }
}
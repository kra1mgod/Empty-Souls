using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject panel;
    public Transform weaponListParent, statListParent;
    public GameObject weaponItemPrefab, statItemPrefab;
    List<UpgradeOption> weapons = new List<UpgradeOption>();
    List<UpgradeOption> stats = new List<UpgradeOption>();

    void Awake() => Instance = this;

    public void AddWeapon(UpgradeOption weapon)
    {
        weapons.Add(weapon);
        // Обновить UI
    }
    public void AddStat(UpgradeOption stat)
    {
        stats.Add(stat);
        // Обновить UI
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        // Очистить и заполнить weaponListParent, statListParent
    }

    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
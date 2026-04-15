using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject panel;
    public Transform weaponListParent, statListParent;

    [Header("Пассивные предметы")]
    public Transform[] passiveItemSlots;        // Ячейки в Canvas (Slot1, Slot2, ...)
    public GameObject itemPrefab;               // Префаб содержимого слота (иконка+текст)

    private List<ItemData> items = new List<ItemData>();
    private List<UpgradeOption> weapons = new List<UpgradeOption>();
    private List<UpgradeOption> stats = new List<UpgradeOption>();

    void Awake()
    {
        Instance = this;
    }

    public void AddWeapon(UpgradeOption weapon)
    {
        weapons.Add(weapon);
        RefreshUI();
    }

    public void AddStat(UpgradeOption stat)
    {
        stats.Add(stat);
        RefreshUI();
    }

    public void AddPassiveItem(ItemData item)
    {
        if (items.Count < passiveItemSlots.Length)
        {
            items.Add(item);
            RefreshUI();
        }
        else
        {
            Debug.LogWarning("Нет свободных слотов для пассивных предметов!");
        }
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        RefreshUI();
    }

    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void AddItem(ItemData item)
    {
        if (items.Count < passiveItemSlots.Length)
        {
            items.Add(item);
            RefreshUI();
        }
        else
        {
            Debug.LogWarning("Нет свободных слотов для пассивных предметов!");
        }
    }

    private void RefreshUI()
    {
        Debug.Log("Обновляем UI, предметов в списке: " + items.Count);
        for (int i = 0; i < passiveItemSlots.Length; i++)
        {
            var slot = passiveItemSlots[i];
            var icon = slot.Find("Canvas/Icon")?.GetComponent<Image>();
            var nameText = slot.Find("Canvas/Name")?.GetComponent<TextMeshProUGUI>();

            if (i < items.Count)
            {
                if (icon != null && items[i].icon != null)
                    icon.sprite = items[i].icon;
                if (nameText != null)
                    nameText.text = items[i].displayName;

                slot.gameObject.SetActive(true);
            }
            else
            {
                if (icon != null)
                    icon.sprite = null;
                if (nameText != null)
                    nameText.text = "";
                slot.gameObject.SetActive(true); // или оставь true, если хочешь видеть пустые ячейки
            }
        }
    }
}
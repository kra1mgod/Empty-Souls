using UnityEngine;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;

    public List<UpgradeOption> allUpgrades; // Заполняешь в инспекторе

    void Awake() => Instance = this;

    public List<UpgradeOption> GetRandomOptions(int count)
    {
        // Возвращает случайные count штук из всех апгрейдов
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
        // Здесь твоя логика применения выбранного улучшения
        Debug.Log("Upgrade: " + option.name);
    }
}
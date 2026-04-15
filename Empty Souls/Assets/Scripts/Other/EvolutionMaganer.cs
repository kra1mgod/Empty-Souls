using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public static EvolutionManager Instance;

    [System.Serializable]
    public class WeaponEvolutionList
    {
        public string weaponName;
        public string weaponScriptTypeName; // Например, "RunesWeapon"
        public List<BaseEvolutionSO> evolutionOptions;
    }

    public List<WeaponEvolutionList> allWeaponEvolutions;
    public EvolutionPanelUI panelUI;

    void Awake() => Instance = this;

    public void ShowEvolutionOptions(MonoBehaviour weapon)
    {
        var evolvable = weapon as IEvolvableWeapon;
        if (evolvable != null && evolvable.IsEvolved)
        {
            Debug.Log($"Weapon {weapon.name} уже эволюционировало! Эволюция невозможна.");
            return;
        }

        var weaponType = weapon.GetType();
        var list = allWeaponEvolutions.Find(w =>
            !string.IsNullOrEmpty(w.weaponScriptTypeName) &&
            w.weaponScriptTypeName == weaponType.Name
        );
        if (list == null || list.evolutionOptions.Count == 0)
        {
            Debug.LogWarning($"No evolution options for weapon: {weaponType.Name}");
            return;
        }
        var options = GetRandomOptions(list.evolutionOptions, 3);

        panelUI.Show(options, selected =>
        {
            if (selected is BaseEvolutionSO so)
                so.ApplyToWeapon(weapon);
        });
    }

    private List<BaseEvolutionSO> GetRandomOptions(List<BaseEvolutionSO> src, int count)
    {
        var copy = new List<BaseEvolutionSO>(src);
        var result = new List<BaseEvolutionSO>();
        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int idx = Random.Range(0, copy.Count);
            result.Add(copy[idx]);
            copy.RemoveAt(idx);
        }
        return result;
    }
}
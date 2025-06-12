using UnityEngine;
using System.Collections.Generic;

public class AutoWeaponManager : MonoBehaviour
{
    private readonly List<IAutoAttackWeapon> autoWeapons = new List<IAutoAttackWeapon>();

    public void AddAutoWeapon(IAutoAttackWeapon weapon)
    {
        if (weapon != null && !autoWeapons.Contains(weapon))
        {
            autoWeapons.Add(weapon);
            MonoBehaviour mono = weapon as MonoBehaviour;
            if (mono != null)
                mono.gameObject.SetActive(true);
        }
    }

    public void RemoveAutoWeapon(IAutoAttackWeapon weapon)
    {
        if (weapon != null && autoWeapons.Contains(weapon))
        {
            autoWeapons.Remove(weapon);
            MonoBehaviour mono = weapon as MonoBehaviour;
            if (mono != null)
                mono.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (var weapon in autoWeapons)
        {
            weapon.TickUpdate();
        }
    }
}
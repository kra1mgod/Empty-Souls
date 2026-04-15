using System.Collections.Generic;
using UnityEngine;

public static class EvolutionOptionDatabase
{
    public static List<IWeaponEvolutionOption> GetRandomKatanaOptions(int count)
    {
        var all = new List<IWeaponEvolutionOption>() {
            new KatanaEvolutionOption(
                KatanaEvolutionOption.KatanaEvoType.TripleWave,
                "Тройная волна",
                "Катана выпускает три волны одновременно"),
            new KatanaEvolutionOption(
                KatanaEvolutionOption.KatanaEvoType.PowerWave,
                "Мощная волна",
                "Волна наносит больше урона, но меньше по размеру"),
            new KatanaEvolutionOption(
                KatanaEvolutionOption.KatanaEvoType.BigShortWave,
                "Большая короткая волна",
                "Короткая волна больше и сильнее, но дальность меньше")
        };
        // Здесь можно добавить рандомизацию, если нужно
        return all.GetRange(0, Mathf.Min(count, all.Count));
    }
}
using UnityEngine;

public interface IWeaponEvolutionOption
{
    string Title { get; }
    string Description { get; }
    Sprite Icon { get; }
    void Apply(MonoBehaviour weapon); // weapon может быть KatanaWeapon, RunesWeapon и т.д.
}


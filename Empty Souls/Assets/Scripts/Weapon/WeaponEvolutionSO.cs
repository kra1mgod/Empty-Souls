using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Evolution Option")]
public class WeaponEvolutionSO : ScriptableObject, IWeaponEvolutionOption
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    public string Title => title;
    public string Description => description;
    public Sprite Icon => icon;

    public virtual void Apply(MonoBehaviour weapon)
    {
        Debug.Log($"Evolution '{title}' applied to {weapon.name} via SO!");
        // Реализуй конкретный эффект здесь или через наследование от этого SO
    }
}
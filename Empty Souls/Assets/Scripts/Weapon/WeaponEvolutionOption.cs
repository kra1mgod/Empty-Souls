using UnityEngine;

[System.Serializable]
public class WeaponEvolutionOption : IWeaponEvolutionOption
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    public string Title => title;
    public string Description => description;
    public Sprite Icon => icon;

    public virtual void Apply(MonoBehaviour weapon)
    {
        Debug.Log($"Evolution '{title}' applied to {weapon.name}");
        // По умолчанию ничего не делает — реализуй в наследниках или через ScriptableObject
    }
}
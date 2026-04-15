using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Evolution Option")]
public class BaseEvolutionSO : ScriptableObject
{
    [Header("Base Evolution Info")]
    public string evolutionName = "New Evolution";
    [TextArea(3, 5)]
    public string description = "Evolution description.";
    public Sprite icon;

    public virtual void ApplyToWeapon(MonoBehaviour weapon)
    {
        Debug.Log($"Evolution '{evolutionName}' applied to {weapon.name} via SO!");
        // Тут твой эффект эволюции
        //EvolutionLock.EvolutionChosen = true; // --- Блокируем дальнейший выбор ---
    }
}
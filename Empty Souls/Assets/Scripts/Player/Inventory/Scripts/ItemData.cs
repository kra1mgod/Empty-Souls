using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive Item")]
public class ItemData : ScriptableObject
{
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
    // Пример: + к стату
    public int bonusHP;
    public float bonusDamage;
    // Можно добавить свои поля для любых эффектов
}
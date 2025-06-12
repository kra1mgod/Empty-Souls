[System.Serializable]
public class UpgradeOption
{
    public string name;
    public string description;
    public UpgradeType type;
    public float value; // На сколько увеличивать (например, +20 HP, +10% dmg)
    // можно добавить поле для спрайта, если нужно
}
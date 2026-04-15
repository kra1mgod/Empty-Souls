[System.Serializable]
public class UpgradeOption
{
    public string name;
    public string description;
    public UpgradeType type;
    public float value; // Например, +20 HP, +0.2f к скорости, +0.1f к размеру и т.п.
}
using UnityEngine;

public class KatanaEvolutionOption : IWeaponEvolutionOption
{
    public enum KatanaEvoType { TripleWave, PowerWave, BigShortWave }
    public KatanaEvoType type;
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Sprite Icon { get; private set; }

    public KatanaEvolutionOption(KatanaEvoType type, string title, string description, Sprite icon = null)
    {
        this.type = type;
        this.Title = title;
        this.Description = description;
        this.Icon = icon;
    }

    public void Apply(MonoBehaviour weapon)
    {
        var katana = weapon as KatanaWeapon;
        if (katana == null) return;

        switch (type)
        {
            case KatanaEvoType.TripleWave:
                katana.enableTripleWave = true;
                katana.waveDamageMultiplier = 0.6f;
                break;
            case KatanaEvoType.PowerWave:
                katana.waveDamageMultiplier = 1.5f;
                katana.waveSizeMultiplier = 0.7f;
                break;
            case KatanaEvoType.BigShortWave:
                katana.waveRangeMultiplier = 0.5f;
                katana.waveSizeMultiplier = 1.7f;
                katana.waveDamageMultiplier = 1.15f;
                break;
        }
        katana.isEvolved = true;
    }
}
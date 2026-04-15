using UnityEngine;

[CreateAssetMenu(fileName = "KatanaBigShortWave", menuName = "Evolutions/Katana/BigShortWave", order = 1)]
public class KatanaBigShortWaveSO : BaseEvolutionSO
{
    public float sizeMultiplier = 1.7f;
    public float rangeMultiplier = 0.5f;
    public float damageMultiplier = 1.15f;

    // --- COLOR EVOLUTION PATCH ---
    public Color evolutionColor = new Color(1f, 0.5f, 0.2f); // Оранжевый

    public override void ApplyToWeapon(MonoBehaviour weapon)
    {
        var katana = weapon as KatanaWeapon;
        if (katana == null) return;

        katana.waveSizeMultiplier = sizeMultiplier;
        katana.waveRangeMultiplier = rangeMultiplier;
        katana.waveDamageMultiplier = damageMultiplier;
        // --- COLOR EVOLUTION PATCH ---
        katana.waveColor = evolutionColor;
        katana.isEvolved = true;
    }
}
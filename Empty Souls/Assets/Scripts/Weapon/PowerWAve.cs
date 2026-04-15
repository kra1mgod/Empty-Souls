using UnityEngine;

[CreateAssetMenu(fileName = "KatanaPowerWave", menuName = "Evolutions/Katana/PowerWave", order = 3)]
public class KatanaPowerWaveSO : BaseEvolutionSO
{
    public float sizeMultiplier = 0.7f;
    public float damageMultiplier = 1.5f;

    // --- COLOR EVOLUTION PATCH ---
    public Color evolutionColor = new Color(0.5f, 1f, 0.3f); // Зеленоватый

    public override void ApplyToWeapon(MonoBehaviour weapon)
    {
        var katana = weapon as KatanaWeapon;
        if (katana == null) return;

        katana.waveSizeMultiplier = sizeMultiplier;
        katana.waveDamageMultiplier = damageMultiplier;
        // --- COLOR EVOLUTION PATCH ---
        katana.waveColor = evolutionColor;
        katana.isEvolved = true;
    }
}
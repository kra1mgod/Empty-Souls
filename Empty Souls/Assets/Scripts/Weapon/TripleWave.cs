using UnityEngine;

[CreateAssetMenu(fileName = "KatanaTripleWave", menuName = "Evolutions/Katana/TripleWave", order = 2)]
public class KatanaTripleWaveSO : BaseEvolutionSO
{
    public float damageMultiplier = 0.6f;

    // --- COLOR EVOLUTION PATCH ---
    public Color evolutionColor = new Color(0.2f, 0.8f, 1f); // Голубой

    public override void ApplyToWeapon(MonoBehaviour weapon)
    {
        var katana = weapon as KatanaWeapon;
        if (katana == null) return;

        katana.enableTripleWave = true;
        katana.waveDamageMultiplier = damageMultiplier;
        // --- COLOR EVOLUTION PATCH ---
        katana.waveColor = evolutionColor;
        katana.isEvolved = true;
    }
}
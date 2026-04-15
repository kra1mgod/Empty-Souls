using UnityEngine;

public class KatanaWeapon : MonoBehaviour, IAutoAttackWeapon
{
    public GameObject wavePrefab;
    public float fireInterval = 1f;
    private float timer;

    public PlayerStats playerStats;

    // --- Ёволюционные параметры ---
    public bool enableTripleWave = false;
    public float waveDamageMultiplier = 1f;
    public float waveSizeMultiplier = 1f;
    public float waveRangeMultiplier = 1f;
    public bool isEvolved = false;

    // --- COLOR EVOLUTION PATCH ---
    public Color waveColor = Color.white;

    public void TickUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            timer = 0f;
            FireWave();
        }
    }

    private void FireWave()
    {
        if (wavePrefab == null) return;
        Vector2 dir = playerStats != null ? playerStats.GetMoveDirection() : Vector2.right;

        foreach (var angleOffset in (enableTripleWave ? new float[] { -15, 0, 15 } : new float[] { 0 }))
        {
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float totalAngle = baseAngle + angleOffset;
            Vector2 rotatedDir = new Vector2(Mathf.Cos(totalAngle * Mathf.Deg2Rad), Mathf.Sin(totalAngle * Mathf.Deg2Rad));

            var waveObj = Instantiate(wavePrefab, transform.position, Quaternion.identity);
            var wave = waveObj.GetComponent<KatanaWave>();
            if (wave != null)
            {
                wave.SetDirection(rotatedDir);
                wave.playerStats = this.playerStats;
                wave.damage = Mathf.RoundToInt(wave.damage * waveDamageMultiplier);
                wave.transform.localScale *= waveSizeMultiplier;
                wave.lifetime *= waveRangeMultiplier;
                // --- COLOR EVOLUTION PATCH ---
                wave.waveColor = this.waveColor;
            }
        }
    }
}
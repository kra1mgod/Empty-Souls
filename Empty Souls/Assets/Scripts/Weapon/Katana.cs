using UnityEngine;

public class KatanaWeapon : MonoBehaviour, IAutoAttackWeapon
{
    public GameObject wavePrefab;
    public float fireInterval = 1f;
    private float timer;

    public PlayerStats playerStats; // назначь в инспекторе или найди в Awake

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
        var waveObj = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        var wave = waveObj.GetComponent<KatanaWave>();
        if (wave != null)
        {
            wave.SetDirection(dir);
        }
    }
}
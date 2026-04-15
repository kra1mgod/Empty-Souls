using UnityEngine;
using System.Collections;
using TMPro;

public class LevelWaveManager : MonoBehaviour
{
    public TextMeshProUGUI waveTimerText;
    public EnemySpawner enemySpawner;
    public BossSpawner bossSpawner;

    private LevelData levelData;
    private int currentWave = -1;
    private float waveTimeLeft;
    private bool levelCompleted = false;
    private GameObject currentBoss;

    private AudioSource musicSource;

    void Start()
    {
        levelData = LevelManager.Instance?.CurrentLevelData;
        if (levelData == null)
        {
            Debug.LogError("LevelWaveManager: LevelData not found!");
            enabled = false;
            return;
        }

        PlayLevelMusic();

        BossAI.OnBossDefeated += OnBossDefeated;
        StartCoroutine(WaveRoutine());
    }

    void PlayLevelMusic()
    {
        if (levelData.levelMusic != null)
        {
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null)
                musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = levelData.levelMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void OnDestroy()
    {
        BossAI.OnBossDefeated -= OnBossDefeated;
    }

    void OnBossDefeated()
    {
        if (levelCompleted) return;
        levelCompleted = true;
        StartCoroutine(HandleVictory());
    }

    IEnumerator HandleVictory()
    {
        if (waveTimerText != null)
            waveTimerText.text = "ПОБЕДА!";
        if (enemySpawner != null)
            enemySpawner.enabled = false;
        if (musicSource != null)
            musicSource.Stop();

        yield return new WaitForSeconds(3f);
        Debug.Log("Level completed!");
    }

    IEnumerator WaveRoutine()
    {
        for (currentWave = 0; currentWave < levelData.waves.Count; currentWave++)
        {
            var wave = levelData.waves[currentWave];
            waveTimeLeft = wave.duration;

            Debug.Log($"Wave {currentWave + 1} started: {wave.waveName}");

            // Спавним врагов для волны
            if (enemySpawner != null)
                enemySpawner.SpawnEnemies(wave.enemySpawns);

            // Таймер волны
            while (waveTimeLeft > 0 && !levelCompleted)
            {
                if (waveTimerText != null)
                {
                    waveTimerText.text = $"Волна {currentWave + 1} ({wave.waveName}) | Осталось: {waveTimeLeft:F1}с";
                }
                waveTimeLeft -= Time.deltaTime;
                yield return null;
            }
        }

        // После всех волн спавним босса
        if (!levelCompleted)
        {
            SpawnBossSimple();
        }
    }

    void SpawnBossSimple()
    {
        Debug.Log("Spawning boss...");

        if (enemySpawner != null)
            enemySpawner.enabled = false;

        if (bossSpawner != null)
        {
            currentBoss = bossSpawner.SpawnBoss();
            if (waveTimerText != null)
                waveTimerText.text = "БОСС!";
        }
        else
        {
            Debug.LogError("LevelWaveManager: bossSpawner not assigned!");
        }
    }

    // Остальные методы ForceSpawnBoss, StopLevel, и т.д. можно оставить как есть
}
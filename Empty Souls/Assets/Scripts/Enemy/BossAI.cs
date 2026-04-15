using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("Boss Stats")]
    public int maxHP = 100;
    public float moveSpeed = 1.5f;
    public int touchDamage = 20;
    public int CurrentHP => currentHP;

    [Header("Attack System")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireInterval = 2f;

    [Header("Summon System")]
    public GameObject enemyPrefab;
    public float summonInterval = 5f;
    public int summonCount = 3;
    public float summonRadius = 8f;

    [Header("Combat")]
    public float stunResistance = 0.5f; // Сопротивление оглушению (0-1)
    public bool isInvulnerable = false;

    private int currentHP;
    private Transform player;
    private float fireTimer = 0f;
    private float summonTimer = 0f;
    private float stunTimer = 0f;
    private bool isDead = false;

    [Header("Boss Effects")]
    public AudioClip spawnSound; // Клип для звука появления
    public BossHPBar customBossHpBar; // Только если хочешь вручную указать, иначе null
    public ParticleSystem spawnEffectPrefab; // Префаб эффекта появления

    // Кеширование компонентов
    private BossHPBar bossHPBar;
    private GameEndPanel gameEndPanel; // victory/defeat panel

    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // --- Динамически ищем BossHPBar и GameEndPanel, если не назначены ---
        bossHPBar = customBossHpBar != null ? customBossHpBar : FindObjectOfType<BossHPBar>();
        if (bossHPBar != null)
        {
            bossHPBar.SetHP(currentHP, maxHP);
            bossHPBar.ShowBar();
        }
        else
        {
            Debug.LogWarning("BossHPBar NOT found!");
        }

        gameEndPanel = FindObjectOfType<GameEndPanel>();

        // --- ЗВУК СПАВНА ---
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, transform.position, 1.0f);
        }

        // --- ЭФФЕКТ ПАРТИКЛОВ ---
        if (spawnEffectPrefab != null)
        {
            ParticleSystem ps = Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        if (player == null)
        {
            Debug.LogWarning("Player not found! Boss AI will not function properly.");
        }

        // Подписка на смерть игрока для панели поражения
        PlayerStats playerStats = player != null ? player.GetComponent<PlayerStats>() : null;
        if (playerStats != null)
        {
            playerStats.OnPlayerDeath += OnPlayerDeath;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            return;
        }

        MoveTowardsPlayer();
        HandleShooting();
        HandleSummoning();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    private void HandleShooting()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    private void HandleSummoning()
    {
        summonTimer += Time.deltaTime;
        if (summonTimer >= summonInterval)
        {
            summonTimer = 0f;
            SummonEnemies();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<EnemyBossBullet>();
        if (bulletScript != null)
        {
            Vector2 direction = (player.position - bulletSpawnPoint.position).normalized;
            bulletScript.SetDirection(direction);
        }
    }

    private void SummonEnemies()
    {
        if (enemyPrefab == null) return;

        for (int i = 0; i < summonCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * summonRadius;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        var flash = GetComponent<DamageFlash>();
        if (flash != null)
            flash.Flash();

        if (bossHPBar != null)
        {
            bossHPBar.SetHP(currentHP, maxHP);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void ApplyStun(float duration)
    {
        float actualDuration = duration * (1f - stunResistance);
        stunTimer = Mathf.Max(stunTimer, actualDuration);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (bossHPBar != null)
        {
            bossHPBar.HideBar();
        }

        // Вызов Victory панели
        if (gameEndPanel != null)
        {
            gameEndPanel.ShowVictory();
        }

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        enabled = false;
        yield return new WaitForSeconds(2f);

        OnBossDefeated?.Invoke();

        Destroy(gameObject);
    }

    // События
    public static System.Action OnBossDefeated;

    // Столкновение с игроком (урон при касании)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            var playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                int baseDamage = touchDamage;
                float strengthBonus = 0f;
                if (playerStats.GetType().GetMethod("GetStrengthTouchDamageBonus") != null)
                {
                    strengthBonus = (float)playerStats.GetType()
                        .GetMethod("GetStrengthTouchDamageBonus")
                        .Invoke(playerStats, null);
                }
                playerStats.TakeDamage(baseDamage + Mathf.RoundToInt(strengthBonus));
            }
        }
    }

    // Методы для внешнего управления
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHP / maxHP;
    }

    // Методы для фаз босса (можно расширить)
    public void EnterPhase2()
    {
        fireInterval *= 0.5f;
        moveSpeed *= 1.3f;
        summonInterval *= 0.7f;
    }

    public void EnterPhase3()
    {
        fireInterval *= 0.3f;
        moveSpeed *= 1.5f;
        summonCount *= 2;
    }

    // Для показа панели поражения, когда умирает игрок
    private void OnPlayerDeath()
    {
        if (gameEndPanel != null)
            gameEndPanel.ShowDefeat();
    }
}
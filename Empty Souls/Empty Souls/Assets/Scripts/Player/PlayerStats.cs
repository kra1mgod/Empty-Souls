using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int level = 1;
    public int experience = 0;
    public int expToNextLevel = 100;

    [Header("UI")]
    public AnimatedHPBar animatedHPBar;
    public GameObject deathPanel;

    [Header("Invulnerability")]
    public float invulnTime = 1.0f;
    private float invulnTimer = 0f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public delegate void OnHPChanged(int hp, int maxHP);
    public event OnHPChanged onHPChanged;

    void Start()
    {
        currentHP = maxHP;
        if (animatedHPBar == null)
            animatedHPBar = FindObjectOfType<AnimatedHPBar>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    void Update()
    {
        if (invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;
            if (spriteRenderer != null)
            {
                float t = Mathf.PingPong(Time.time * 10, 1f);
                spriteRenderer.color = Color.Lerp(originalColor, Color.clear, t);
            }
        }
        else
        {
            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (invulnTimer > 0) return;
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;
        onHPChanged?.Invoke(currentHP, maxHP);
        if (currentHP == 0) Die();
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
        invulnTimer = invulnTime;
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        onHPChanged?.Invoke(currentHP, maxHP);
        if (animatedHPBar != null)
            animatedHPBar.SetHP(currentHP, maxHP);
    }

    void Die()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        while (experience >= expToNextLevel)
        {
            experience -= expToNextLevel;
            LevelUp();
        }
    }
    void LevelUp()
    {
        level++;
        Debug.Log("LevelUpUI.Instance = " + LevelUpUI.Instance);
        if (LevelUpUI.Instance == null) Debug.LogError("LevelUpUI.Instance is NULL!");
        LevelUpUI.Instance.ShowUpgradeChoices();
    }
}
using UnityEngine;

public class UserStatsManager : MonoBehaviour
{
    public static UserStatsManager Instance;

    public float totalPlayTime; // in seconds
    public int totalKills;
    public int totalDeaths;
    public int highestLevel;
    public int totalSoulFragments;
    // Добавь другие нужные поля

    private float sessionStartTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sessionStartTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            totalPlayTime += Time.unscaledDeltaTime;
        }
    }

    public void AddKill()
    {
        totalKills++;
        SaveStats();
    }

    public void AddDeath()
    {
        totalDeaths++;
        SaveStats();
    }

    public void SetHighestLevel(int level)
    {
        if (level > highestLevel)
        {
            highestLevel = level;
            SaveStats();
        }
    }

    public void AddSoulFragments(int amount)
    {
        totalSoulFragments += amount;
        SaveStats();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetFloat("UserStats_TotalPlayTime", totalPlayTime);
        PlayerPrefs.SetInt("UserStats_TotalKills", totalKills);
        PlayerPrefs.SetInt("UserStats_TotalDeaths", totalDeaths);
        PlayerPrefs.SetInt("UserStats_HighestLevel", highestLevel);
        PlayerPrefs.SetInt("UserStats_TotalSoulFragments", totalSoulFragments);
        PlayerPrefs.Save();
    }

    public void LoadStats()
    {
        totalPlayTime = PlayerPrefs.GetFloat("UserStats_TotalPlayTime", 0);
        totalKills = PlayerPrefs.GetInt("UserStats_TotalKills", 0);
        totalDeaths = PlayerPrefs.GetInt("UserStats_TotalDeaths", 0);
        highestLevel = PlayerPrefs.GetInt("UserStats_HighestLevel", 0);
        totalSoulFragments = PlayerPrefs.GetInt("UserStats_TotalSoulFragments", 0);
    }
}
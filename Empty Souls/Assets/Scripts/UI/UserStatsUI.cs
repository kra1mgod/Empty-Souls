using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UserStatsPanelUI : MonoBehaviour
{
    public GameObject statsPanel;
    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;
    public TextMeshProUGUI highestLevelText;
    public TextMeshProUGUI soulFragmentsText;

    void OnEnable()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        if (UserStatsManager.Instance != null)
        {
            playTimeText.text = $"Время в игре: {FormatTime(UserStatsManager.Instance.totalPlayTime)}";
            killsText.text = $"Врагов убито: {UserStatsManager.Instance.totalKills}";
            deathsText.text = $"Смертей: {UserStatsManager.Instance.totalDeaths}";
            highestLevelText.text = $"Максимальный уровень: {UserStatsManager.Instance.highestLevel}";
            soulFragmentsText.text = $"Собрано фрагментов: {UserStatsManager.Instance.totalSoulFragments}";
        }
    }

    string FormatTime(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
    }

    public void OpenPanel()
    {
        if (statsPanel != null)
            statsPanel.SetActive(true);
        UpdateStats();
    }

    public void ClosePanel()
    {
        if (statsPanel != null)
            statsPanel.SetActive(false);
    }
}
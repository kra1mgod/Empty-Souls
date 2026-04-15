using UnityEngine;
using TMPro;
using System;

public class UserStatsPanelUI : MonoBehaviour
{
    public GameObject statsPanel;
    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI deathsText;
    public TextMeshProUGUI highestLevelText;
    public TextMeshProUGUI soulFragmentsText;
    public TextMeshProUGUI evolutionCountText;

    void OnEnable()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile != null)
        {
            playTimeText.text = $"Время в игре: {FormatTime(profile.gameStatistics.totalPlayTimeSec)}";
            deathsText.text = $"Смертей: {profile.gameStatistics.totalDeaths}";
            highestLevelText.text = $"Максимальный уровень: {profile.gameStatistics.bestLevel}";
            soulFragmentsText.text = $"Фрагменты души: {profile.resources.totalSoulFragments}";
            evolutionCountText.text = $"Эволюций: {profile.gameStatistics.evolutionCount}";
        }
        else
        {
            playTimeText.text = deathsText.text = highestLevelText.text = soulFragmentsText.text = evolutionCountText.text = "Нет данных";
        }
    }

    string FormatTime(int totalSeconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
    }
}
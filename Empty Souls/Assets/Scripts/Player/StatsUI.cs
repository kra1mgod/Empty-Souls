using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsMenuUI : MonoBehaviour
{
    [Header("References")]
    public GameObject statsPanel;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI intelligenceText;

    [Header("Stats Source")]
    public PlayerStats playerStats;

    private bool isOpen = false;

    void Start()
    {
        statsPanel.SetActive(false);
        if (statsPanel != null)
            statsPanel.SetActive(false);
    }

    void Update()
    {
        // ≈сли меню открыто Ч обновл€й тексты
        if (isOpen && playerStats != null)
        {
            strengthText.text = $"—ила: {playerStats.strength.level}";
            agilityText.text = $"Ћовкость: {playerStats.agility.level}";
            intelligenceText.text = $"»нтеллект: {playerStats.intelligence.level}";
        }
    }
}
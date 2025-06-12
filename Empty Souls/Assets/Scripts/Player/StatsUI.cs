using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsMenuUI : MonoBehaviour
{
    [Header("References")]
    public GameObject statsPanel;
    public Button openButton;
    public Button closeButton;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI intelligenceText;

    [Header("Stats Source")]
    public PlayerStats playerStats;

    private bool isOpen = false;

    void Start()
    {
        if (statsPanel != null)
            statsPanel.SetActive(false);
        if (openButton != null)
            openButton.onClick.AddListener(ToggleMenu);
        if (closeButton != null)
            closeButton.onClick.AddListener(ToggleMenu);
    }

    void Update()
    {
        // ќткрыть/закрыть по клавише "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleMenu();
        }

        // ≈сли меню открыто Ч обновл€й тексты
        if (isOpen && playerStats != null)
        {
            strengthText.text = $"—ила: {playerStats.strength.level}";
            agilityText.text = $"Ћовкость: {playerStats.agility.level}";
            intelligenceText.text = $"»нтеллект: {playerStats.intelligence.level}";
        }
    }

    public void ShowButton(GameObject buttonObj)
    {
        if (buttonObj != null)
            buttonObj.SetActive(true);
    }

    public void HideButton(GameObject buttonObj)
    {
        if (buttonObj != null)
            buttonObj.SetActive(false);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        if (statsPanel != null)
            statsPanel.SetActive(isOpen);
    }
}
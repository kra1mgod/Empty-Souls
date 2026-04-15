using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TabMenuUI : MonoBehaviour
{
    public GameObject tabMenuPanel;         // Общий большой панель
    public GameObject statsPanel;
    public GameObject inventoryPanel;
    public GameObject charPanel;

    [Header("Кнопки/Табы")]
    public Button statsTabButton;
    public Button inventoryTabButton;
    public Button charTabButton;

    [Header("Stats")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI intelligenceText;
    public PlayerStats playerStats;

    private bool isOpen = false;

    void Start()
    {
        statsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        charPanel.SetActive(false);
        tabMenuPanel.SetActive(false);

        statsTabButton.onClick.AddListener(() => ShowTab("stats"));
        inventoryTabButton.onClick.AddListener(() => ShowTab("inv"));
        charTabButton.onClick.AddListener(() => ShowTab("char"));

        // НЕ вызывай ShowTab("stats") здесь!
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            tabMenuPanel.SetActive(isOpen);
            Time.timeScale = isOpen ? 0f : 1f;

            if (isOpen)
            {
                // Открыли меню — показываем вкладку по умолчанию
                ShowTab("stats");
            }
            else
            {
                // Закрыли меню — скрываем все внутренние панели
                statsPanel.SetActive(false);
                inventoryPanel.SetActive(false);
                charPanel.SetActive(false);
            }
        }

        if (isOpen && statsPanel.activeSelf)
            UpdateStatsText();
    }

    void ShowTab(string tab)
    {
        statsPanel.SetActive(tab == "stats");
        inventoryPanel.SetActive(tab == "inv");
        charPanel.SetActive(tab == "char");
        if (tab == "char")
            charPanel.GetComponent<CharPanelUI>()?.Refresh();
    }

    void UpdateStatsText()
    {
        if (playerStats == null) return;
        strengthText.text = $"Сила: {playerStats.strength.level}";
        agilityText.text = $"Ловкость: {playerStats.agility.level}";
        intelligenceText.text = $"Интеллект: {playerStats.intelligence.level}";
        // ...добавь любые другие характеристики!
    }
}
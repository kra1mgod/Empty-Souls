using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button[] choiceButtons;
    public Text[] choiceTexts;
    public GameObject lumzvarBar;

    List<UpgradeOption> currentOptions;

    public static LevelUpUI Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        panel.SetActive(false);
    }
    public void ShowUpgradeChoices()
    {
        if (lumzvarBar != null)
            lumzvarBar.SetActive(false); // Скрыть при выборе оружия
        panel.SetActive(true);
        int toShow = Mathf.Min(choiceButtons.Length, UpgradeSystem.Instance.allUpgrades.Count);
        currentOptions = UpgradeSystem.Instance.GetRandomOptions(toShow);

        // Скрыть все кнопки перед показом
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(i < toShow);
        }

        for (int i = 0; i < toShow; i++)
        {
            choiceTexts[i].text = currentOptions[i].description;
            int idx = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoose(idx));
        }

        Time.timeScale = 0f;
    }

    void OnChoose(int idx)
    {
        UpgradeSystem.Instance.ApplyUpgrade(currentOptions[idx]);
        panel.SetActive(false);
        Time.timeScale = 1f;
        if (lumzvarBar != null)
            lumzvarBar.SetActive(true); // Показать обратно после выбора
    }
}
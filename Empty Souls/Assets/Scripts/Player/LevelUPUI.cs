using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button[] choiceButtons;
    public Text[] choiceTexts;

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
        panel.SetActive(true);
        Debug.Log("ShowUpgradeChoices called!");
        Debug.Log("UpgradeSystem.Instance: " + UpgradeSystem.Instance);
        if (UpgradeSystem.Instance != null)
            Debug.Log("allUpgrades.Count: " + UpgradeSystem.Instance.allUpgrades.Count);

        currentOptions = UpgradeSystem.Instance.GetRandomOptions(3);
        Debug.Log("currentOptions.Count: " + currentOptions.Count);

        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"choiceTexts[{i}] = {choiceTexts[i]}");
            Debug.Log($"choiceButtons[{i}] = {choiceButtons[i]}");
            Debug.Log($"currentOptions[{i}] = {currentOptions[i]}");
            if (currentOptions[i] != null)
                Debug.Log($"currentOptions[{i}].description = {currentOptions[i].description}");

            choiceTexts[i].text = currentOptions[i].description; // тут может быть null
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
    }
}
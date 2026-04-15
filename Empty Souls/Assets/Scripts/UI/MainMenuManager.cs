using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Главный контроллер меню с поддержкой выбора профиля пользователя (как в Risk of Rain 2).
/// Добавлено: панель со статами игрока (из сейва) и кнопка для её вызова.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject fragmentSelectPanel;
    public GameObject helpPanel;
    public GameObject characterSelectPanel;
    public GameObject levelSelectPanel;

    [Header("Fragment Selection UI")]
    public Image fragmentImage;
    public TextMeshProUGUI fragmentNameText;
    public TextMeshProUGUI fragmentDescriptionText;
    public Button prevFragmentButton;
    public Button nextFragmentButton;
    public Button chooseFragmentButton;
    public Button backFromFragmentButton;
    public Button levelSelectButton;
    public TextMeshProUGUI FragmentMessage;

    [Header("Fragments")]
    public BaseAbilitySO[] fragments; // Заполняется в инспекторе
    private int fragmentIndex = 0;

    // --- Profile Selection UI ---
    [Header("Profile Selection UI")]
    public GameObject profilePanel; // Панель профилей (отдельная панель или секция)
    public Transform profileListParent; // Контейнер под кнопки профилей (Vertical Layout Group)
    public GameObject profileButtonPrefab; // Префаб кнопки профиля (Button + Text)
    public TMP_InputField createProfileInput; // Поле для ввода имени нового профиля
    public Button createProfileButton;
    public Button deleteProfileButton;
    public Button selectProfileButton;
    public TextMeshProUGUI activeProfileText;

    [Header("Soul Fragments UI")]
    public TextMeshProUGUI soulFragmentsText; // UI элемент для Soul Fragments

    // --- Player Stats Panel ---
    [Header("Player Stats Panel")]
    public GameObject playerStatsPanel; // Панель для отображения статов игрока
    public Button openPlayerStatsButton; // Кнопка для открытия панели статов
    public Button closePlayerStatsButton; // Кнопка для закрытия панели статов

    // Статы (TextMeshProUGUI для каждого, назначить в инспекторе)
    public TextMeshProUGUI statsLevelText;
    public TextMeshProUGUI statsExpText;
    public TextMeshProUGUI statsHPText;
    public TextMeshProUGUI statsStrengthText;
    public TextMeshProUGUI statsAgilityText;
    public TextMeshProUGUI statsIntelligenceText;
    public TextMeshProUGUI statsSoulFragmentsText;
    public TextMeshProUGUI statsEvolutionsText;
    public TextMeshProUGUI statsPlaytimeText;
    public TextMeshProUGUI statsDeathsText;

    private string selectedProfileId = null;
    private string selectedProfileName = null;

    private async void Start()
    {
        // --- Панели ---
        ShowOnlyMainMenu();

        if (FragmentMessage != null) FragmentMessage.text = $"Выбранный фрагмент: ";

        // --- GameData сброс ---
        GameData.selectedFragment = null;
        GameData.selectedCharacterIndex = 0;
        GameData.selectedCharacter = CharacterType.Red;

        // --- Кнопки выбора фрагментов ---
        if (prevFragmentButton != null) prevFragmentButton.onClick.AddListener(SelectPreviousFragment);
        if (nextFragmentButton != null) nextFragmentButton.onClick.AddListener(SelectNextFragment);
        if (chooseFragmentButton != null) chooseFragmentButton.onClick.AddListener(ChooseFragment);
        if (backFromFragmentButton != null) backFromFragmentButton.onClick.AddListener(BackToCharacterSelect);
        if (levelSelectButton != null) levelSelectButton.onClick.AddListener(ShowLevelSelect);

        // --- Профили ---
        if (createProfileButton != null) createProfileButton.onClick.AddListener(OnCreateProfileClicked);
        if (deleteProfileButton != null) deleteProfileButton.onClick.AddListener(OnDeleteProfileClicked);
        if (selectProfileButton != null) selectProfileButton.onClick.AddListener(OnSelectProfileClicked);

        // --- Кнопки открытия/закрытия панели статов ---
        if (openPlayerStatsButton != null) openPlayerStatsButton.onClick.AddListener(OpenPlayerStatsPanel);
        if (closePlayerStatsButton != null) closePlayerStatsButton.onClick.AddListener(ClosePlayerStatsPanel);

        // Скрываем панель статов по умолчанию
        if (playerStatsPanel != null) playerStatsPanel.SetActive(false);

        // Ждем полной загрузки сейва перед работой с профилями/фрагментами!
        if (!SaveManager.Instance.IsLoaded)
            await SaveManager.Instance.LoadAsync();

        RefreshProfileList();
        UpdateFragmentUI();
        UpdateSoulFragmentsUI();
    }

    // --- Soul Fragments UI ---
    private void UpdateSoulFragmentsUI()
    {
        var profile = SaveManager.Instance?.ActiveProfile;
        if (soulFragmentsText != null && profile != null)
            soulFragmentsText.text = $"Фрагменты души: {profile.resources.totalSoulFragments}";
        else if (soulFragmentsText != null)
            soulFragmentsText.text = "Фрагменты души: 0";
    }

    // --- Панели: только одна активна, profilePanel только на главном меню ---
    void ShowOnlyMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (fragmentSelectPanel != null) fragmentSelectPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (characterSelectPanel != null) characterSelectPanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(true); // profilePanel только на главном меню!
        if (playerStatsPanel != null) playerStatsPanel.SetActive(false);
        UpdateSoulFragmentsUI();
    }

    void ShowOnlyPanel(GameObject panel)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (fragmentSelectPanel != null) fragmentSelectPanel.SetActive(false);
        if (helpPanel != null) helpPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (characterSelectPanel != null) characterSelectPanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(false); // profilePanel скрывается на остальных!
        if (playerStatsPanel != null) playerStatsPanel.SetActive(false);
        if (panel != null) panel.SetActive(true);
        UpdateSoulFragmentsUI();
    }

    // --- PROFILE UI LOGIC ---

    void RefreshProfileList()
    {
        // Очистить старые кнопки
        foreach (Transform child in profileListParent)
            Destroy(child.gameObject);

        var save = SaveManager.Instance?.LoadedSave;
        if (save == null)
        {
            activeProfileText.text = "Нет сохранений!";
            UpdateSoulFragmentsUI();
            return;
        }

        foreach (var profile in save.profiles)
        {
            var go = Instantiate(profileButtonPrefab, profileListParent);
            var btn = go.GetComponent<Button>();
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = profile.profileName;
            string pid = profile.profileId;
            string pname = profile.profileName;
            btn.onClick.AddListener(() => OnProfileSelected(pid, pname));
            // Подсветка активного
            if (SaveManager.Instance.ActiveProfile != null && SaveManager.Instance.ActiveProfile.profileId == pid)
                txt.color = Color.yellow;
            else
                txt.color = Color.white;
        }

        UpdateActiveProfileUI();
        selectProfileButton.interactable = false;
        deleteProfileButton.interactable = false;
        UpdateSoulFragmentsUI();
    }

    void OnProfileSelected(string profileId, string profileName)
    {
        selectedProfileId = profileId;
        selectedProfileName = profileName;
        selectProfileButton.interactable = true;
        deleteProfileButton.interactable = true;
    }

    void OnSelectProfileClicked()
    {
        if (string.IsNullOrEmpty(selectedProfileId)) return;
        SaveManager.Instance.SetActiveProfile(selectedProfileId);
        UpdateActiveProfileUI();
        RefreshProfileList();
        UpdateSoulFragmentsUI();
    }

    void OnCreateProfileClicked()
    {
        if (createProfileInput == null)
        {
            Debug.LogError("CreateProfileInput is not assigned in inspector!");
            return;
        }
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager instance not found in scene!");
            return;
        }
        string name = createProfileInput.text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("Введите имя профиля!");
            return;
        }
        SaveManager.Instance.CreateProfile(name);
        SaveManager.Instance.SetActiveProfile(SaveManager.Instance.ActiveProfile.profileId);
        RefreshProfileList();
        selectProfileButton.interactable = false;
        deleteProfileButton.interactable = false;
        UpdateSoulFragmentsUI();
    }

    void OnDeleteProfileClicked()
    {
        if (string.IsNullOrEmpty(selectedProfileId)) return;
        SaveManager.Instance.DeleteProfile(selectedProfileId);
        selectedProfileId = null;
        selectedProfileName = null;
        RefreshProfileList();
        UpdateSoulFragmentsUI();
    }

    void UpdateActiveProfileUI()
    {
        var active = SaveManager.Instance.ActiveProfile;
        if (activeProfileText != null)
            activeProfileText.text = active != null ? $"Активный профиль: {active.profileName}" : "Нет активного профиля";
        UpdateSoulFragmentsUI();
    }

    // --- Фрагменты и UI ---
    public void ShowCharacterSelect() => ShowOnlyPanel(characterSelectPanel);

    public void ShowMainMenuForHelpPanel() => ShowOnlyMainMenu();

    public void ShowFragmentSelect()
    {
        ShowOnlyPanel(fragmentSelectPanel);
        fragmentIndex = 0;
        UpdateFragmentUI();
    }

    public void ShowHelpPanel() => ShowOnlyPanel(helpPanel);

    public void BackToCharacterSelect() => ShowOnlyPanel(characterSelectPanel);

    public void BackToMainMenu() => ShowOnlyMainMenu();

    public void SelectPreviousFragment()
    {
        if (fragments == null || fragments.Length == 0) return;
        fragmentIndex = (fragmentIndex - 1 + fragments.Length) % fragments.Length;
        UpdateFragmentUI();
    }

    public void SelectNextFragment()
    {
        if (fragments == null || fragments.Length == 0) return;
        fragmentIndex = (fragmentIndex + 1) % fragments.Length;
        UpdateFragmentUI();
    }

    public void ChooseFragment()
    {
        if (fragments == null || fragments.Length == 0)
        {
            Debug.LogWarning("Нет доступных фрагментов для выбора.");
            return;
        }
        if (FragmentMessage != null) FragmentMessage.text = $"Выбранный фрагмент: {fragments[fragmentIndex].abilityName}";
        GameData.selectedFragment = fragments[fragmentIndex];
        Debug.Log("Фрагмент выбран: " + fragments[fragmentIndex].abilityName);
    }

    private void UpdateFragmentUI()
    {
        if (fragments == null || fragments.Length == 0 || fragmentIndex < 0 || fragmentIndex >= fragments.Length)
        {
            if (fragmentImage != null) fragmentImage.sprite = null;
            if (fragmentNameText != null) fragmentNameText.text = "N/A";
            if (fragmentDescriptionText != null) fragmentDescriptionText.text = "Нет доступных фрагментов.";
            return;
        }
        var frag = fragments[fragmentIndex];
        if (frag == null)
        {
            Debug.LogWarning($"Фрагмент с индексом {fragmentIndex} не найден (null).");
            return;
        }
        if (fragmentImage != null) fragmentImage.sprite = frag.icon;
        if (fragmentNameText != null) fragmentNameText.text = frag.abilityName;
        if (fragmentDescriptionText != null) fragmentDescriptionText.text = frag.description;
    }

    // Персонажи
    public void SelectRedCharacter()
    {
        GameData.selectedCharacter = CharacterType.Red;
        GameData.selectedCharacterIndex = 1;
        Debug.Log("Выбран Красный персонаж");
    }
    public void SelectBlueCharacter()
    {
        GameData.selectedCharacter = CharacterType.Blue;
        GameData.selectedCharacterIndex = 2;
        Debug.Log("Выбран Синий персонаж");
    }

    // Кнопка "Играть"
    public void PlayGame()
    {
        if (SaveManager.Instance.ActiveProfile == null)
        {
            Debug.LogWarning("Профиль не выбран!");
            // Можно показать сообщение пользователю в UI
            return;
        }
        if (GameData.selectedFragment == null)
        {
            Debug.LogWarning("Сначала выберите фрагмент!");
            return;
        }
        if (GameData.selectedCharacterIndex == 0)
        {
            Debug.LogWarning("Сначала выберите персонажа!");
            return;
        }
        if (levelSelectPanel != null && levelSelectPanel.activeSelf)
        {
            levelSelectPanel.SetActive(false);
        }
        SaveManager.Instance.ActiveProfile.currentSelection.selectedFragment = GameData.selectedFragment.abilityName;
        // Далее загружаем сцену
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ShowLevelSelect()
    {
        ShowOnlyPanel(levelSelectPanel);
    }

    public void BackFromLevelSelect()
    {
        ShowOnlyMainMenu();
    }

    // --- PLAYER STATS PANEL LOGIC ---

    public void OpenPlayerStatsPanel()
    {
        if (playerStatsPanel != null) playerStatsPanel.SetActive(true);

        var profile = SaveManager.Instance?.ActiveProfile;
        if (profile == null)
        {
            if (statsLevelText != null) statsLevelText.text = "Уровень: ?";
            if (statsExpText != null) statsExpText.text = "Опыт: ?";
            if (statsHPText != null) statsHPText.text = "HP: ?";
            if (statsStrengthText != null) statsStrengthText.text = "?";
            if (statsAgilityText != null) statsAgilityText.text = "?";
            if (statsIntelligenceText != null) statsIntelligenceText.text = "?";
            if (statsSoulFragmentsText != null) statsSoulFragmentsText.text = "?";
            if (statsEvolutionsText != null) statsEvolutionsText.text = "?";
            if (statsPlaytimeText != null) statsPlaytimeText.text = "?";
            if (statsDeathsText != null) statsDeathsText.text = "?";
            return;
        }

        // Доступные реальные поля:
        if (statsLevelText != null) statsLevelText.text = $"Лучший уровень: {profile.gameStatistics.bestLevel}";
        if (statsEvolutionsText != null) statsEvolutionsText.text = $"Эволюций: {profile.gameStatistics.evolutionCount}";
        if (statsPlaytimeText != null) statsPlaytimeText.text = $"Время: {FormatPlayTime(profile.gameStatistics.totalPlayTimeSec)}";
        if (statsDeathsText != null) statsDeathsText.text = $"Смертей: {profile.gameStatistics.totalDeaths}";
        if (statsSoulFragmentsText != null) statsSoulFragmentsText.text = $"Фрагменты: {profile.resources.totalSoulFragments}";

        // Показываем количество разблокированных умений/оружия, если нужно:
        if (statsExpText != null) statsExpText.text = $"Умений: {profile.progression.unlockedAbilities.Count}";
        if (statsHPText != null) statsHPText.text = $"Оружий: {profile.progression.weaponProgress.Count}";

        // Если хочешь показывать силу, ловкость, интеллект, уровень, опыт, HP —
        // добавь эти поля в Progression и записывай их из PlayerStats при сохранении!
    }   

    public void ClosePlayerStatsPanel()
    {
        if (playerStatsPanel != null) playerStatsPanel.SetActive(false);
    }

    private string FormatPlayTime(int seconds)
    {
        int h = seconds / 3600;
        int m = (seconds % 3600) / 60;
        int s = seconds % 60;
        if (h > 0)
            return $"{h}ч {m}м {s}с";
        else if (m > 0)
            return $"{m}м {s}с";
        else
            return $"{s}с";
    }
}
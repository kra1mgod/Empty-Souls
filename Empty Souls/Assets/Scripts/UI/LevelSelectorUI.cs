using UnityEngine;
using UnityEngine.UI; // Для работы с UI элементами, такими как Button, Text
using System.Collections.Generic;
using TMPro; // Для List

/// <summary>
/// Управляет UI для выбора уровней.
/// Взаимодействует с LevelManager для отображения и переключения уровней.
/// </summary>
public class LevelSelectorUI : MonoBehaviour
{
    [Header("Ссылки на компоненты")]
    [Tooltip("Ссылка на LevelManager в сцене.")]
    [SerializeField] private LevelManager levelManager;

    [Tooltip("Текстовое поле для отображения имени текущего уровня (опционально).")]
    [SerializeField] private TextMeshProUGUI currentLevelNameText;

    [Tooltip("Кнопка для загрузки предыдущего уровня (опционально).")]
    [SerializeField] private Button previousLevelButton;

    [Tooltip("Кнопка для загрузки следующего уровня (опционально).")]
    [SerializeField] private Button nextLevelButton;

    [Header("Динамическое создание кнопок уровней (опционально)")]
    [Tooltip("Контейнер (например, Panel с Vertical Layout Group) для размещения кнопок выбора уровня.")]
    [SerializeField] private Transform levelButtonsContainer;

    [Tooltip("Префаб кнопки для выбора уровня. Кнопка должна иметь компонент Button и Text (дочерний).")]
    [SerializeField] private GameObject levelButtonPrefab;

    void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError("[LevelSelectorUI] LevelManager не назначен!");
            enabled = false; // Отключаем скрипт, если нет LevelManager
            return;
        }

        // Подписываемся на событие смены уровня в LevelManager
        levelManager.OnLevelChanged += UpdateUI;

        // Настраиваем слушателей для кнопок "вперед/назад", если они есть
        if (previousLevelButton != null)
        {
            previousLevelButton.onClick.AddListener(levelManager.LoadPreviousLevel);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(levelManager.LoadNextLevel);
        }

        // Создаем кнопки для каждого уровня, если указан контейнер и префаб
        if (levelButtonsContainer != null && levelButtonPrefab != null)
        {
            PopulateLevelButtons();
        }

        // Обновляем UI при запуске, чтобы отобразить начальный уровень
        if (levelManager.CurrentLevelData != null)
        {
            UpdateUI(levelManager.CurrentLevelData);
        }
        else if (levelManager.GetTotalLevels() > 0)
        {
            // Если уровни есть, но CurrentLevelData еще не установлен (например, LoadLevel(0) в Start еще не отработал)
            // можно подождать или попытаться получить данные первого уровня.
            // Для простоты, если Start в LevelManager уже отработал, UpdateUI вызовется через событие.
            // Если нет, то первый UpdateUI(levelManager.CurrentLevelData) выше покроет это.
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события, чтобы избежать утечек памяти
        if (levelManager != null)
        {
            levelManager.OnLevelChanged -= UpdateUI;
        }

        // Также можно удалить слушателей с кнопок, если они были добавлены динамически
        // но для кнопок, назначенных через инспектор, это обычно не требуется, 
        // если только сам LevelManager не уничтожается раньше этого UI.
        if (previousLevelButton != null)
        {
            previousLevelButton.onClick.RemoveListener(levelManager.LoadPreviousLevel);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveListener(levelManager.LoadNextLevel);
        }
    }

    /// <summary>
    /// Обновляет элементы UI на основе данных текущего уровня.
    /// </summary>
    /// <param name="newLevelData">Данные нового загруженного уровня.</param>
    private void UpdateUI(LevelData newLevelData)
    {
        if (newLevelData == null)
        {
            Debug.LogWarning("[LevelSelectorUI] Попытка обновить UI с null LevelData.");
            if (currentLevelNameText != null)
            {
                currentLevelNameText.text = "N/A";
            }
            return;
        }

        // Обновляем текстовое поле с именем уровня
        if (currentLevelNameText != null)
        {
            currentLevelNameText.text = $"Уровень: {newLevelData.levelName}";
        }

        // Обновляем состояние кнопок "вперед/назад" (например, делаем их неактивными, если уровней мало)
        // Это больше актуально, если нет зацикливания уровней.
        // В LevelManager сейчас зацикливание, так что кнопки всегда активны, если есть хотя бы 1 уровень.
        bool canNavigate = levelManager.GetTotalLevels() > 1;
        if (previousLevelButton != null) previousLevelButton.interactable = canNavigate;
        if (nextLevelButton != null) nextLevelButton.interactable = canNavigate;

        // Можно добавить логику для подсветки активной кнопки уровня, если они создаются динамически
        if (levelButtonsContainer != null)
        {
            HighlightCurrentLevelButton(levelManager.GetCurrentLevelIndex());
        }
    }

    /// <summary>
    /// Создает кнопки для выбора каждого доступного уровня.
    /// </summary>
    private void PopulateLevelButtons()
    {
        // Очищаем старые кнопки, если они были
        foreach (Transform child in levelButtonsContainer)
        {
            Destroy(child.gameObject);
        }

        if (levelManager.GetTotalLevels() == 0)
        {
            Debug.LogWarning("[LevelSelectorUI] Нет уровней для создания кнопок выбора.");
            return;
        }

        for (int i = 0; i < levelManager.GetTotalLevels(); i++)
        {
            LevelData levelData = levelManager.allLevels[i]; // Получаем LevelData напрямую из списка
            if (levelData == null) continue;

            GameObject buttonGO = Instantiate(levelButtonPrefab, levelButtonsContainer);
            buttonGO.name = $"LevelButton_{i}_{levelData.levelName}";

            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = levelData.levelName;
            }

            Button buttonComponent = buttonGO.GetComponent<Button>();
            if (buttonComponent != null)
            {
                // Важно захватить индекс в локальную переменную для лямбда-выражения
                int levelIndex = i;
                buttonComponent.onClick.AddListener(() => levelManager.LoadLevel(levelIndex));
            }
        }
    }

    /// <summary>
    /// Подсвечивает кнопку текущего уровня (если реализовано).
    /// </summary>
    private void HighlightCurrentLevelButton(int currentLevelIndex)
    {
        if (levelButtonsContainer == null || currentLevelIndex < 0) return;

        for (int i = 0; i < levelButtonsContainer.childCount; i++)
        {
            Button button = levelButtonsContainer.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                // Простой пример: изменение цвета кнопки
                // Вам может понадобиться более сложная логика (например, отдельный спрайт для выделения)
                var colors = button.colors;
                if (i == currentLevelIndex)
                {
                    colors.normalColor = Color.green; // Цвет для активной кнопки
                    colors.highlightedColor = Color.green * 0.9f;
                }
                else
                {
                    colors.normalColor = Color.white; // Стандартный цвет
                    colors.highlightedColor = new Color(0.96f, 0.96f, 0.96f); // Стандартный Highlighted
                }
                button.colors = colors;
            }
        }
    }
}

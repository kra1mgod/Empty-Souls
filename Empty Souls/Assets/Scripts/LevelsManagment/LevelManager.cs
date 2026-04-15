using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Список всех доступных данных уровней (ScriptableObjects).")]
    [SerializeField] public List<LevelData> allLevels = new List<LevelData>();

    private int _currentLevelIndex = -1;

    public LevelData CurrentLevelData { get; private set; }

    public List<GameObject> CurrentLevelTilePrefabs => CurrentLevelData?.tilePrefabs;

    public event Action<LevelData> OnLevelChanged;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Если LevelManager должен быть один на всю игру и переноситься между сценами:
            // DontDestroyOnLoad(gameObject);
            // Но для текущей задачи (отдельные LevelManager на сценах меню и игры) это НЕ рекомендуется.
            // Если вы решите использовать DontDestroyOnLoad, убедитесь, что у вас есть логика
            // для обработки дубликатов при загрузке сцен (например, Instance != this => Destroy(gameObject)).
        }
        else if (Instance != this)
        {
            // Если другой экземпляр уже существует (например, из-за DontDestroyOnLoad или дубликата на сцене)
            Debug.LogWarning($"[LevelManager] Обнаружен еще один экземпляр LevelManager на сцене. Уничтожение этого экземпляра ({gameObject.name}). Используйте существующий.");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        bool loadedFromGameData = false;
        if (GameData.SelectedLevelIndex != -1)
        {
            if (GameData.SelectedLevelIndex >= 0 && GameData.SelectedLevelIndex < allLevels.Count)
            {
                Debug.Log($"[LevelManager] Найден сохраненный индекс уровня: {GameData.SelectedLevelIndex}. Загрузка...");
                LoadLevel(GameData.SelectedLevelIndex);
                loadedFromGameData = true;

                // Опционально: сбросить сохраненный индекс после использования,
                // чтобы при следующем обычном запуске игровой сцены (не из меню) загружался уровень по умолчанию.
                // Если вы хотите, чтобы выбор "запоминался" до следующего выбора в меню, закомментируйте следующую строку.
                // ResetSelectedLevelIndex(); 
            }
            else
            {
                Debug.LogWarning($"[LevelManager] Сохраненный индекс уровня {GameData.SelectedLevelIndex} некорректен для текущего списка уровней (размер: {allLevels.Count}). Загрузка уровня по умолчанию.");
                GameData.SelectedLevelIndex = -1; // Сбрасываем некорректный индекс
            }
        }

        if (!loadedFromGameData) // Если уровень не был загружен из GameData
        {
            if (allLevels.Count > 0)
            {
                if (_currentLevelIndex == -1) // И никакой уровень еще не был установлен
                {
                    Debug.Log("[LevelManager] Загрузка уровня по умолчанию (индекс 0).");
                    LoadLevel(0);
                }
            }
            else
            {
                Debug.LogWarning("[LevelManager] Список уровней (allLevels) пуст. Невозможно загрузить уровень.");
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= allLevels.Count)
        {
            Debug.LogError($"[LevelManager] Неверный индекс уровня: {levelIndex}. Доступно уровней: {allLevels.Count}");
            return;
        }

        // Можно убрать проверку на CurrentLevelIndex == levelIndex, если мы хотим, чтобы GameData.SelectedLevelIndex
        // всегда обновлялся, даже если выбран тот же самый уровень (например, в UI меню).
        // if (_currentLevelIndex == levelIndex && CurrentLevelData != null)
        // {
        //     Debug.Log($"[LevelManager] Уровень {allLevels[levelIndex].levelName} уже загружен. Индекс в GameData уже должен быть корректен.");
        //     // OnLevelChanged?.Invoke(CurrentLevelData); // Повторно уведомить, если нужно
        //     return;
        // }

        _currentLevelIndex = levelIndex;
        CurrentLevelData = allLevels[_currentLevelIndex];

        if (CurrentLevelData == null)
        {
            Debug.LogError($"[LevelManager] LevelData для индекса {levelIndex} не найден (null).");
            GameData.SelectedLevelIndex = -1; // Сбрасываем, так как загрузка не удалась
            return;
        }

        Debug.Log($"[LevelManager] Уровень '{CurrentLevelData.levelName}' (индекс {levelIndex}) загружен. Сохранение индекса в GameData.");
        GameData.SelectedLevelIndex = _currentLevelIndex;

        OnLevelChanged?.Invoke(CurrentLevelData);
    }

    public void LoadNextLevel()
    {
        if (allLevels.Count == 0)
        {
            Debug.LogWarning("[LevelManager] Список уровней пуст.");
            return;
        }
        int nextLevelIndex = (_currentLevelIndex + 1) % allLevels.Count;
        LoadLevel(nextLevelIndex);
    }

    public void LoadPreviousLevel()
    {
        if (allLevels.Count == 0)
        {
            Debug.LogWarning("[LevelManager] Список уровней пуст.");
            return;
        }
        int previousLevelIndex = (_currentLevelIndex - 1 + allLevels.Count) % allLevels.Count;
        LoadLevel(previousLevelIndex);
    }

    public int GetTotalLevels() => allLevels.Count;
    public int GetCurrentLevelIndex() => _currentLevelIndex;

    /// <summary>
    /// Сбрасывает сохраненный в GameData индекс выбранного уровня.
    /// Следующая загрузка LevelManager (без предварительного выбора в меню) будет использовать уровень по умолчанию.
    /// </summary>
    public static void ResetSelectedLevelIndex()
    {
        Debug.Log("[LevelManager] Сброс GameData.SelectedLevelIndex на -1.");
        GameData.SelectedLevelIndex = -1;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        // Отписываться от OnLevelChanged не нужно, т.к. это событие этого же класса.
        // Если бы были подписки на внешние события, здесь было бы место для отписки.
    }
}

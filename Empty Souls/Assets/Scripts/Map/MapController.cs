using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    // public GameObject[] tilePrefabs; // Старая система: тайлы задаются здесь напрямую
    [Tooltip("Ссылка на LevelManager для получения тайлов текущего уровня.")]
    public LevelManager levelManager; // Новая система: ссылка на LevelManager
    public float tileSize = 1f;
    public Transform player;
    public Camera mainCamera;
    public int chunkTilesX = 16; // Размер чанка по X (в тайлах)
    public int chunkTilesY = 16; // Размер чанка по Y (в тайлах)
    public int viewRadius = 4; // Сколько чанков в каждую сторону от игрока (2 — всего 5x5 чанков)

    // Кэш чанков
    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();

    private List<GameObject> currentLevelTilePrefabs;

    void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError("[ChunkManager] LevelManager не назначен! Пожалуйста, назначьте его в инспекторе. Генерация тайлов не будет работать корректно.");
            enabled = false; // Отключаем ChunkManager, если нет LevelManager
            return;
        }
        levelManager.OnLevelChanged += HandleLevelChanged;
        // Первоначальная загрузка тайлов для текущего уровня
        // LevelManager сам вызовет OnLevelChanged при загрузке первого уровня в своем Start,
        // поэтому HandleLevelChanged здесь вызывать не обязательно, если LevelManager есть на сцене и активен.
        // Но для надежности, если LevelManager мог уже загрузить уровень до того, как этот Start выполнится:
        if (levelManager.CurrentLevelData != null)
        {
            HandleLevelChanged(levelManager.CurrentLevelData);
        }
        // Если CurrentLevelData еще null, то событие OnLevelChanged из LevelManager позаботится об этом.
    }

    void Update()
    {
        if (player == null || mainCamera == null || levelManager == null) return;
        // Проверяем, есть ли тайлы для текущего уровня. currentLevelTilePrefabs может быть не null, но пустым.
        if (currentLevelTilePrefabs == null || currentLevelTilePrefabs.Count == 0)
        {
            // Можно добавить Debug.LogWarning один раз, если это состояние длится долго
            return;
        }

        Vector2 playerPos = player.position;
        Vector2Int playerChunkCoord = WorldToChunkCoord(playerPos);

        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();
        for (int dx = -viewRadius; dx <= viewRadius; dx++)
        {
            for (int dy = -viewRadius; dy <= viewRadius; dy++)
            {
                Vector2Int coord = new Vector2Int(playerChunkCoord.x + dx, playerChunkCoord.y + dy);
                neededChunks.Add(coord);

                if (!loadedChunks.ContainsKey(coord))
                {
                    if (chunkPrefab == null)
                    {
                        Debug.LogError("[ChunkManager] Префаб чанка (chunkPrefab) не назначен!");
                        enabled = false; // Отключаем, чтобы не спамить ошибками
                        return;
                    }
                    GameObject chunkObj = Instantiate(chunkPrefab, WorldChunkCoordToWorldPos(coord), Quaternion.identity, transform);
                    Chunk chunk = chunkObj.GetComponent<Chunk>();
                    if (chunk != null)
                    {
                        chunk.tilePrefabs = currentLevelTilePrefabs.ToArray();
                    }
                    else
                    {
                        Debug.LogError($"[ChunkManager] Префаб чанка {chunkPrefab.name} не содержит компонент Chunk!");
                        Destroy(chunkObj); // Уничтожаем, если чанк некорректный
                        continue;
                    }
                    chunk.width = chunkTilesX;
                    chunk.height = chunkTilesY;

                    if (currentLevelTilePrefabs.Count > 0)
                    {
                        chunk.Generate(coord, tileSize);
                    }
                    else
                    {
                        // Эта ситуация уже покрывается проверкой в начале Update
                        // Debug.LogWarning($"[ChunkManager] Невозможно сгенерировать чанк {coord}, т.к. нет тайлов для текущего уровня.");
                    }
                    loadedChunks.Add(coord, chunkObj);
                }
            }
        }

        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in loadedChunks)
        {
            if (!neededChunks.Contains(kvp.Key))
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var coord in toRemove)
            loadedChunks.Remove(coord);
    }

    private void HandleLevelChanged(LevelData newLevelData)
    {
        if (newLevelData == null)
        {
            Debug.LogWarning("[ChunkManager] Получены null LevelData при смене уровня. Тайлы не будут обновлены. Очистка существующих чанков.");
            currentLevelTilePrefabs = new List<GameObject>();
            ClearAllChunks();
            return;
        }

        Debug.Log($"[ChunkManager] Уровень изменился на '{newLevelData.levelName}'. Обновление списка тайлов.");
        currentLevelTilePrefabs = newLevelData.tilePrefabs;

        if (currentLevelTilePrefabs == null) // Проверяем на null перед .Count
        {
            Debug.LogWarning($"[ChunkManager] Список tilePrefabs для уровня '{newLevelData.levelName}' равен null. Предыдущие чанки будут очищены.");
            currentLevelTilePrefabs = new List<GameObject>(); // Инициализируем пустым списком
        }
        else if (currentLevelTilePrefabs.Count == 0)
        {
            Debug.LogWarning($"[ChunkManager] Для уровня '{newLevelData.levelName}' не заданы тайлы в LevelData (список пуст). Предыдущие чанки будут очищены.");
        }

        ClearAllChunks();
    }

    Vector2Int WorldToChunkCoord(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / (chunkTilesX * tileSize));
        int y = Mathf.FloorToInt(worldPos.y / (chunkTilesY * tileSize));
        return new Vector2Int(x, y);
    }

    Vector3 WorldChunkCoordToWorldPos(Vector2Int chunkCoord)
    {
        return new Vector3(
            chunkCoord.x * chunkTilesX * tileSize,
            chunkCoord.y * chunkTilesY * tileSize,
            0
        );
    }

    private void ClearAllChunks()
    {
        if (loadedChunks == null) return;
        foreach (var kvp in loadedChunks)
        {
            if (kvp.Value != null)
            {
                Destroy(kvp.Value);
            }
        }
        loadedChunks.Clear();
        Debug.Log("[ChunkManager] Все существующие чанки очищены.");
    }

    void OnDestroy()
    {
        if (levelManager != null)
        {
            levelManager.OnLevelChanged -= HandleLevelChanged;
        }
    }
}

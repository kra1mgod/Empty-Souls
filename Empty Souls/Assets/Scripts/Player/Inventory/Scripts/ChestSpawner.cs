using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [Tooltip("Префаб сундука, который будет появляться")]
    public GameObject chestPrefab;

    [Tooltip("Область, в которой могут появляться сундуки (BoxCollider2D)")]
    public Collider2D spawnArea;

    [Tooltip("Минимальный интервал времени между появлениями сундуков (в секундах)")]
    public float minSpawnInterval = 10f;

    [Tooltip("Максимальный интервал времени между появлениями сундуков (в секундах)")]
    public float maxSpawnInterval = 30f;

    private float nextSpawnTime;

    void Start()
    {
        if (chestPrefab == null)
        {
            Debug.LogError("ChestPrefab не назначен в ChestSpawner!");
            enabled = false; // Отключаем скрипт, если префаб не назначен
            return;
        }

        if (spawnArea == null)
        {
            Debug.LogError("SpawnArea не назначена в ChestSpawner!");
            enabled = false; // Отключаем скрипт, если зона спавна не назначена
            return;
        }

        // Устанавливаем время первого спавна
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnChest();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnTime = Time.time + spawnInterval;
        Debug.Log($"Следующий сундук появится через: {spawnInterval} сек.");
    }

    void SpawnChest()
    {
        if (spawnArea == null || chestPrefab == null) return;

        Bounds spawnBounds = spawnArea.bounds;
        float randomX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        float randomY = Random.Range(spawnBounds.min.y, spawnBounds.max.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Проверяем, находится ли случайная точка внутри коллайдера (на случай, если коллайдер не прямоугольный)
        // Для BoxCollider2D это избыточно, но полезно для других форм коллайдеров
        if (spawnArea.OverlapPoint(spawnPosition))
        {
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Сундук появился в: {spawnPosition}");
        }
        else
        {
            // Если точка оказалась вне сложного коллайдера, можно попробовать еще раз или выбрать центр
            Debug.LogWarning($"Случайная точка {spawnPosition} оказалась вне spawnArea. Сундук не создан. Попробуйте увеличить spawnArea или убедитесь, что она корректно настроена.");
            // В качестве запасного варианта, можно спавнить по центру, если OverlapPoint не сработает
            // Instantiate(chestPrefab, spawnBounds.center, Quaternion.identity);
        }
    }
}

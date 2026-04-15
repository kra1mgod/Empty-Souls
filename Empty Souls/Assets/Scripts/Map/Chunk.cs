using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject[] tilePrefabs; // 3 типа тайлов
    public int width = 16;
    public int height = 16;

    private float[] tileWeights = new float[] { 0.6f, 0.35f, 0.05f }; // Сумма = 1.0
    public void Generate(Vector2Int chunkCoord, float tileSize)
    {
        System.Random rand = new System.Random(chunkCoord.x * 10000 + chunkCoord.y);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int prefabIndex = GetRandomTileIndex(rand);
                // Важно: Убедиться, что prefabIndex не выходит за пределы массива tilePrefabs
                if (tilePrefabs != null && tilePrefabs.Length > 0 && prefabIndex < tilePrefabs.Length && tilePrefabs[prefabIndex] != null)
                {
                    Vector3 pos = new Vector3(transform.position.x + x * tileSize, transform.position.y + y * tileSize, 0);
                    Instantiate(tilePrefabs[prefabIndex], pos, Quaternion.identity, transform);
                }
                else if (tilePrefabs == null || tilePrefabs.Length == 0)
                {
                    // Debug.LogWarning($"[Chunk] tilePrefabs не задан или пуст для чанка {chunkCoord}.");
                    // Можно пропустить генерацию тайла или вывести более заметное сообщение
                }
                else if (prefabIndex >= tilePrefabs.Length)
                {
                    // Debug.LogWarning($"[Chunk] prefabIndex ({prefabIndex}) выходит за пределы tilePrefabs ({tilePrefabs.Length}) для чанка {chunkCoord}. Используется последний доступный тайл.");
                    // prefabIndex = tilePrefabs.Length - 1; // Запасной вариант - использовать последний тайл
                    // Vector3 pos = new Vector3(transform.position.x + x * tileSize, transform.position.y + y * tileSize, 0);
                    // Instantiate(tilePrefabs[prefabIndex], pos, Quaternion.identity, transform);
                    // Лучше убедиться, что tileWeights соответствует количеству tilePrefabs
                }
            }
        }
    }
    int GetRandomTileIndex(System.Random rand)
    {
        float value = (float)rand.NextDouble();
        float sum = 0f;

        // Если tileWeights не соответствует количеству tilePrefabs, могут быть проблемы.
        // Идеально, если tileWeights.Length == tilePrefabs.Length
        // Пока что оставляем как есть, но это потенциальное место для улучшения/проверки
        int count = Mathf.Min(tileWeights.Length, tilePrefabs != null ? tilePrefabs.Length : 0);
        if (count == 0) return 0; // Нечего выбирать

        for (int i = 0; i < count; i++)
        {
            sum += tileWeights[i]; // Используем веса только для доступных тайлов
            if (value < sum)
                return i;
        }
        return count - 1; // fallback
    }
}

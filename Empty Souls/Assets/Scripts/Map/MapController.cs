using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject[] tilePrefabs; // Три типа тайлов
    public float tileSize = 1f;
    public Transform player;
    public Camera mainCamera;
    public int chunkTilesX = 16; // Размер чанка по X (в тайлах)
    public int chunkTilesY = 16; // Размер чанка по Y (в тайлах)
    public int viewRadius = 4; // Сколько чанков в каждую сторону от игрока (2 — всего 5x5 чанков)

    // Кэш чанков
    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();
    private float[] tileWeights = new float[] { 0.6f, 0.35f, 0.05f }; // Сумма = 1.0

    

    void Update()
    {
        if (player == null || mainCamera == null) return;

        Vector2 playerPos = player.position;
        Vector2Int playerChunkCoord = WorldToChunkCoord(playerPos);

        // 1. Генерируем чанки в радиусе viewRadius от центра
        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();
        for (int dx = -viewRadius; dx <= viewRadius; dx++)
        {
            for (int dy = -viewRadius; dy <= viewRadius; dy++)
            {
                Vector2Int coord = new Vector2Int(playerChunkCoord.x + dx, playerChunkCoord.y + dy);
                neededChunks.Add(coord);

                if (!loadedChunks.ContainsKey(coord))
                {
                    Vector3 chunkOrigin = new Vector3(
                        coord.x * chunkTilesX * tileSize,
                        coord.y * chunkTilesY * tileSize,
                        0
                    );
                    GameObject chunkObj = Instantiate(chunkPrefab, chunkOrigin, Quaternion.identity, transform);
                    Chunk chunk = chunkObj.GetComponent<Chunk>();
                    chunk.tilePrefabs = tilePrefabs;
                    chunk.width = chunkTilesX;
                    chunk.height = chunkTilesY;
                    chunk.Generate(coord, tileSize);
                    loadedChunks.Add(coord, chunkObj);
                }
            }
        }

        // 2. Удаляем чанки, которые больше не нужны
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

    // Переводит мировые координаты в координаты чанка
    Vector2Int WorldToChunkCoord(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / (chunkTilesX * tileSize));
        int y = Mathf.FloorToInt(worldPos.y / (chunkTilesY * tileSize));
        return new Vector2Int(x, y);
    }
}
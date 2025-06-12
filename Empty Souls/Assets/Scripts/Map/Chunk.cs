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
                Vector3 pos = new Vector3(transform.position.x + x * tileSize, transform.position.y + y * tileSize, 0);
                Instantiate(tilePrefabs[prefabIndex], pos, Quaternion.identity, transform);
            }
        }
    }
    int GetRandomTileIndex(System.Random rand)
    {
        float value = (float)rand.NextDouble();
        float sum = 0f;
        for (int i = 0; i < tileWeights.Length; i++)
        {
            sum += tileWeights[i];
            if (value < sum)
                return i;
        }
        return tileWeights.Length - 1; // fallback
    }
}
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public float spawnDistance = 12f;

    public GameObject SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogError("BossSpawner: bossPrefab not assigned!");
            return null;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = player != null ? (Vector2)player.transform.position : Vector2.zero;

        // Спавним босса справа от игрока на фиксированное расстояние
        Vector2 spawnPos = playerPos + Vector2.right * spawnDistance;
        return Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }
}
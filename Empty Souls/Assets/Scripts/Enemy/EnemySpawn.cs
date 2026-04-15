using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRadius = 10f;
    private Transform player;

    public void SpawnEnemies(List<EnemySpawnInfo> enemySpawns)
    {
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj) player = playerObj.transform;
        }
        StartCoroutine(SpawnRoutine(enemySpawns));
    }

    private IEnumerator SpawnRoutine(List<EnemySpawnInfo> enemySpawns)
    {
        foreach (var info in enemySpawns)
        {
            for (int i = 0; i < info.count; i++)
            {
                Vector2 spawnPos = (player != null)
                    ? (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius
                    : (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
                Instantiate(info.enemyPrefab, spawnPos, Quaternion.identity);
                if (info.delay > 0f && i < info.count - 1)
                    yield return new WaitForSeconds(info.delay);
            }
        }
    }
}
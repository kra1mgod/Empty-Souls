using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float interval = 2f;
    public float spawnRadius = 10f;
    private float timer;

    void Update()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: enemyPrefab not assigned!");
            return;
        }

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            Vector2 playerPos = Vector2.zero;
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerPos = playerObj.transform.position;

            Vector2 spawnPos = playerPos + Random.insideUnitCircle.normalized * spawnRadius;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}
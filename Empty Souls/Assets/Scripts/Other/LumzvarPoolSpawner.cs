using UnityEngine;

public class LumzvarPoolSpawner : MonoBehaviour
{
    public GameObject lumzvarPoolPrefab;
    public float spawnInterval = 10f;
    public float spawnRadius = 7f;
    public int maxPools = 3;

    private float timer = 0f;
    private int currentPools = 0;

    void Update()
    {
        timer += Time.deltaTime;
        // ¬место currentPools Ч считаем реальные объекты на сцене
        int actualPools = FindObjectsOfType<LumzvarPool>().Length;
        if (timer >= spawnInterval && actualPools < maxPools)
        {
            timer = 0f;
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(lumzvarPoolPrefab, spawnPos, Quaternion.identity);
        }
    }
}
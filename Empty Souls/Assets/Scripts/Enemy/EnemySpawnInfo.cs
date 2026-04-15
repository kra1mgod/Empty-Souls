using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab; // Префаб врага
    public int count = 1;          // Сколько заспавнить
    public float delay = 0f;       // Задержка между спавнами этой группы
}
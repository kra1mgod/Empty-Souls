using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public string waveName = "Wave";
    public float duration = 30f; // Длительность волны (сек)
    public List<EnemySpawnInfo> enemySpawns = new List<EnemySpawnInfo>();
}
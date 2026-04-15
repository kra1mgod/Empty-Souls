using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level System/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Tooltip("Имя уровня, отображаемое в UI.")]
    public string levelName = "New Level";

    [Tooltip("Музыка для уровня.")]
    public AudioClip levelMusic;

    [Tooltip("Список волн на этом уровне")]
    public List<WaveData> waves = new List<WaveData>();

    [Tooltip("Список префабов тайлов, которые могут использоваться на этом уровне.")]
    public List<GameObject> tilePrefabs = new List<GameObject>();

    // Можно добавить bossPrefab, если нужен уникальный босс для уровня
    // public GameObject bossPrefab;
}
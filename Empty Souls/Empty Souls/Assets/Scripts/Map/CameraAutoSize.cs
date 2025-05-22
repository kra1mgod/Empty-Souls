using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoSize : MonoBehaviour
{
    public float tilesOnScreenY = 10f; // Сколько тайлов по высоте видно (подбери под желаемый размер персонажа)
    public float tileSize = 1f; // Размер одного тайла в юнитах

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = (tilesOnScreenY * tileSize) / 2f;
    }
}
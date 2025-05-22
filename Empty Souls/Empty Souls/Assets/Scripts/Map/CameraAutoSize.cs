using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoSize : MonoBehaviour
{
    public float tilesOnScreenY = 10f; // ������� ������ �� ������ ����� (������� ��� �������� ������ ���������)
    public float tileSize = 1f; // ������ ������ ����� � ������

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = (tilesOnScreenY * tileSize) / 2f;
    }
}
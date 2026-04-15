// Этот скрипт можно повесить на Main Camera
using UnityEngine;

public class CameraAutoSize : MonoBehaviour
{
    public float targetWidth = 1920f;
    public float targetHeight = 1080f;
    public float baseSize = 1f; // Твое стартовое значение, можно менять в инспекторе

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float targetAspect = targetWidth / targetHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        cam.orthographicSize = baseSize; // <-- тут твой базовый отдалённый размер

        if (scaleHeight < 1.0f)
        {
            cam.orthographicSize = baseSize / scaleHeight;
        }
    }
}
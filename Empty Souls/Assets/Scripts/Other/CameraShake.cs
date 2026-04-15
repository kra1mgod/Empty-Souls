using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.5f;

    private Vector3 originalPosition;
    private float currentShakeTime = 0f;

    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentShakeTime > 0)
        {
            transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            currentShakeTime -= Time.unscaledDeltaTime;
            if (currentShakeTime <= 0)
                transform.localPosition = originalPosition;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeTime = duration;
    }
}
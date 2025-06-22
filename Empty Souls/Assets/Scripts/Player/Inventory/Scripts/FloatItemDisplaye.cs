using UnityEngine;
using UnityEngine.UI;

public class FloatingItemDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TMPro.TMP_Text nameText;

    [Header("Floating Settings")]
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1.5f;
    public float glowSpeed = 3.5f;
    public float glowStrength = 0.5f;

    [Header("Lifetime")]
    public float displayTime = 2f;

    private Color baseColor;
    private float timer;
    private Vector3 startPos;
    private Transform target;

    void Awake()
    {
        timer = 0f;
        if (iconImage != null)
            baseColor = iconImage.color;
    }

    // Исправленный метод
    public void Setup(ItemData itemSO, Transform playerTransform)
    {
        if (iconImage != null)
            iconImage.sprite = itemSO.icon;
        if (nameText != null)
            nameText.text = itemSO.displayName;
        target = playerTransform;
        if (target != null)
            startPos = target.position + Vector3.up * 2.2f;
        else
            startPos = transform.position;
        transform.position = startPos;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (target != null)
            startPos = target.position + Vector3.up * 2.2f;
        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + Vector3.up * offsetY;

        if (iconImage != null)
        {
            float glow = (Mathf.Sin(Time.time * glowSpeed) + 1f) * 0.5f * glowStrength;
            var c = baseColor;
            c.a = Mathf.Clamp01(baseColor.a + glow);
            iconImage.color = c;
        }

        if (timer >= displayTime)
            Destroy(gameObject);
    }
}
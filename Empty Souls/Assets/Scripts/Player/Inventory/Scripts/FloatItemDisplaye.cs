using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingItemDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TMP_Text nameText;

    [Header("Floating Settings")]
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1.5f;
    public float glowSpeed = 3.5f;
    public float glowStrength = 0.5f;

    [Header("Lifetime")]
    public float displayTime = 2f;

    private Color baseColor = Color.white;
    private float timer;
    private Vector3 startPos;
    private Transform target;
    private Canvas myCanvas;

    void Awake()
    {
        timer = 0f;
        if (iconImage != null)
            baseColor = iconImage.color;

        myCanvas = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// Call this right after Instantiate. Pass item data and transform of the player (or world object).
    /// </summary>
    public void Setup(ItemData itemSO, Transform playerTransform)
    {
        if (iconImage != null && itemSO.icon != null)
            iconImage.sprite = itemSO.icon;
        if (nameText != null && !string.IsNullOrEmpty(itemSO.displayName))
            nameText.text = itemSO.displayName;
        target = playerTransform;
        if (target != null)
            startPos = target.position + Vector3.up * 2.2f;
        else
            startPos = transform.position;
        transform.position = startPos;

        // Если myCanvas в World Space, позиционируем нормально:
        if (myCanvas != null && myCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Для Overlay Canvas позиционируется в экранных координатах, иначе — в world space.
            Vector3 screenPos = Camera.main.WorldToScreenPoint(startPos);
            transform.position = screenPos;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Поддержка движения за целью (игроком)
        if (target != null)
            startPos = target.position + Vector3.up * 2.2f;

        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 worldPos = startPos + Vector3.up * offsetY;

        if (myCanvas != null && myCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPos = Camera.main != null ? Camera.main.WorldToScreenPoint(worldPos) : worldPos;
            transform.position = screenPos;
        }
        else
        {
            transform.position = worldPos;
        }

        // Glow-эффект на иконке
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
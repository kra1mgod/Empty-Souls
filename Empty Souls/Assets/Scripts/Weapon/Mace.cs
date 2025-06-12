using UnityEngine;

public class MaceWeapon : MonoBehaviour
{
    public GameObject macePrefab;
    public float radius = 2f;
    public float rotateSpeed = 180f;

    [Header("Stat System")]
    public AttributeType mainAttribute = AttributeType.Strength; // ќсновной стат дл€ булавы Ч сила
    public PlayerStats playerStats; // —сылка на статы персонажа

    private GameObject maceInstance;
    private float angle = 0f;
    private LineRenderer line;

    void Awake()
    {
        if (playerStats == null)
            playerStats = GetComponentInParent<PlayerStats>();

        line = GetComponent<LineRenderer>();
        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();

        // ¬ажно: задаЄм материал!
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = 2;
        line.startColor = Color.white;
        line.endColor = Color.white;

        // —ортировка (чтобы было видно)
        line.sortingLayerName = "Player";
        line.sortingOrder = 10;
    }

    void OnEnable()
    {
        if (maceInstance == null && macePrefab != null)
        {
            maceInstance = Instantiate(macePrefab, transform);
            // ѕередаЄм ссылку на статы и основной стат в скрипт урона
            var hity = maceInstance.GetComponent<MaceHity>();
            if (hity != null)
            {
                hity.playerStats = playerStats;
                hity.mainAttribute = mainAttribute;
            }
        }
        angle = 0f;
        UpdateMacePosition();
        if (maceInstance != null)
            maceInstance.SetActive(true);
        if (line != null)
            line.enabled = true;
    }

    void OnDisable()
    {
        if (maceInstance != null)
            maceInstance.SetActive(false);
        if (line != null)
            line.enabled = false;
    }

    void Update()
    {
        if (maceInstance == null) return;
        angle += rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
        UpdateMacePosition();
        DrawChain();
    }

    void UpdateMacePosition()
    {
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        maceInstance.transform.localPosition = new Vector3(x, y, 0);
    }

    void DrawChain()
    {
        if (line != null && maceInstance != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, maceInstance.transform.position);
        }
    }
}
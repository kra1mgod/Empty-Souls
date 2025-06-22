using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Image

public class ChestCompass : MonoBehaviour
{
    [Tooltip("UI Image элемент, который будет использоваться в качестве стрелки компаса")]
    public Image compassNeedle;

    [Tooltip("Transform игрока, относительно которого будет искаться ближайший сундук")]
    public Transform playerTransform;

    [Tooltip("Максимальная дистанция, на которой компас будет обнаруживать сундуки")]
    public float maxDistance = 100f;

    [Tooltip("Тег, используемый для идентификации объектов сундуков")]
    public string chestTag = "Chest";

    private GameObject closestChest = null;

    void Start()
    {
        if (compassNeedle == null)
        {
            Debug.LogError("CompassNeedle не назначен в ChestCompass!");
            enabled = false;
            return;
        }
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform не назначен в ChestCompass!");
            enabled = false;
            return;
        }
        if (string.IsNullOrEmpty(chestTag))
        {
            Debug.LogError("ChestTag не указан в ChestCompass!");
            enabled = false;
            return;
        }

        // Изначально стрелка компаса может быть невидимой или указывать вверх
        compassNeedle.gameObject.SetActive(false);
    }

    void Update()
    {
        FindClosestChest();
        UpdateCompassNeedle();
    }

    void FindClosestChest()
    {
        GameObject[] chests = GameObject.FindGameObjectsWithTag(chestTag);
        closestChest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject chest in chests)
        {
            float distanceToChest = Vector3.Distance(playerTransform.position, chest.transform.position);
            if (distanceToChest < minDistance)
            {
                minDistance = distanceToChest;
                closestChest = chest;
            }
        }
    }

    void UpdateCompassNeedle()
    {
        if (closestChest == null)
        {
            compassNeedle.gameObject.SetActive(false);
            return;
        }

        float distanceToClosestChest = Vector3.Distance(playerTransform.position, closestChest.transform.position);

        if (distanceToClosestChest <= maxDistance)
        {
            compassNeedle.gameObject.SetActive(true);

            // Рассчитываем направление от игрока к сундуку
            Vector3 directionToChest = (closestChest.transform.position - playerTransform.position).normalized;

            // Рассчитываем угол для поворота стрелки компаса
            float angle = Mathf.Atan2(directionToChest.y, directionToChest.x) * Mathf.Rad2Deg;
            compassNeedle.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            compassNeedle.gameObject.SetActive(false);
        }
    }
}

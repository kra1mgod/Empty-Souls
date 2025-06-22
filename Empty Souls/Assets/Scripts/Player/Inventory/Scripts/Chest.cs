using UnityEngine;

public class Chest : MonoBehaviour
{
    public ItemData[] possibleItems;
    public GameObject floatingItemPrefab;
    private bool opened = false;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!opened && other.CompareTag("Player"))
        {
            opened = true;
            if (animator != null)
                animator.SetTrigger("Open");

            GiveRandomItem(other.transform);

            Destroy(gameObject, 1.2f); // 1.2 Ч длина анимации в секундах

        }
    }

    void GiveRandomItem(Transform player)
    {
        if (possibleItems.Length == 0) return;
        var item = possibleItems[Random.Range(0, possibleItems.Length)];
        InventorySystem.Instance.AddItem(item);
        var display = Instantiate(floatingItemPrefab);
        display.GetComponent<FloatingItemDisplay>().Setup(item, player);
    }

}
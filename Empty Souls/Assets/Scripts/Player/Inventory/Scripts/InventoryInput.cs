using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryUI.Instance.panel.activeSelf)
                InventoryUI.Instance.Hide();
            else
                InventoryUI.Instance.Show();
        }
    }
}
using UnityEngine;
using UnityEngine.UI; // Required for UI.Text

public class SoulFragmentUI : MonoBehaviour
{
    public Text soulFragmentText; // Assign a UI Text element in the Inspector

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("SoulFragmentUI: PlayerStats not found in scene!");
            if (soulFragmentText != null)
                soulFragmentText.text = "Error";
            enabled = false; // Disable script if PlayerStats isn't found
            return;
        }

        // Subscribe to the event
        playerStats.OnSoulFragmentsChanged += UpdateUI;

        // Initial UI Update
        UpdateUI(playerStats.soulFragments);
    }

    void OnDestroy()
    {
        // Unsubscribe when this UI object is destroyed
        if (playerStats != null)
        {
            playerStats.OnSoulFragmentsChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int currentFragments)
    {
        if (soulFragmentText != null)
        {
            soulFragmentText.text = $"SF: {currentFragments}"; // Or any format you prefer, e.g., just the number
        }
    }
}

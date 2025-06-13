using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider or Image

public class LumzvarBar : MonoBehaviour
{
    public Slider lumzvarSlider; // Assign a UI Slider in the Inspector
    public Text lumzvarText;     // Optional: Assign a UI Text to show progress (e.g., "50/100")

    private PlayerStats playerStats; // Reference to PlayerStats to get Lumzvar values

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("LumzvarBar: PlayerStats not found in scene!");
            enabled = false; // Disable script if PlayerStats isn't found
            return;
        }

        // Subscribe to an event in PlayerStats that fires when Lumzvar changes
        // This assumes PlayerStats will have an event like: public event System.Action<int, int> OnLumzvarChanged;
        // (currentLumzvar, maxLumzvar)
        // If PlayerStats directly calls an update method on this bar, event subscription is not needed.
        // For now, we'll assume PlayerStats will call an update method.

        UpdateBar(playerStats.GetCurrentLumzvar(), playerStats.GetLumzvarForNextEvolution());
    }

    // This method should be called by PlayerStats whenever Lumzvar points change or max Lumzvar changes
    public void UpdateBar(int currentLumzvar, int maxLumzvar)
    {
        if (lumzvarSlider != null)
        {
            lumzvarSlider.maxValue = maxLumzvar;
            lumzvarSlider.value = currentLumzvar;
        }

        if (lumzvarText != null)
        {
            lumzvarText.text = $"{currentLumzvar} / {maxLumzvar}";
        }
    }
}

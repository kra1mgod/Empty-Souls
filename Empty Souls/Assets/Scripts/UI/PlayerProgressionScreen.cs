using UnityEngine;
using UnityEngine.UI; // Required for UI.Text

public class PlayerProgressionScreen : MonoBehaviour
{
    public GameObject progressionPanel; // Assign the main panel GameObject in Inspector
    public Text soulFragmentsText;    // Assign a UI Text for soul fragments display

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerProgressionScreen: PlayerStats not found in scene!");
            // Optionally disable the panel or show an error if PlayerStats is crucial
            if (progressionPanel != null) progressionPanel.SetActive(false);
            enabled = false;
            return;
        }

        // Subscribe to soul fragment changes
        playerStats.OnSoulFragmentsChanged += UpdateSoulFragmentsDisplay;

        // Initial setup: ensure panel is hidden and update display if it's initially active (though unlikely for a popup)
        if (progressionPanel != null && progressionPanel.activeSelf)
        {
            UpdateSoulFragmentsDisplay(playerStats.soulFragments);
        }
        else if (progressionPanel != null)
        {
            progressionPanel.SetActive(false); // Start with the panel hidden
        }
    }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnSoulFragmentsChanged -= UpdateSoulFragmentsDisplay;
        }
    }

    public void OpenProgressionScreen()
    {
        if (progressionPanel != null)
        {
            progressionPanel.SetActive(true);
            if (playerStats != null) // Ensure playerStats is available
            {
                UpdateSoulFragmentsDisplay(playerStats.soulFragments);
            }
            else
            {
                 if(soulFragmentsText != null) soulFragmentsText.text = "SF: Error";
            }
            // Potentially pause game Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("PlayerProgressionScreen: Progression Panel GameObject is not assigned.");
        }
    }

    public void CloseProgressionScreen()
    {
        if (progressionPanel != null)
        {
            progressionPanel.SetActive(false);
            // Potentially resume game Time.timeScale = 1f;
        }
    }

    private void UpdateSoulFragmentsDisplay(int currentFragments)
    {
        if (soulFragmentsText != null)
        {
            soulFragmentsText.text = $"Soul Fragments: {currentFragments}";
        }
    }

    // Example of how a button might toggle this screen
    // public void ToggleProgressionScreen()
    // {
    //    if (progressionPanel != null)
    //    {
    //        bool isActive = !progressionPanel.activeSelf;
    //        progressionPanel.SetActive(isActive);
    //        if (isActive && playerStats != null)
    //        {
    //            UpdateSoulFragmentsDisplay(playerStats.soulFragments);
    //        }
    //
    //        // Handle game pause/resume
    //        // Time.timeScale = isActive ? 0f : 1f;
    //    }
    // }
}

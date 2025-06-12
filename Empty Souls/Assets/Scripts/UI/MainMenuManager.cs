using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
#if UNITY_EDITOR
using UnityEditor; // Required for Application.Quit in editor
#endif

public class MainMenuManager : MonoBehaviour
{
    // Public string for the game scene name to make it easily configurable in Inspector
    public string gameSceneName = "SampleScene"; // Default to SampleScene, adjust if your game scene is named differently

    // --- Public methods to be called by UI Buttons ---

    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game scene name is not set in MainMenuManager.");
        }
    }

    public void OpenHelp()
    {
        // Placeholder: Implement Help screen logic
        Debug.Log("Help button clicked - Help screen not yet implemented.");
        // Example: Load a Help scene or activate a Help UI panel
        // if (SceneManager.GetSceneByName("HelpScene") != null) SceneManager.LoadScene("HelpScene");
        // else if (helpPanel != null) helpPanel.SetActive(true);
    }

    public void OpenPlayerStats()
    {
        // Placeholder: Implement Player Stats screen logic
        Debug.Log("Player Stats button clicked - Player Stats screen not yet implemented.");
        // Example: Load a PlayerStats scene or activate a PlayerStats UI panel
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked.");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stops play mode in editor
#else
        Application.Quit(); // Quits the built application
#endif
    }

    // --- Optional: UI Panel References (if using panels within the same scene) ---
    // public GameObject helpPanel;
    // public GameObject playerStatsPanel;

    // void Start()
    // {
    //    if (helpPanel != null) helpPanel.SetActive(false);
    //    if (playerStatsPanel != null) playerStatsPanel.SetActive(false);
    // }
}

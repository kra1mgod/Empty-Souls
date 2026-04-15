using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameStateManager
{
    public static bool IsBossFight = false;

    public static void LoadScene(int sceneIndex)
    {
        if (IsBossFight)
        {
            Debug.Log("Cannot change scene during boss fight!");
            return;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    public static void LoadScene(string sceneName)
    {
        if (IsBossFight)
        {
            Debug.Log("Cannot change scene during boss fight!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public static void StartBossFight()
    {
        IsBossFight = true;
        Debug.Log("Boss fight started - scene changes blocked");
    }

    public static void EndBossFight()
    {
        IsBossFight = false;
        Debug.Log("Boss fight ended - scene changes allowed");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndPanel : MonoBehaviour
{
    public GameObject panelRoot; // Корневой объект панели (назначь в инспекторе)
    public TextMeshProUGUI headerText; // Текст "ПОБЕДА" или "ПОРАЖЕНИЕ"
    public TextMeshProUGUI infoText; // Можно для времени/статы/награды (необяз.)
    public string mainMenuSceneName = "MainMenu"; // Название сцены меню

    void Awake()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    /// <summary>
    /// Показывает панель конца игры
    /// </summary>
    public void ShowVictory(string extraInfo = null)
    {
        if (panelRoot != null) panelRoot.SetActive(true);
        if (headerText != null) headerText.text = "ПОБЕДА!";
        if (infoText != null && extraInfo != null) infoText.text = extraInfo;
        else if (infoText != null) infoText.text = "";
        Time.timeScale = 0f;
    }

    public void ShowDefeat(string extraInfo = null)
    {
        if (panelRoot != null) panelRoot.SetActive(true);
        if (headerText != null) headerText.text = "ПОРАЖЕНИЕ";
        if (infoText != null && extraInfo != null) infoText.text = extraInfo;
        else if (infoText != null) infoText.text = "";
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
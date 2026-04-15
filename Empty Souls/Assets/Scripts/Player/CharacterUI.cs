using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject panel; // WeaponSelectPanel
    public Button runeButton;
    public Button katanaButton;
    public Button maceButton;
    public GameObject lumzvarBar; // Перетащи объект LumzvarBar в инспекторе
    public GameObject waveTimer;  // Перетащи сюда объект таймера волны (Text или любой контейнер)

    void Awake()
    {
        if (lumzvarBar != null)
            lumzvarBar.SetActive(false); // Скрыть при выборе оружия

        if (waveTimer != null)
            waveTimer.SetActive(false); // Скрыть таймер волны

        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    void SelectWeapon(string weaponName)
    {
        playerStats.GiveBaseWeapon(weaponName);
        panel.SetActive(false);
        Time.timeScale = 1f;

        if (lumzvarBar != null)
            lumzvarBar.SetActive(true); // Показать обратно после выбора

        if (waveTimer != null)
            waveTimer.SetActive(true); // Показать таймер волны после выбора
    }

    void Start()
    {
        runeButton.onClick.AddListener(() => SelectWeapon("Rune"));
        katanaButton.onClick.AddListener(() => SelectWeapon("Katana"));
        maceButton.onClick.AddListener(() => SelectWeapon("Mace"));
    }
}
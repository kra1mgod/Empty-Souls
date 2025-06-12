using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject panel; // WeaponSelectPanel
    public StatsMenuUI statsMenu;
    public Button statsButton;
    public Button runeButton;
    public Button katanaButton;
    public Button maceButton;

    void Awake()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        if (statsMenu != null && statsButton != null)
            statsMenu.HideButton(statsButton.gameObject);
    }

    void SelectWeapon(string weaponName)
    {
        playerStats.GiveBaseWeapon(weaponName);
        panel.SetActive(false);
        Time.timeScale = 1f;

        if (statsMenu != null && statsButton != null)
            statsMenu.ShowButton(statsButton.gameObject);
    }

    void Start()
    {
        runeButton.onClick.AddListener(() => SelectWeapon("Rune"));
        katanaButton.onClick.AddListener(() => SelectWeapon("Katana"));
        maceButton.onClick.AddListener(() => SelectWeapon("Mace"));
    }
}
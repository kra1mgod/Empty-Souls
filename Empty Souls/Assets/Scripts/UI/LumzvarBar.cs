using UnityEngine;
using UnityEngine.UI;

public class LumzvarBar : MonoBehaviour
{
    public Slider lumzvarSlider;
    public Text lumzvarText;

    private PlayerStats playerStats;

    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("LumzvarBar: PlayerStats not found in scene!");
            enabled = false;
            return;
        }
        playerStats.OnLumzvarChanged -= UpdateBar;
        playerStats.OnLumzvarChanged += UpdateBar;
        Debug.Log("[LumzvarBar] Подписался на OnLumzvarChanged в Awake!");
    }

    void Start()
    {
        if (playerStats != null)
            UpdateBar(playerStats.currentLumzvarPoints, playerStats.lumzvarForNextEvolution);
    }

    public void UpdateBar(int currentLumzvar, int maxLumzvar)
    {
        Debug.Log($"[LumzvarBar] UpdateBar called: {currentLumzvar} / {maxLumzvar} | slider={lumzvarSlider} text={lumzvarText}");
        if (lumzvarSlider != null)
        {
            lumzvarSlider.maxValue = maxLumzvar;
            lumzvarSlider.value = currentLumzvar;
            Debug.Log($"[LumzvarBar] Slider value set to {currentLumzvar}, max {maxLumzvar}");
        }
        else
        {
            Debug.Log("[LumzvarBar] lumzvarSlider is NULL!");
        }
        if (lumzvarText != null)
        {
            lumzvarText.text = $"{currentLumzvar} / {maxLumzvar}";
        }
    }
}
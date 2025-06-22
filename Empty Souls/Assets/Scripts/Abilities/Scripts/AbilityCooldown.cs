using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldownUI : MonoBehaviour
{
    public Image abilityIcon;
    public TextMeshProUGUI cooldownText;
    public PlayerStats playerStats;

    void Update()
    {
        if (playerStats == null || playerStats.activeAbility == null)
        {
            abilityIcon.enabled = false;
            cooldownText.text = "";
            return;
        }

        abilityIcon.enabled = true;
        abilityIcon.sprite = playerStats.activeAbility.icon;

        // Получаем кулдаун и время последнего использования (требует реализации в BaseAbilitySO)
        float cd = playerStats.activeAbility.cooldown;
        float lastUse = playerStats.activeAbility.lastUseTime;
        float timeLeft = Mathf.Max(0f, lastUse + cd - Time.time);

        if (timeLeft > 0.05f)
        {
            cooldownText.text = timeLeft.ToString("F1");
            abilityIcon.color = new Color(1, 1, 1, 0.5f); // затемнённая иконка на КД
        }
        else
        {
            cooldownText.text = "";
            abilityIcon.color = Color.white; // полная яркость
        }
    }
}
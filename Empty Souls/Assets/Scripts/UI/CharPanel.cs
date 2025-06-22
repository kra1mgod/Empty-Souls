using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharPanelUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI levelText, expText, hpText, damageText, moveSpeedText, soulFragmentText, lumzvarText, evolutionText;

    void OnEnable()
    {
        Refresh();
    }

    void Update()
    {
        if (gameObject.activeSelf)
            Refresh();
    }

    public void Refresh()
    {
        if (playerStats == null) return;
        levelText.text = $"”ровень: {playerStats.level}";
        expText.text = $"ќпыта: {playerStats.experience}/{playerStats.expToNextLevel}";
        hpText.text = $"HP: {playerStats.currentHP} / {playerStats.maxHP}";
        damageText.text = $"”рон: {playerStats.baseDamage}";
        moveSpeedText.text = $"—корость: {playerStats.moveSpeed}";
        soulFragmentText.text = $"Soul Fragments: {playerStats.soulFragments}";
        lumzvarText.text = $"Lumzvar: {playerStats.currentLumzvarPoints}/{playerStats.lumzvarForNextEvolution}";
        evolutionText.text = $"Ёволюций: {playerStats.evolutionCount}";

       /* // ѕассивки/способности (пример)
        foreach (Transform child in passiveListContainer)
            Destroy(child.gameObject);
        foreach (var ability in playerStats.learnedAbilities)
        {
            if (ability.type == AbilityType.Passive)
            {
                var go = Instantiate(passiveItemPrefab, passiveListContainer);
                var txt = go.GetComponentInChildren<TextMeshProUGUI>();
                var img = go.GetComponentInChildren<Image>();
                if (txt) txt.text = ability.abilityName;
                if (img) img.sprite = ability.icon;
            }
        }*/
    }
}
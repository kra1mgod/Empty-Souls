using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public Slider hpSlider;
    public PlayerStats player;

    void Start()
    {
        player.onHPChanged += UpdateBar;
        UpdateBar(player.currentHP, player.maxHP);
    }

    void UpdateBar(int hp, int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = hp;
    }
}
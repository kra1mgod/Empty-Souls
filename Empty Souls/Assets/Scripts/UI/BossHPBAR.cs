using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Slider hpSlider;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowBar()
    {
        gameObject.SetActive(true);
    }
    public void HideBar()
    {
        gameObject.SetActive(false);
    }

    public void SetHP(int hp, int maxHp)
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }
    }
}
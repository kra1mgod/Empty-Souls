using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class EvolutionPanelUI : MonoBehaviour
{
    public GameObject panel;
    public Button[] optionButtons;
    public TMP_Text[] optionTitles;
    public TMP_Text[] optionDescriptions;
    public Image[] optionIcons;
    public Button skipButton;

    private Action<BaseEvolutionSO> onSelect;
    private List<BaseEvolutionSO> currentOptions;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Show(List<BaseEvolutionSO> options, Action<BaseEvolutionSO> onSelect)
    {
        //if (EvolutionLock.EvolutionChosen)
        //{
        //    panel.SetActive(false);
        //    return;
        //}

        panel.SetActive(true);
        this.onSelect = onSelect;
        currentOptions = options;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Count)
            {
                var opt = options[i];
                optionTitles[i].text = opt.evolutionName;
                optionDescriptions[i].text = opt.description;
                if (optionIcons != null && optionIcons.Length > i && opt.icon != null)
                {
                    optionIcons[i].sprite = opt.icon;
                    optionIcons[i].gameObject.SetActive(true);
                }
                else if (optionIcons != null && optionIcons.Length > i)
                {
                    optionIcons[i].gameObject.SetActive(false);
                }
                optionButtons[i].gameObject.SetActive(true);
                int idx = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => Choose(idx));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
                if (optionIcons != null && optionIcons.Length > i)
                    optionIcons[i].gameObject.SetActive(false);
            }
        }

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(Skip);
        }

        Time.timeScale = 0f;
    }

    void Choose(int idx)
    {
        //EvolutionLock.EvolutionChosen = true;
        HidePanel();
        onSelect?.Invoke(currentOptions[idx]);
    }

    void Skip()
    {
        //EvolutionLock.EvolutionChosen = true;
        HidePanel();
        onSelect?.Invoke(null);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
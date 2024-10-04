using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityButton : MonoBehaviour
{
    public string ID;

    public PlayerAbilities abilities;
    public BuyPanel panel;

    public bool isBought;
    public bool isFirstInList;

    public Text costText;
    public Text nameText;

    public Image image, connectionImage;

    public Color NotAvaibleButton;
    public Color AvaibleColor;
    public Color BoughtColor;

    public AbilityButton nextButton, prevButton;

    public AbilityEnum abilityType;
    [Range(0, 1000)]
    public int ExperienceNeeded;

    private void Awake()
    {
        ID = Guid.NewGuid().ToString();

        if (isFirstInList)
        {
            image.color = AvaibleColor;
        }
        else
        {
            image.color = NotAvaibleButton;
        }

        abilities = FindAnyObjectByType<PlayerAbilities>();
        panel = FindAnyObjectByType<BuyPanel>();

        if (connectionImage) connectionImage.color = NotAvaibleButton;
        costText.text = ExperienceNeeded.ToString() + " exp.";
    }

    public void OnButtonClick()
    {
        panel.SetAbility(this);
    }

    public void TriggerAvaibleColor()
    {
        image.color = AvaibleColor;
    }
}

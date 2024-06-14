using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Client;
using _root.Script.Ingame.Ability;
using _root.Script.Network;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private AbilityButton[] abilityButtons;

    private AbilityButton selected;
    
    private void Awake()
    {
        abilityButtons = GetComponentsInChildren<AbilityButton>();
    }

    private void Start()
    {
        SetAbilities(null, null, null);
    }

    public void SetAbilities(GameStudentCard card, Action<AbilityButton> onSelect, Action<Tiers> onClick)
    {
        selected = null;
        for (var i = 0; i < abilityButtons.Length; i++)
        {
            if (card?.tiers == null || card.tiers.Count <= i) abilityButtons[i].SetButton(null, null, null);
            else abilityButtons[i].SetButton(card.tiers[i], onSelect, onClick);
        }
    }

    public void SelectAbility(AbilityButton button)
    {
        if(selected) selected.SetSelect(false);
        selected = button;
        button.SetSelect(true);
    }
}

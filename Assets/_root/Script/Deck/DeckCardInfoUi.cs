using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardInfoUi : MonoBehaviour
{
    public void SetUi(DeckCard card)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }
        
        var cardNameUi = transform.GetChild(0).Find("CardName").GetComponent<TextMeshProUGUI>();
        cardNameUi.text = card.cardStats.cardName;

        var deckEditMenu = GetComponentInParent<DeckEditMenu>();
        var equipButton  = transform.GetChild(0).Find("EquipButton").GetComponent<Button>();
        var equipText    = equipButton.GetComponentInChildren<TextMeshProUGUI>();

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => { deckEditMenu.Equip(card); });
        equipText.text = card.isEquipped ? "Un Equip" : "Equip";
        
        gameObject.SetActive(true);
    }

    public void ResetUi()
    {
        gameObject.SetActive(false);
    }
}

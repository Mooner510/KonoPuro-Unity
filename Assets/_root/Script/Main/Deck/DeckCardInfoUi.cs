using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Deck;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardInfoUi : MonoBehaviour
{
    public void SetUi(PlayerCardResponse card, bool equipped)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }
        
        var cardNameUi = transform.GetChild(0).Find("CardName").GetComponent<TextMeshProUGUI>();
        cardNameUi.text = card.id;

        var deckEditMenu = GetComponentInParent<DeckEditMenu>();
        var equipButton  = transform.GetChild(0).Find("EquipButton").GetComponent<Button>();
        var equipText    = equipButton.GetComponentInChildren<TextMeshProUGUI>();

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => { deckEditMenu.Equip(card); });
        equipText.text = equipped ? "Un Equip" : "Equip";
        
        gameObject.SetActive(true);
    }

    public void ResetUi()
    {
        gameObject.SetActive(false);
    }
}

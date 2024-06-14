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
    private DeckEditMenu deckEditMenu;
    
    private TextMeshProUGUI cardName;

    private TextMeshProUGUI equipText;
    private Button          equipButton;

    private void Awake()
    {
        deckEditMenu = GetComponentInParent<DeckEditMenu>();
        cardName = transform.GetChild(0).Find("CardName").GetComponent<TextMeshProUGUI>();
        equipButton  = transform.GetChild(0).Find("EquipButton").GetComponent<Button>();
        equipText    = equipButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetUi(PlayerCardResponse card, bool equipped)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }
        
        cardName.text = card.cardType;

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

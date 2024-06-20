using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Data;
using _root.Script.Deck;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardInfoUi : MonoBehaviour
{
    private DeckEditMenu deckEditMenu;
    
    private TextMeshProUGUI nameT;

    private TextMeshProUGUI descriptionT;

    private TextMeshProUGUI equipText;

    private Button          equipButton;

    private void Awake()
    {
        deckEditMenu = GetComponentInParent<DeckEditMenu>();
        var tmps = GetComponentsInChildren<TextMeshProUGUI>();
        nameT = tmps[0];
        descriptionT = tmps[1];
        equipText = tmps[2];
        equipButton    = GetComponentInChildren<Button>();
    }

    public void SetUi(PlayerCardResponse card, bool equipped)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }

        if (card.type == CardType.Student)
        {
            var info = GameStatics.studentCardDictionary[card.cardType];
            nameT.text = info.name;
            var origin = $"티어 : {card.tier}\n전공 : {card.cardGroups.Select(x=> x.ToString()).Aggregate((s, s1) => $"{s}, {s1}")}\n{info.description}\n \n";
            descriptionT.text = card.passives.Select(passive => GameStatics.passiveDictionary[passive]).Aggregate(origin, (current, pInfo) => $"{current}{pInfo.name}\n{pInfo.description}\n \n");
        }
        else
        {
            var info = GameStatics.defaultCardDictionary[card.cardType];
            nameT.text        = info.name;
            descriptionT.text = $"사용 시간 : {info.time}\n티어 : {info.tier}  ||  유형 : {info.Type}\n \n{info.description}";
        }

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => { deckEditMenu.Equip(card); });
        equipText.text = equipped ? "해제하기" : "장착하기";

        gameObject.SetActive(true);
    }

    public void ResetUi()
    {
        gameObject.SetActive(false);
    }
}

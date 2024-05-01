using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Card;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class DeckCardUi : MonoBehaviour
{
    [HideInInspector]
    public bool isEquippedDeckUi;

    [FormerlySerializedAs("cardInfo")] public PlayerCardResponse card;

    public void SetUi(PlayerCardResponse card, bool equipped, bool selected)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }

        SetFocus(selected);
        
        gameObject.SetActive(true);

        this.card = card;
        // if (this.card.)
        //     GetComponent<Card>().frontSide.sprite = this.card.sprite;

        transform.Find("EquipMark").gameObject.SetActive(equipped && !isEquippedDeckUi);
    }

    public void ResetUi()
    {
        gameObject.SetActive(false);
        SetFocus(false);
    }

    public void SetFocus(bool focus)
    {
        transform.Find("SelectMark").gameObject.SetActive(focus);
    }
}
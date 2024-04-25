using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Card;
using UnityEngine;
using UnityEngine.Rendering;

public class DeckCardUi : MonoBehaviour
{
    [HideInInspector]
    public bool isEquippedDeckUi;

    public DeckCard cardInfo;

    public void SetUi(DeckCard card)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }

        gameObject.SetActive(true);

        cardInfo = card;
        if (cardInfo.sprite)
            GetComponent<Card>().frontSide.sprite = cardInfo.sprite;

        transform.Find("EquipMark").gameObject.SetActive(cardInfo.isEquipped && !isEquippedDeckUi);
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
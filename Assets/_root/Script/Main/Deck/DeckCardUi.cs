using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Card;
using _root.Script.Ingame;
using _root.Script.Manager;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class DeckCardUi : MonoBehaviour
{
    [HideInInspector]
    public bool isEquippedDeckUi;

    public  PlayerCardResponse cardData;
    
    private Card       cardFrame;
    private GameObject equipMark;
    private GameObject selectMark;

    private void Awake()
    {
        equipMark  = transform.Find("EquipMark").gameObject;
        selectMark = transform.Find("SelectMark").gameObject;
        cardFrame       = GetComponent<Card>();
    }

    public void SetUi(PlayerCardResponse card, bool equipped, bool selected)
    {
        if (card == null)
        {
            ResetUi();
            return;
        }

        SetSelect(selected);

        cardData                   = card;
        var sprite                            = ResourceManager.GetSprite(card.cardType);
        if(sprite) cardFrame.frontSide.sprite = sprite;
        equipMark.SetActive(equipped && !isEquippedDeckUi);
    
        gameObject.SetActive(true);
    }

    public void ResetUi()
    {
        gameObject.SetActive(false);
        SetSelect(false);
    }

    public void SetSelect(bool focus)
    {
        selectMark.SetActive(focus);
    }
}
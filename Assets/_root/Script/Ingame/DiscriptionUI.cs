using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Card;
using _root.Script.Ingame;
using _root.Script.Manager;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DiscriptionUI : MonoBehaviour
{
    private GameObject CardName;
    private TMP_Text CardNameText;
    private GameObject CardDiscription;
    private TMP_Text CardDiscriptionText;
    private GameObject CardImg;
    private Image CardImg_Image;
    private RectTransform Rect;
    [SerializeField] private float FadeTime;

    private void Awake()
    {
        Rect = gameObject.GetComponent<RectTransform>();
        CardName = transform.GetChild(0).gameObject;
        CardDiscription = transform.GetChild(1).gameObject;
        CardImg = transform.GetChild(2).gameObject;
        CardNameText = CardName.GetComponent<TMP_Text>();
        CardDiscriptionText = CardDiscription.GetComponent<TMP_Text>();
        CardImg_Image = CardImg.GetComponent<Image>();
    }

    public void viewCard(IngameCard card)
    {
        if (!card)
        {
            Out();
            return;
        }
        if(card.type == IngameCardType.Hand)
        {
            gameObject.SetActive(true);
            Vector3 CardPos = card.transform.position;
            GetStudent(card);
            Rect.position = new Vector3(CardPos.x, 
                CardPos.y + 1f, CardPos.z + (CardPos.z > 0 ? -2f : 2f));
        }
        else if (card.type == IngameCardType.Field)
        {
            gameObject.SetActive(true);
            Vector3 CardPos = card.transform.position;
            GetStudent(card);
            Rect.position = new Vector3(CardPos.x,
                CardPos.y+1f, CardPos.z+(CardPos.z > 0 ? -2f : 2f));
        }
        else if (card.type == IngameCardType.Student)
        {
            gameObject.SetActive(true);
            Vector3 CardPos = card.transform.position;
            GetStudent(card);
            Rect.position = new Vector3(CardPos.x,
                CardPos.y+1f, CardPos.z + (CardPos.z > 0 ? -2f : 2f));
        }
        else if (card.type == IngameCardType.Deck)
        {
            Out();
            return;
        }
        
        if (gameObject)
        {
            float x = Rect.position.x;
            float y = Rect.position.y;
            float z = Rect.position.z;
            Rect.position =
                new Vector3(Mathf.Clamp(x, -5f, 4f), y, Mathf.Clamp(z, -8f, 8f));
        }
    }

    private void GetStudent(IngameCard card)
    {
        CardNameText.text = card.type.ToString();
        CardDiscriptionText.text = card.type.ToString();
        CardImg_Image.sprite = card.gameObject.GetComponent<Card>().frontSide.sprite;
    }

    public void Out()
    {
        gameObject.SetActive(false);
        CardImg_Image.sprite = null;
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Ingame
{
    public class DescriptionUI : MonoBehaviour
    {
        [SerializeField] private float FadeTime;
        private GameObject CardDiscription;
        private TMP_Text CardDiscriptionText;
        private GameObject CardImg;
        private Image CardImg_Image;
        private GameObject CardName;
        private TMP_Text CardNameText;
        private RectTransform Rect;

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

        public void ViewCard(IngameCard card)
        {
            if (!card)
            {
                Out();
                return;
            }

            switch (card.type)
            {
                case IngameCardType.Hand:
                {
                    gameObject.SetActive(true);
                    var CardPos = card.transform.position;
                    GetStudent(card);
                    Rect.position = new Vector3(CardPos.x,
                        CardPos.y + 1f, CardPos.z + (CardPos.z > 0 ? -2f : 2f));
                    break;
                }
                case IngameCardType.Field:
                {
                    gameObject.SetActive(true);
                    var CardPos = card.transform.position;
                    GetStudent(card);
                    Rect.position = new Vector3(CardPos.x,
                        CardPos.y + 1f, CardPos.z + (CardPos.z > 0 ? -2f : 2f));
                    break;
                }
                case IngameCardType.Student:
                {
                    gameObject.SetActive(true);
                    var CardPos = card.transform.position;
                    GetStudent(card);
                    Rect.position = new Vector3(CardPos.x,
                        CardPos.y + 1f, CardPos.z + (CardPos.z > 0 ? -2f : 2f));
                    break;
                }
                case IngameCardType.Deck:
                    Out();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!gameObject) return;
            var x = Rect.position.x;
            var y = Rect.position.y;
            var z = Rect.position.z;
            Rect.position =
                new Vector3(Mathf.Clamp(x, -5f, 4f), y, Mathf.Clamp(z, -8f, 8f));
        }

        private void GetStudent(IngameCard card)
        {
            CardNameText.text = card.type.ToString();
            CardDiscriptionText.text = card.type.ToString();
            CardImg_Image.sprite = card.gameObject.GetComponent<Card.Card>().frontSide.sprite;
        }

        public void Out()
        {
            gameObject.SetActive(false);
            CardImg_Image.sprite = null;
        }
    }
}
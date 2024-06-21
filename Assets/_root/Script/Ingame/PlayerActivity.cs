using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Data;
using _root.Script.Ingame;
using _root.Script.Network;
using UnityEngine;

public class PlayerActivity : MonoBehaviour
{
	public static string usingcard;
	public static bool isMyActive;
	private IngameUi ingameUi;

	private PlayerHand selfHand;
	private PlayerHand otherHand;
	private Camera     mainCamera;

	public IngameCard selectedCard;
	[SerializeField] private GameObject CardInfoPanel;
	private Animator CardAnim;
	private DiscriptionUI cardui;
	
	private bool interactable;

	public List<GameCard> GetHandCards(bool self) => self ? selfHand.HandCards : otherHand.HandCards;

	private void Awake()
	{
		ingameUi = FindObjectOfType<IngameUi>();

		var hands = FindObjectsOfType<PlayerHand>().ToList();
		mainCamera = Camera.main;
		selfHand   = hands.First(x => x.gameObject.name == "Self Hand");
		otherHand = hands.First(x => x.gameObject.name == "Other Hand");
		CardInfoPanel           = GameObject.Find("Card Info Panel");
		cardui = CardInfoPanel.GetComponent<DiscriptionUI>();
		CardAnim = CardInfoPanel.GetComponent<Animator>();
	}

	private void Start()
	{
		SetActive(false);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{	
			//CardInfoUI Interact
			Debug.Log("um");
			cardui.Out();
			viewCard(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var viewcard)
				? viewcard.transform.GetComponent<IngameCard>()
				: null);
		}
	}

	public void SetActive(bool active)
	{
		if (!active)
		{
			ingameUi.SetCardInfo(null);
			SelectCard(null);
		}

		selectedCard = null;
		selfHand.SetActive(active);
		otherHand.SetActive(active);
		interactable = active;
	}

	private void viewCard(IngameCard card)//CardInfoUI
	{
		CardInfoPanel.SetActive(true);
		if (!card)
		{
			CardInfoPanel.SetActive(false);
		}
		else
		{
			if(card.type == IngameCardType.Hand)
			{
				if (card.isMine)
				{
					CardInfoPanel.SetActive(true);
					CardAnim.Play("CardInfoFadeIn");
					cardui.viewCard(card);
				}
				else
				{
					CardInfoPanel.SetActive(false);
				}
			}
			else if (card.type == IngameCardType.Field)
			{
				CardInfoPanel.SetActive(true);
				CardAnim.Play("CardInfoFadeIn");
				cardui.viewCard(card);
			}
			else if (card.type == IngameCardType.Student)
			{
				CardInfoPanel.SetActive(true);
				CardAnim.Play("CardInfoFadeIn");
				cardui.viewCard(card);
			}
			else
			{
				CardInfoPanel.SetActive(false);
			}
		}
	}

	public IngameCard SelectCard(IngameCard card)
	{
		if (!interactable) return null;
		cardui.Out();
		if (card && card == selectedCard && card.type == IngameCardType.Hand && card.isMine)
			return card;

		selectedCard = card;
		ingameUi.SetHover(!selectedCard);
		ingameUi.SetInteract(!selectedCard && GameStatics.isTurn);
		if (!card) selfHand.SelectCard(null);
		else if (card.type != IngameCardType.Hand) selfHand.SelectCard(null, false);
		else if (card.isMine && card.type == IngameCardType.Hand)
		{

			selfHand.SelectCard(card);

		}
		ingameUi.SetCardInfo(card);
		return null;
	}

	public void AddHandCard(IngameCard card, bool isMine)
	{
		var hand = isMine ? selfHand : otherHand;
		hand.AddCard(card);
	}

	public IngameCard RemoveHandCard(IngameCard card, bool isMine)
	{
		var hand = isMine ? selfHand : otherHand;
		return hand.RemoveHandCard(card);
	}

	public IngameCard RemoveHandCard(string id, bool isMine)
	{
		var hand = isMine ? selfHand : otherHand;
		return hand.RemoveHandCard(id);
	}

	public void UseAbility()
	{
		if (!GameStatics.isTurn) return;
	}

	public void Sleep()
	{
		// NetworkClient.Send(RawData.of(105, null));
	}
}
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
	private IngameUi ingameUi;

	private PlayerHand selfHand;
	private PlayerHand otherHand;
	private Camera     mainCamera;

	public IngameCard selectedCard;

	private void Awake()
	{
		ingameUi = FindObjectOfType<IngameUi>();

		var hands = FindObjectsOfType<PlayerHand>().ToList();
		mainCamera = Camera.main;
		selfHand   = hands.First(x => x.gameObject.name == "Self Hand");
		otherHand  = hands.First(x => x.gameObject.name == "Other Hand");

		//TODO: 빌드에는 포함시키기 - 화면 밖으로 커서가 안나가도록 해줌
		// Cursor.lockState = CursorLockMode.Confined;
	}

	private void Start()
	{
		SetActive(false);
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0)) return;
		SelectCard(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit)
				           ? hit.transform.GetComponent<IngameCard>()
				           : null);
	}

	public void SetActive(bool active)
	{
		selectedCard = null;
		selfHand.SetActive(active);
		otherHand.SetActive(active);
		enabled = active;
	}

	private void SelectCard(IngameCard card)
	{
		if (card && card == selectedCard && card.type == IngameCardType.Hand && card.isMine)
		{
			UseCard(selectedCard);
			return;
		}

		selectedCard = card;
		if (!card || card.type == IngameCardType.Deck) selfHand.SelectCard(null);
		else if (card.isMine && card.type == IngameCardType.Hand) selfHand.SelectCard(card);
		ingameUi.SetCardInfo(card);
	}

	public void AddHandCard(IngameCard card, bool isMine)
	{
		var hand = isMine ? selfHand : otherHand;
		hand.AddCard(card);
	}

	public void RemoveHandCard(IngameCard card, bool isMine)
	{
		var hand = isMine ? selfHand : otherHand;
		hand.RemoveHandCard(card);
	}

	public void UseCard(IngameCard card)
	{
		if(!GameStatics.isTurn) return;
		selectedCard = null;
		ingameUi.SetCardInfo(null);
		RemoveHandCard(card, true);

		NetworkClient.Send(RawProtocol.of(103, card.type == IngameCardType.Student ? card.GetStudentData().id : card.GetCardData().id));	
	}

	public void UseAbility()
	{
		if(!GameStatics.isTurn) return;
	}

	public void Sleep()
	{
		// NetworkClient.Send(RawData.of(105, null));
	}
}
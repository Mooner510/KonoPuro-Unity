using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Ingame;
using _root.Script.Network;
using UnityEngine;

public class PlayerActivity : MonoBehaviour
{
	private PlayerHand selfHand;
	private PlayerHand otherHand;
	private Camera     mainCamera;

	public PlayerCardResponse selectedCard;

	private void Awake()
	{
		var hands = FindObjectsOfType<PlayerHand>().ToList();
		selfHand             = hands.First(x=>x.gameObject.name == "Self Hand");
		otherHand             = hands.First(x=>x.gameObject.name == "Other Hand");
		//TODO: 빌드에는 포함시키기
		// Cursor.lockState = CursorLockMode.Confined;
	}

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0)) return;
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
		SelectCard(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit)
				           ? hit.transform.GetComponent<IngameCard>()
				           : null);
	}

	private void SelectCard(IngameCard card)
	{
		if (!card)
		{
			selfHand.SelectCard(card);
		}
		else if(card.type == IngameCardType.Hand)
		{
			selfHand.SelectCard(card);
		}
	}	

	public void UseCard(string cardId)
	{
		NetworkClient.Send(RawData.of(103, cardId));	
	}

	public void UseAbility()
	{
	}

	public void Sleep()
	{
		NetworkClient.Send(RawData.of(105, null));
	}
}

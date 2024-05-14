using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Client;
using _root.Script.Ingame;
using _root.Script.Network;
using UnityEngine;

public class PlayerActivity : MonoBehaviour
{
	private PlayerHeld held;
	private Camera     mainCamera;

	public PlayerCardResponse selectedCard;

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void Awake()
	{
		held = FindObjectOfType<PlayerHeld>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
			{
				SelectCard(hit.transform.GetComponent<IngameCard>());
			}
		}
	}
	
	private void SelectCard(IngameCard card)
	{
		if (!card)
		{
			
		}
		if(card.type == IngameCardType.Held)
		{
			held.SelectCard(card.card);
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

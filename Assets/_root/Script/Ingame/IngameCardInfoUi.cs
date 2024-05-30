using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Ingame;
using _root.Script.Manager;
using _root.Script.Network;
using TMPro;
using UnityEngine;

public class IngameCardInfoUi : MonoBehaviour
{
	private TextMeshProUGUI nameT;

	private void Awake()
	{
		var tmps = GetComponentsInChildren<TextMeshProUGUI>();
		nameT = tmps[0];
	}

	private void Start()
	{
		SetActive(false);
	}

	private void Init()
	{
		nameT.text = "Name";
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	public void SetCard(IngameCard card)
	{
		if (card == null || card.type == IngameCardType.Deck ||
		    (card.type is not (IngameCardType.Field or IngameCardType.Student) && !card.isMine))
		{
			SetActive(false);
			return;
		}

		Init();
		SetActive(true);

		var data = card.cardData;
		if (data == null) return;
		nameT.text = data.cardType;
	}
}
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

	public void SetInfo(IngameCard card)
	{
		if (card == null || card.type == IngameCardType.Deck ||
		    (card.type is not (IngameCardType.Field or IngameCardType.Student) && !card.isMine))
		{
			SetActive(false);
			return;
		}

		SetActive(true);

		if (card.type == IngameCardType.Student)
		{
			var studentData = card.GetStudentData();
			if (studentData != null)
			{
				Debug.Log(studentData.cardType);
				nameT.text = studentData.cardType;
			}
			else
			{
				var defaultData = card.GetData();
				Debug.Log(defaultData);
				nameT.text = defaultData.cardType;
			}
		}
		else
		{
			var data = card.GetCardData();
			if (data == null) return;
			Debug.Log(data.defaultCardType);
			nameT.text = data.defaultCardType;
		}
	}
	
	public void SetInfo(Tiers ability)
	{
		SetActive(true);

		nameT.text = ability.ToString();
	}
}
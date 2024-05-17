using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Ingame;
using _root.Script.Network;
using UnityEngine;

public class FieldSetter : MonoBehaviour
{
	[SerializeField] private GameObject cardPrefab;
	
	[SerializeField]
	private List<IngameCard> fieldCards = new();

	private List<Transform> trs = new List<Transform>();

	[SerializeField] private float cardSize = 1;

	[SerializeField]
	private bool isMine = true;
	
	private void Awake()
	{
		var transforms = GetComponentsInChildren<Transform>().ToList();
		trs = transforms;
		trs.Remove(transform);

		
		//TODO: 실험용 삭제 필요
		for (int i = 0; i < 5; i++)
		{
			var card = IngameCard.CreateIngameCard(null);
			card.type = IngameCardType.Student;
			card.Init(isMine, IngameCardType.Student);
			fieldCards.Add(card);
		}
		for (int i = 0; i < 3; i++)
		{
			var card = IngameCard.CreateIngameCard(null);
			card.type = IngameCardType.Field;
			card.Init(isMine, IngameCardType.Field);
			fieldCards.Add(card);
		}
	}

	private void Start()
	{
		UpdateCards();
	}

	public void AddNewCard(IngameCard addition)
	{
		fieldCards.Add(addition);
		UpdateCards();
	} 

	public void AddNewCards(IEnumerable<IngameCard> addition)
	{
		fieldCards.AddRange(addition);
		UpdateCards();
	}

	public void UpdateCards()
	{
		var students = fieldCards.Where(c => c.type == IngameCardType.Student).ToList();
		var skills1  = fieldCards.Except(students).ToList();
		var skills2  = skills1.GetRange(0, skills1.Count / 2);
		skills1 = skills1.Except(skills2).ToList();

		SetPos(trs[0], students);
		SetPos(trs[1], skills1);
		SetPos(trs[2], skills2);
	}

	public void SetPos(Transform field, List<IngameCard> cards)
	{
		var     count      = cards.Count;
		float   multiplyZ  = field.localScale.y / count;
		Vector3 defaultPos = field.position;
		defaultPos.z -= (count - 1) * multiplyZ * 0.5f;
		for (int i = 0; i < count; i++)
		{
			var appliedPos = defaultPos;
			appliedPos.z                  += i * multiplyZ;
			cards[i].transform.localScale =  Vector3.one * cardSize;
			cards[i].transform.position   =  appliedPos;
		}
	}
}

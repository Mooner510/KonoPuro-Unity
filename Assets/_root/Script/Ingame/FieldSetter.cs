using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Ingame;
using _root.Script.Network;
using UnityEngine;

public class FieldSetter : MonoBehaviour
{
	[SerializeField] private GameObject cardPrefab;

	[SerializeField] private List<IngameCard> fieldCards = new();

	private List<Transform> trs = new();

	[SerializeField] private float cardSize = 1;

	public bool isMine = true;

	private void Awake()
	{
		var transforms = GetComponentsInChildren<Transform>().ToList();
		trs = transforms;
		trs.Remove(transform);
	}

	private void Start()
	{
		StudentUpdate();
		UpdateCards();
	}

	public void AddNewCard(IngameCard addition)
	{
		fieldCards.Add(addition);
		if (addition.type == IngameCardType.Student) StudentUpdate();
		else UpdateCards();
	}

	public void AddNewCards(IEnumerable<IngameCard> addition)
	{
		var ingameCards = addition as IngameCard[] ?? addition.ToArray();
		fieldCards.AddRange(ingameCards);
		if (ingameCards.Any(x => x.type == IngameCardType.Student)) StudentUpdate();
		if (ingameCards.Any(x => x.type != IngameCardType.Student)) UpdateCards();
	}

	public void AddNewCard(GameStudentCard addition) =>
			AddNewCard(IngameCard.CreateIngameCard(addition, transform.position + new Vector3(0, 1f),
			                                       Quaternion.Euler(-90, 0, 90)));

	public void AddNewCards(IEnumerable<GameStudentCard> addition) =>
			AddNewCards(addition.Select(x => IngameCard.CreateIngameCard(x, transform.position + new Vector3(0, 1f),
			                                                             Quaternion.Euler(-90, 0, 90))));

	public void AddNewCard(GameCard addition) =>
			AddNewCard(IngameCard.CreateIngameCard(addition, transform.position + new Vector3(0, 1f),
			                                       Quaternion.Euler(-90, 0, 90)));

	public void AddNewCards(IEnumerable<GameCard> addition) =>
			AddNewCards(addition.Select(x => IngameCard.CreateIngameCard(x, transform.position + new Vector3(0, 1f),
			                                                             Quaternion.Euler(-90, 0, 90))));
	
	

	private void StudentUpdate()
	{
		var students = fieldCards.Where(c => c.type == IngameCardType.Student).ToList();
		SetPos(trs[0], students);
	}

	private void UpdateCards()
	{
		var students = fieldCards.Where(c => c.type == IngameCardType.Student).ToList();
		var skills1  = fieldCards.Except(students).ToList();
		var skills2  = skills1.GetRange(0, skills1.Count / 2);
		skills1 = skills1.Except(skills2).ToList();

		SetPos(trs[1], skills1);
		SetPos(trs[2], skills2);
	}

	public void SetPos(Transform field, List<IngameCard> cards)
	{
		var     count      = cards.Count;
		float   multiplyZ  = field.localScale.y / count;
		Vector3 defaultPos = field.position;
		defaultPos.z -= (count - 1) * multiplyZ * 0.5f;
		defaultPos.y += .1f;
		for (int i = 0; i < count; i++)
		{
			var appliedPos = defaultPos;
			appliedPos.z                  += i * multiplyZ;
			cards[i].transform.localScale =  Vector3.one * cardSize;
			cards[i].transform.position   =  appliedPos;
		}
	}
}
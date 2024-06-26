using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using UnityEngine;
using UnityEngine.UIElements;

namespace _root.Script.Ingame
{
public class FieldSetter : MonoBehaviour
{
	[SerializeField] private GameObject cardPrefab;

	[SerializeField] private List<IngameCard> fieldCards   = new();
	[SerializeField] private List<IngameCard> studentField = new();

	public bool isMine = true;

	private List<Transform> trs = new();

	public List<IngameCard> GetStudentCards() => studentField;

	private void Awake()
	{
		var transforms = GetComponentsInChildren<Transform>().ToList();
		trs = transforms;
		trs.Remove(transform);
	}

	private void Start()
	{
		UpdateStudentPos();
		UpdateFieldCardPos();
	}

	public void UpdateField(List<GameCard> cards)
	{
		var updated = new List<IngameCard>();

		var updatedCount = cards.Count;
		var fieldCount   = fieldCards.Count;

		if (updatedCount <= fieldCount)
		{
			for (var i = 0; i < fieldCount; i++)
			{
				var card = fieldCards[i];
				if (i < updatedCount)
				{
					var index = i;
					card.Show(false, callback: () => card.LoadDisplay(cards[index]));
					updated.Add(card);
				}
				else card.Show(false, true);
			}
		}
		else
		{
			for (var i = 0; i < updatedCount; i++)
			{
				if (i < fieldCount)
				{
					var card  = fieldCards[i];
					var index = i;
					card.Show(false, callback: () => card.LoadDisplay(cards[index]));
					updated.Add(card);
				}
				else
				{
					var ingameCard = IngameCard.CreateIngameCard(cards[i]);
					ingameCard.isMine             = isMine;
					ingameCard.type               = IngameCardType.Field;
					ingameCard.transform.rotation = Quaternion.Euler(-90, 0, 90);
					updated.Add(ingameCard);
				}
			}
		}

		fieldCards = updated;

		StartCoroutine(UpdateFlow());
	}

	private IEnumerator UpdateFlow()
	{
		yield return new WaitForSeconds(.5f);
		UpdateFieldCardPos();
		foreach (var ingameCard in fieldCards)
		{
			ingameCard.Show(true);
		}
	}

	public void AddNewCard(IngameCard addition)
	{
		addition.isMine = isMine;
		if (addition.type == IngameCardType.Student)
		{
			studentField.Add(addition);
			UpdateStudentPos();
		}
		else
		{
			fieldCards.Add(addition);
			addition.type = IngameCardType.Field;
			UpdateFieldCardPos();
		}
	}

	public void AddNewCards(IEnumerable<IngameCard> addition)
	{
		var ingameCards = addition as IngameCard[] ?? addition.ToArray();
		var students    = ingameCards.Where(x => x.type == IngameCardType.Student).ToList();
		var fields      = ingameCards.Except(students).ToList();
		studentField.AddRange(students);
		fieldCards.AddRange(fields);
		foreach (var ingameCard in fields) ingameCard.type = IngameCardType.Field;

		if (students.Count > 1) UpdateStudentPos();
		if (fields.Count > 1) UpdateFieldCardPos();
	}

	public void AddNewCard(GameStudentCard addition)
	{
		AddNewCard(IngameCard.CreateIngameCard(addition, transform.position + new Vector3(0, 1f),
		                                       Quaternion.Euler(-90, 0, 90)));
	}

	public void AddNewCards(IEnumerable<GameStudentCard> addition)
	{
		AddNewCards(addition.Select(x => IngameCard.CreateIngameCard(x, transform.position + new Vector3(0, 1f),
		                                                             Quaternion.Euler(-90, 0, 90))));
	}

	private void UpdateStudentPos()
	{
		StudentSetPos(trs[0], studentField);
	}

	public void AddNewCard(GameCard addition)
	{
		AddNewCard(IngameCard.CreateIngameCard(addition, transform.position + new Vector3(0, 1f),
		                                       Quaternion.Euler(-90, 0, 90)));
	}

	public void AddNewCards(IEnumerable<GameCard> addition)
	{
		AddNewCards(addition.Select(x => IngameCard.CreateIngameCard(x, transform.position + new Vector3(0, 1f),
		                                                             Quaternion.Euler(-90, 0, 90))));
	}

	private void UpdateFieldCardPos()
	{
		var skills1 = fieldCards.ToList();
		var skills2 = skills1.GetRange(0, skills1.Count / 2);
		skills1 = skills1.Except(skills2).ToList();

		SetPos(trs[1], skills1);
		SetPos(trs[2], skills2);
	}

	private void StudentSetPos(Transform field, IReadOnlyList<IngameCard> cards)
	{
		const int cardSize = 1;

		var count      = cards.Count;
		var multiplyZ  = field.localScale.y / count;
		var defaultPos = field.position;
		defaultPos.z -= (count - 1) * multiplyZ * 0.5f;
		defaultPos.y += .1f;
		for (var i = 0; i < count; i++)
		{
			var appliedPos = defaultPos;
			appliedPos.z                  += i * multiplyZ;
			cards[i].transform.localScale =  Vector3.one * cardSize;
			cards[i].transform.position   =  appliedPos;
		}
	}

	private int currentScale = 1;

	private void SetPos(Transform field, IReadOnlyList<IngameCard> cards)
	{
		var cardSize  = new Vector2(2.5f, 3.5f);
		var fieldSize = field.localScale + Vector3.one * .5f;

		var count = cards.Count;
		var ratio = fieldSize.y / cardSize.x / (fieldSize.x / cardSize.y);

		var preRow = Mathf.Sqrt(count / ratio);
		var preCol = ratio * preRow;
		var (row, col) = GetInt(preRow, preCol, count);
		
		var size = fieldSize.x / (row * cardSize.y);

		var multiplyZ  = fieldSize.x / col;
		var multiplyX  = fieldSize.y / row;
		var defaultPos = field.position;
		defaultPos.z -= (col - 1) * multiplyZ * 0.5f;
		defaultPos.x -= (row - 1) * multiplyX * 0.5f;
		defaultPos.y += .1f;
		for (var i = 0; i < count; i++)
		{
			cards[i].transform.localScale = Vector3.one * size;
			var appliedPos = defaultPos;
			appliedPos.z                += i % col * multiplyZ;
			appliedPos.x                += i / col * multiplyX;
			cards[i].transform.position =  appliedPos;
		}

		Debug.LogError("New");
		Debug.LogWarning(count);
		Debug.LogWarning(row);
		Debug.LogWarning(col);
	}

	public (int, int) GetInt(float a, float b, int min)
	{
		var floorA = Mathf.FloorToInt(a);
		var floorB = Mathf.FloorToInt(b);
		var ceilA  = Mathf.CeilToInt(a);
		var ceilB  = Mathf.CeilToInt(b);

		if (floorA * floorB >= min) return (floorA, floorB);
		else if (ceilA * floorB >= min) return (ceilA, floorB);
		else if (floorA * ceilB >= min) return (floorA, ceilB);
		else return (ceilA, ceilB);
	}
}
}
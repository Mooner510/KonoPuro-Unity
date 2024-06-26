using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Ingame;
using _root.Script.Network;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSetter : MonoBehaviour
{
	[SerializeField] private GameObject cardPrefab;

	[SerializeField] private List<IngameCard> fieldCards   = new();
	[SerializeField] private List<IngameCard> studentField = new();

	private List<Transform> trs = new();

	[SerializeField] private float cardSize = 1;

	public bool isMine = true;

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
		var fieldIds   = fieldCards.Select(x => x.GetCardData().id).ToList();
		var updatedIds = cards.Select(x => x.id).ToList();
		var placedIds  = fieldIds.Intersect(updatedIds).ToList();
		var addedIds   = updatedIds.Except(placedIds).ToList();

		var updatedField = fieldCards.Where(x => updatedIds.Contains(x.GetCardData().id)).ToList();
		var removeField  = fieldCards.Except(updatedField).ToList();

		var              addedCards       = cards.Where(x => addedIds.Contains(x.id)).ToList();
		foreach (var addedCard in addedCards)
		{
			var ingameCard = IngameCard.CreateIngameCard(addedCard);
			ingameCard.isMine             = isMine;
			ingameCard.type               = IngameCardType.Field;
			ingameCard.transform.rotation = Quaternion.Euler(-90, 0, 90);
			updatedField.Add(ingameCard);
		}

		fieldCards = updatedField;

		foreach (var ingameCard in removeField) ingameCard.Show(false, true);
		foreach (var ingameCard in fieldCards) ingameCard.Show(false);

		StartCoroutine(UpdateFlow());
	}

	private IEnumerator UpdateFlow()
	{
		yield return new WaitForSeconds(.7f);
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

		if (students.Count > 1) UpdateStudentPos();
		if (fields.Count > 1) UpdateFieldCardPos();
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

	private void UpdateStudentPos()
	{
		SetPos(trs[0], studentField);
	}

	private void UpdateFieldCardPos()
	{
		var students = fieldCards.Where(c => c.type == IngameCardType.Student).ToList();
		var skills1  = fieldCards.Except(students).ToList();
		var skills2  = skills1.GetRange(0, skills1.Count / 2);
		skills1 = skills1.Except(skills2).ToList();

		SetPos(trs[1], skills1);
		SetPos(trs[2], skills2);
	}

	private void SetPos(Transform field, IReadOnlyList<IngameCard> cards)
	{
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
}
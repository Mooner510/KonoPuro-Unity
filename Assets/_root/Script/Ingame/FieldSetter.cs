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

	public List<IngameCard> GetStudentCards() => fieldCards.Where(x => x.type == IngameCardType.Student).ToList();

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
		var fieldIds   = fieldCards.Select(x => x.GetData().id);
		var updatedIds = cards.Select(x => x.id);
		var placedIds     = fieldIds.Intersect(updatedIds);
		var addedIds      = updatedIds.Except(placedIds);
		var removedIds    = fieldIds.Except(placedIds).Except(addedIds);

		var updatedField = fieldCards.Where(x => updatedIds.Contains(x.GetData().id)).ToList();

		var              addedCards       = cards.Where(x => addedIds.Contains(x.id));
		List<IngameCard> addedIngameCards = new List<IngameCard>();
		foreach (var addedCard in addedCards)
		{
			var ingameCard = IngameCard.CreateIngameCard(addedCard);
			ingameCard.isMine             = isMine;
			ingameCard.type               = IngameCardType.Field;
			ingameCard.transform.rotation = Quaternion.Euler(-90, 0, 90);
			addedIngameCards.Add(ingameCard);
		}
		
		updatedField.AddRange(addedIngameCards);

		StartCoroutine(UpdateFlow(fieldCards.Where(x => removedIds.Contains(x.GetData().id)).ToList(), updatedField));
	}

	private IEnumerator UpdateFlow(List<IngameCard> removed, List<IngameCard> field)
	{
		foreach (var ingameCard in removed)
		{
			ingameCard.Show(false, true);
		}

		yield return new WaitForSeconds(0.7f);
		
		UpdateFieldCardPos();
		foreach (var ingameCard in field)
		{
			ingameCard.Show(true);
		}
	}

	public void AddNewCard(IngameCard addition)
	{
		fieldCards.Add(addition);
		addition.isMine = isMine;
		if (addition.type == IngameCardType.Student) UpdateStudentPos();
		else
		{
			addition.type = IngameCardType.Field;
			UpdateFieldCardPos();
		}
	}

	public void AddNewCards(IEnumerable<IngameCard> addition)
	{
		var ingameCards = addition as IngameCard[] ?? addition.ToArray();
		fieldCards.AddRange(ingameCards);
		if (ingameCards.Any(x => x.type == IngameCardType.Student)) UpdateStudentPos();
		if (ingameCards.Any(x => x.type != IngameCardType.Student)) UpdateFieldCardPos();
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
		var students = fieldCards.Where(c => c.type == IngameCardType.Student).ToList();
		SetPos(trs[0], students);
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
		var     count      = cards.Count;
		var   multiplyZ  = field.localScale.y / count;
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
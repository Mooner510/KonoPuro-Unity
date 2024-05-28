using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Network;
using Unity.VisualScripting;
using UnityEngine;

namespace _root.Script.Ingame
{
public class PlayerHand : MonoBehaviour
{
	private                  IngameUi ui;
	[SerializeField] private GameObject heldCardPrefab;

	private readonly         List<IngameCard> handCards  = new();
	private                  List<Transform>  transforms = new();

	[SerializeField] private float            cardSize      = .7f;
	[SerializeField] private float            cardMoveSpeed = 2;

	[SerializeField] private float      closedXOffset = .1f;
	[SerializeField] private float      openedXOffset = .2f;

	[SerializeField] private float      maxZWidth;
	[SerializeField] private float      maxSubZWidth;

	[SerializeField] private Vector3    selectedPos = new Vector3(-1, 10, 0);

	private                  IngameCard selectedCard;

	private Coroutine cardMoveCoroutine;
	private bool      heldUpdating;
	private bool      heldOpened;

	public bool isMine;

	private Camera cam;

	//TODO: 실험용 - 삭제 필요
	public int testCount;

	private void Awake()
	{
		ui       = FindObjectOfType<IngameUi>();
		cam        = Camera.main;
		transforms = GetComponentsInChildren<Transform>().ToList();
		transforms.Remove(transform);
	}

	private void Start()
	{
		selectedCard = null;
		heldOpened   = false;
		HandUpdate(false);

		//TODO: 실험용 - 삭제 필요
		for (int i = 0; i < testCount; i++)
		{
			var card = IngameCard.CreateIngameCard(null);
			card.transform.localScale = Vector3.one * cardSize;
			card.Init(isMine, IngameCardType.Hand);
			AddCards(new List<IngameCard>()
			         { card });
		}
	}

	private void Update()
	{
		if (isMine) HandShowCheck();
	}

	public void SelectCard(IngameCard card)
	{
		selectedCard = card;
		HandUpdate(card || heldOpened);
	}

	private void HandShowCheck()
	{
		if (selectedCard) return;
		var tr         = transforms[(heldOpened ? 1 : 0)];
		var position   = tr.position;
		var localScale = tr.localScale;
		var mousePos   = Input.mousePosition;
		mousePos.z = Mathf.Abs(position.y - cam.transform.position.y);
		mousePos   = cam.ScreenToWorldPoint(mousePos);
		var inner = (mousePos.x >= position.x - (localScale.x * 0.5f)) &&
		            (mousePos.x <= position.x + (localScale.x * 0.5f)) &&
		            (mousePos.z >= position.z - (localScale.y * 0.5f)) &&
		            (mousePos.z <= position.z + (localScale.y * 0.5f));
		if (heldOpened ? !inner : inner) HandUpdate(!heldOpened);
	}

	public void AddCards(List<IngameCard> cards)
	{
		foreach (var card in cards)
			handCards.Add(card);
		HandUpdate(false);
	}

	public void HandUpdate(bool show)
	{
		ui.SetHover(!show);
		heldOpened = show;
		if (cardMoveCoroutine != null) StopCoroutine(cardMoveCoroutine);
		cardMoveCoroutine = StartCoroutine(HandUpdateCoroutine(!show));
	}

	private IEnumerator HandUpdateCoroutine(bool state)
	{
		List<Tuple<Transform, Vector3>> moveTuples = new();

		var heldCount = handCards.Count;
		var origin    = transforms[state ? 0 : 1];
		var position  = origin.position;
		var defaultX  = position.x + (state ? closedXOffset : openedXOffset);
		var defaultY  = position.y;
		var multiplyZ = (state ? maxZWidth : maxSubZWidth) / heldCount;
		var defaultZ  = position.z - ((heldCount - 1) * multiplyZ * .5f);

		for (int i = 0; i < heldCount; i++)
		{
			var appliedPos = new Vector3(defaultX, defaultY + i * .05f, defaultZ + multiplyZ * i);
			if (selectedCard && selectedCard == handCards[i]) appliedPos = selectedPos;
			moveTuples.Add(new Tuple<Transform, Vector3>(handCards[i].transform, appliedPos));
		}

		while (moveTuples.Count > 0)
		{
			List<Tuple<Transform, Vector3>> RemoveRequired = new();
			foreach (var tuple in moveTuples)
			{
				var tr          = tuple.Item1;
				var currentPosition    = tr.position;
				var destination = tuple.Item2;
				var alpha       = Mathf.Clamp01((cardMoveSpeed * Time.deltaTime) / Vector3.Distance(currentPosition, destination));
				var movePos     = Vector3.Lerp(currentPosition, destination, alpha);
				tr.position = movePos;
				if (!(Mathf.Abs(tr.position.magnitude - destination.magnitude) < .000001)) continue;
				tr.position = destination;
				RemoveRequired.Add(tuple);
			}

			foreach (var tuple in RemoveRequired) moveTuples.Remove(tuple);
			RemoveRequired.Clear();

			yield return null;
		}

		cardMoveCoroutine = null;
	}
}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Network;
using Unity.VisualScripting;
using UnityEngine;

namespace _root.Script.Ingame
{
public class PlayerHand : MonoBehaviour
{
	private readonly List<IngameCard> handCards  = new();
	private          List<Transform>  transforms = new();

	[SerializeField] private float cardMoveSpeed = 2;

	[SerializeField] private float closedXOffset = .1f;
	[SerializeField] private float openedXOffset = .2f;

	[SerializeField] private float maxZWidth;
	[SerializeField] private float maxSubZWidth;

	[SerializeField] private Vector3 selectedPos = new Vector3(-1, 10, 0);

	private IngameCard selectedCard;

	private bool heldUpdating;
	private bool handOpened;

	public bool isMine;

	private Camera cam;

	private bool interactable;

	public List<GameCard> HandCards => handCards.Select(x => x.GetCardData()).ToList();

	private void Awake()
	{
		cam        = Camera.main;
		transforms = GetComponentsInChildren<Transform>().ToList();
		transforms.Remove(transform);
	}

	private void Start()
	{
		selectedCard = null;
		handOpened   = false;
		HandUpdate(false);
	}

	private void Update()
	{
		if (isMine) HandShowCheck();
	}

	public void SetActive(bool active)
	{
		if (!active) SelectCard(null);
		interactable = active;
	}

	public void SelectCard(IngameCard card, bool? powerUpdate = null)
	{
		selectedCard = card;
		HandUpdate(powerUpdate is null or true && (card || handOpened));
	}

	private void HandShowCheck()
	{
		if (!interactable || selectedCard) return;
		var tr         = transforms[(handOpened ? 1 : 0)];
		var position   = tr.position;
		var localScale = tr.localScale;
		var mousePos   = Input.mousePosition;
		mousePos.z = Mathf.Abs(position.y - cam.transform.position.y);
		mousePos   = cam.ScreenToWorldPoint(mousePos);
		var inner = (mousePos.x >= position.x - (localScale.x * 0.5f)) &&
		            (mousePos.x <= position.x + (localScale.x * 0.5f)) &&
		            (mousePos.z >= position.z - (localScale.y * 0.5f)) &&
		            (mousePos.z <= position.z + (localScale.y * 0.5f));
		if (handOpened ? !inner : inner) HandUpdate(!handOpened);
	}

	public void AddCard(IngameCard card)
	{
		handCards.Add(card);
		HandUpdate(false);
	}

	public void AddCards(IEnumerable<IngameCard> cards)
	{
		handCards.AddRange(cards);
		HandUpdate(false);
	}

	public IngameCard RemoveHandCard(IngameCard card)
	{
		if (!isMine && card == null)
		{
			card = handCards[0];
			handCards.RemoveAt(0);
		}
		else if (handCards.Contains(card)) handCards.Remove(card);

		HandUpdate(false);
		return card;
	}

	public IngameCard RemoveHandCard(string id)
	{
		var card = handCards.First(x => x.GetCardData().id == id);
		handCards.Remove(card);

		HandUpdate(false);
		return card;
	}

	private void HandUpdate(bool show)
	{
		handOpened = show;

		var handCount = handCards.Count;
		var origin    = transforms[show ? 1 : 0];
		var position  = origin.position;
		var defaultX  = position.x + (show ? openedXOffset : closedXOffset);
		var defaultY  = position.y;
		var multiplyZ = (show ? maxSubZWidth : maxZWidth) / handCount;
		var defaultZ  = position.z - ((handCount - 1) * multiplyZ * .5f);

		for (var i = 0; i < handCount; i++)
		{
			var appliedPos = new Vector3(defaultX, defaultY + i * .05f, defaultZ + multiplyZ * i);
			if (selectedCard && selectedCard == handCards[i]) appliedPos = selectedPos;
			handCards[i].MoveBySpeed(appliedPos, cardMoveSpeed);
		}
	}
}
}
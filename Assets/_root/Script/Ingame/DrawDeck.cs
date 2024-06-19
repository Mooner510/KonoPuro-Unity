using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Client;
using _root.Script.Ingame;
using _root.Script.Manager;
using _root.Script.Network;
using UnityEngine;

public class DrawDeck : MonoBehaviour
{
	public bool isMine;

	private IngameCard currentCard;

	[SerializeField] private float   drawTime = 1;
	[SerializeField] private Vector3 drawPos = new Vector3(0, 10, 0);

	public void Init()
	{
		currentCard = IngameCard.CreateIngameCard(transform.position + new Vector3(0, 0.1f), Quaternion.identity);
		currentCard.Init(isMine, IngameCardType.Deck);
		currentCard.transform.rotation = Quaternion.Euler(90f, isMine ? 90f : -90f, 90f);
	}

	public void DrawCard(Action<IngameCard, bool> drawCallback, bool last, GameCard data)
	{
		if(!currentCard) return;
		var card = currentCard;
		StartCoroutine(DrawSequence(card, drawCallback, data));
		if (!last) Init();
		else currentCard = null;
	}
	
	public void DrawCards(Action<IngameCard, bool> drawCallback, bool last, int count)
	{
		var cards = new List<GameCard>()
		            { new GameCard(){}, new GameCard(){}, new GameCard(){}, new GameCard(){}};
		
		StartCoroutine(DrawMultiSequence(drawCallback, last, cards));
	}

	public void DrawCards(Action<IngameCard, bool> drawCallback, bool last, IEnumerable<GameCard> data)
	{
		StartCoroutine(DrawMultiSequence(drawCallback, last, data));
	}

	private IEnumerator DrawMultiSequence(Action<IngameCard, bool> drawCallback, bool last, IEnumerable<GameCard> data)
	{
		if(!currentCard) yield break;
		foreach (var cardData in data)
		{
			var card = currentCard;
			StartCoroutine(DrawSequence(card, drawCallback, cardData));
			Init();
			yield return new WaitForSeconds(0.1f);
		}
		
		if (!last) Init();
		else currentCard = null;
	}

	private IEnumerator DrawSequence(IngameCard card, Action<IngameCard, bool> drawCallback, GameCard data)
	{
		card.LoadDisplay(data);
		for (int i = 0; i < 5; i++)
		{
			card.transform.position += Vector3.up * (.02f * .5f);
			yield return new WaitForSeconds(0.02f);
		}

		float timer      = 0;
		var   tr = card.transform;
		var   originPos  = tr.position;
		var   originRot  = tr.rotation;
		var   destRot    = isMine ? Quaternion.Euler(-90, 0, 90) : Quaternion.Euler(90, -180, 90);
		while (timer < drawTime)
		{
			timer += Time.deltaTime;

			var alpha = timer / drawTime;
			tr.position = Vector3.Lerp(originPos, drawPos, alpha);
			tr.rotation = Quaternion.Lerp(originRot, destRot, alpha);
			yield return null;
		}
		
		AudioManager.PlaySoundInstance("Audio/CARD_SETTING");

		for (int i = 0; i < 6; i++)
		{
			card.transform.position += Vector3.up * (.05f * .5f);
			yield return new WaitForSeconds(0.05f);
		}

		card.type = IngameCardType.Hand;
		
		drawCallback.Invoke(card, isMine);
	}
}
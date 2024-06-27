using System;
using System.Collections;
using _root.Script.Client;
using _root.Script.Manager;
using _root.Script.Network;
using UnityEngine;

namespace _root.Script.Ingame
{
public enum IngameCardType
{
	Student,
	Hand,
	Field,
	Deck
}

public class IngameCard : MonoBehaviour
{
	private static GameObject ingameCardPrefab;

	public  bool           isMine;
	public  IngameCardType type;
	private GameCard       cardData;

	private Card.Card          cardFrame;
	private PlayerCardResponse defaultData;

	private bool destroyed;

	private Coroutine       moveCoroutine;
	private GameStudentCard student;

	public GameStudentCard GetStudentData()
	{
		return student;
	}

	public GameCard GetCardData()
	{
		return cardData;
	}

	public PlayerCardResponse GetData()
	{
		return defaultData;
	}

	public void Init(bool mine, IngameCardType cardType)
	{
		isMine = mine;
		type   = cardType;
		var otherHand = cardType == IngameCardType.Hand && !mine;
		if (otherHand)
			Destroy(GetComponent<BoxCollider>());
		FlipCard(!otherHand, otherHand);
	}

	public void FlipCard(bool open, bool otherSide)
	{
		var rot = transform.rotation.eulerAngles;
		rot.x              = open ? -90 : 90;
		rot.z              = otherSide ? -90 : 90;
		transform.rotation = Quaternion.Euler(rot);
	}

	public void DestroyCard()
	{
		destroyed = true;
		Destroy(gameObject);
	}

	public void MoveBySpeed(Vector3 pos, float moveSpeed)
	{
		if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		moveCoroutine = StartCoroutine(MoveBySpeedCoroutine(pos, transform.rotation, moveSpeed, 0));
	}

	public void MoveBySpeed(Vector3 pos, Quaternion rot, float moveSpeed, float rotSpeed)
	{
		if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		moveCoroutine = StartCoroutine(MoveBySpeedCoroutine(pos, rot, moveSpeed, rotSpeed));
	}

	private IEnumerator MoveBySpeedCoroutine(Vector3 pos, Quaternion rot, float moveSpeed, float rotSpeed)
	{
		while (transform.position != pos)
		{
			var tr        = transform;
			var position  = tr.position;
			var rotation  = tr.rotation;
			var moveAlpha = Mathf.Clamp01(moveSpeed * Time.deltaTime / Vector3.Distance(position, pos));
			var rotAlpha = Mathf.Clamp01(rotSpeed * Time.deltaTime / Mathf.Rad2Deg *
			                             Mathf.Acos(Vector3.Dot(rotation * Vector3.forward, rot * Vector3.forward)));
			var movePos = Vector3.Lerp(position, pos, moveAlpha);
			var moveRot = Quaternion.Lerp(rotation, rot, rotAlpha);
			tr.position = movePos;
			tr.rotation = moveRot;
			yield return null;
		}

		moveCoroutine = null;
	}

	public void MoveByRichTime(Vector3 pos, float moveRichTime)
	{
		if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		moveCoroutine = StartCoroutine(MoveByRichTimeCoroutine(pos, transform.rotation, moveRichTime, 1));
	}

	public void MoveByRichTime(Vector3 pos, Quaternion rot, float moveRichTime)
	{
		if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		moveCoroutine = StartCoroutine(MoveByRichTimeCoroutine(pos, rot, moveRichTime, moveRichTime));
	}

	public void MoveByRichTime(Vector3 pos, Quaternion rot, float moveRichTime, float rotRichTime)
	{
		if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		moveCoroutine = StartCoroutine(MoveByRichTimeCoroutine(pos, rot, moveRichTime, rotRichTime));
	}

	private IEnumerator MoveByRichTimeCoroutine(Vector3 pos, Quaternion rot, float moveRichTime, float rotRichTime)
	{
		float timer     = 0;
		var   tr        = transform;
		var   originPos = tr.position;
		var   originRot = tr.rotation;
		while (timer < Mathf.Max(moveRichTime, rotRichTime))
		{
			timer += Time.deltaTime;
			var moveAlpha = Mathf.Clamp01(timer / moveRichTime);
			var rotAlpha  = Mathf.Clamp01(timer / rotRichTime);
			var movePos   = Vector3.Lerp(originPos, pos, moveAlpha);
			var moveRot   = Quaternion.Lerp(originRot, rot, rotAlpha);
			tr.position = movePos;
			tr.rotation = moveRot;
			yield return null;
		}

		moveCoroutine = null;
	}

	public void Show(bool show, bool destroy = false, float? showTime = null, Action callback = null)
	{
		if (destroyed) return;
		if (showTime != null)
			cardFrame.Show(show, () =>
			                     { if (destroy) DestroyCard();
			                     callback?.Invoke(); }, showTime.Value);
		else
			cardFrame.Show(show, () =>
			                     { if (destroy) DestroyCard(); callback?.Invoke();});
	}

	public IngameCard LoadDisplay(GameStudentCard data)
	{
		type = IngameCardType.Student;
		if (data == null) return this;
		student = data;
		var card                          = GetComponent<Card.Card>();
		var sprite                        = ResourceManager.GetSprite(data.cardType);
		if (sprite) card.frontSide.sprite = sprite;
		return this;
	}

	public IngameCard LoadDisplay(GameCard data)
	{
		if (data == null) return this;
		cardData = data;
		var sprite                             = ResourceManager.GetSprite(data.defaultCardType);
		if (sprite) cardFrame.frontSide.sprite = sprite;
		return this;
	}

	public static IngameCard CreateIngameCard()
	{
		return CreateIngameCard(Vector3.zero, Quaternion.identity);
	}

	public static IngameCard CreateIngameCard(GameStudentCard cardData)
	{
		return CreateIngameCard(cardData, Vector3.zero, Quaternion.identity);
	}

	public static IngameCard CreateIngameCard(GameCard cardData)
	{
		return CreateIngameCard(cardData, Vector3.zero, Quaternion.identity);
	}

	public static IngameCard CreateIngameCard(Vector3 spawnPos, Quaternion spawnRot)
	{
		if (!ingameCardPrefab) ingameCardPrefab = Resources.Load<GameObject>("Prefab/Ingame Card");
		var ingameCardGO                        = Instantiate(ingameCardPrefab, spawnPos, spawnRot);
		var ingameCard                          = ingameCardGO.GetComponent<IngameCard>();
		ingameCard.cardFrame = ingameCardGO.GetComponent<Card.Card>();
		return ingameCard;
	}

	public static IngameCard CreateIngameCard(GameStudentCard cardData, Vector3 spawnPos, Quaternion spawnRot)
	{
		if (!ingameCardPrefab) ingameCardPrefab = Resources.Load<GameObject>("Prefab/Ingame Card");
		var ingameCardGO                        = Instantiate(ingameCardPrefab, spawnPos, spawnRot);
		var ingameCard                          = ingameCardGO.GetComponent<IngameCard>();
		ingameCard.cardFrame = ingameCardGO.GetComponent<Card.Card>();
		ingameCard.LoadDisplay(cardData);
		return ingameCard;
	}

	public static IngameCard CreateIngameCard(GameCard cardData, Vector3 spawnPos, Quaternion spawnRot)
	{
		if (!ingameCardPrefab) ingameCardPrefab = Resources.Load<GameObject>("Prefab/Ingame Card");
		var ingameCardGO                        = Instantiate(ingameCardPrefab, spawnPos, spawnRot);
		var ingameCard                          = ingameCardGO.GetComponent<IngameCard>();
		ingameCard.cardFrame = ingameCardGO.GetComponent<Card.Card>();
		ingameCard.LoadDisplay(cardData);
		return ingameCard;
	}
}
}
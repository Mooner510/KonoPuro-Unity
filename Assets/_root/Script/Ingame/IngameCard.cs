using System;
using System.Runtime.CompilerServices;
using _root.Script.Manager;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
	public static GameObject ingameCardPrefab;

	public bool               isMine;
	public IngameCardType     type;
	public PlayerCardResponse cardData;

	public void Init(bool mine, IngameCardType cardType)
	{
		bool otherHand = cardType == IngameCardType.Hand && !mine;
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

	public static IngameCard CreateIngameCard(PlayerCardResponse cardData)
	{
		return CreateIngameCard(cardData, Vector3.zero, Quaternion.identity);
	}

	public static IngameCard CreateIngameCard(PlayerCardResponse cardData, Vector3 spawnPos, Quaternion spawnRot)
	{
		if (!ingameCardPrefab) ingameCardPrefab = Resources.Load<GameObject>("Prefab/Ingame Card");
		var ingameCardGO                        = Instantiate(ingameCardPrefab, spawnPos, spawnRot);
		var card                                = ingameCardGO.GetComponent<Card.Card>();
		var ingameCard                          = ingameCardGO.GetComponent<IngameCard>();
		if (cardData == null) return ingameCard;
		var sprite                              = ResourceManager.GetSprite(cardData.cardType);
		card.frontSide.sprite = sprite;
		return ingameCard;
	}
}
}
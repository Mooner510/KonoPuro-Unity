using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Network;
using UnityEngine;

namespace _root.Script.Ingame
{
public class PlayerHeld : MonoBehaviour
{
	[SerializeField] private GameObject heldCardPrefab;

	private List<IngameCard>  heldCards = new();
	private List<BoxCollider> colliders = new();

	[SerializeField] private float maxZWidth;
	
	private bool cardSelected;
	private bool heldUpdating;

	private void Awake()
	{
		colliders            = GetComponentsInChildren<BoxCollider>().ToList();
		colliders[0].enabled = true;
		colliders[1].enabled = false;
	}
	
	public void SelectCard(PlayerCardResponse card)
	{
		if (card == null)
		{
			cardSelected = false;
		}
	}

	public void AddNewHeld(List<IngameCard> cards)
	{
		foreach (var card in cards)
		{
			heldCards.Add(card);
		}

		HeldUpdate();
	}

	public void HeldUpdate()
	{
		SelectCard(null);
		ShowHeld(false);
		StartCoroutine(HeldUpdateCoroutine());
	}

	IEnumerator HeldUpdateCoroutine()
	{
		heldUpdating = true;
		
		List<Tuple<Transform, Vector3>> moveTuples        = new();
		List<Tuple<Transform, Vector3>> moveReserveTuples = new();

		int   heldCount = heldCards.Count;
		float defaultX  = colliders[0].transform.position.x;
		float defaultY  = colliders[0].transform.position.y;
		float multiplyZ = maxZWidth / heldCount;
		float defaultZ  = colliders[0].transform.position.z - ((heldCount - 1) * multiplyZ * .5f);

		for (int i = 0; i < heldCount; i++)
			moveTuples.Add(new Tuple<Transform, Vector3>(heldCards[i].transform,
			                                             new Vector3(defaultX, defaultY + i * .01f, defaultZ + multiplyZ * i)));

		var first = moveTuples.First();
		moveReserveTuples.Add(first);
		moveTuples.Remove(first);

		while (moveReserveTuples.Count > 0)
		{
			List<Tuple<Transform, Vector3>> addRequired    = new();
			List<Tuple<Transform, Vector3>> RemoveRequired = new();
			foreach (var tuple in moveReserveTuples)
			{
				var tr = tuple.Item1;
				tr.position = Vector3.Lerp(tr.position, tuple.Item2, 0.08f);
				if (!(Mathf.Abs(tr.position.magnitude - tuple.Item2.magnitude) <= .0001)) continue;
				RemoveRequired.Add(tuple);
				if(moveTuples.Count < 1) continue;
				var tempTuple = moveTuples.First();
				addRequired.Add(tempTuple);
				moveTuples.Remove(tempTuple);
			}

			foreach (var tuple in addRequired)moveReserveTuples.Add(tuple);
			foreach (var tuple in RemoveRequired) moveReserveTuples.Remove(tuple);
			addRequired.Clear();
			RemoveRequired.Clear();

			yield return null;
		}

		heldUpdating = false;
		Debug.Log("End");
	}

	public void ShowHeld(bool show)
	{
		if(heldUpdating || cardSelected) return;
		if (show)
		{
			
			Debug.Log("Show");
		}
		else
		{
			Debug.Log("Close");
		}
	}

	private void OnMouseEnter()
	{
		Debug.Log("Show");
		colliders[0].enabled = false;
		colliders[1].enabled = true;
	}

	private void OnMouseExit()
	{
		Debug.Log("Close");
		colliders[0].enabled = true;
		colliders[1].enabled = false;
	}
}
}
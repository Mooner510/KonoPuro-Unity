using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Data;
using _root.Script.Network;
using UnityEngine;

public class TitleUi : MonoBehaviour
{
	[SerializeField] private GameObject baseAuths;

	[SerializeField] private GameObject loginRequired;

	private Throbber throbber;

	private void Awake()
	{
		throbber = FindObjectOfType<Throbber>();
	}

	private void Start()
	{
		gameObject.SetActive(false);
		if(Networking.AccessToken == null) Login(true); 
	}

	public void Login(bool logout)
	{
		baseAuths.SetActive(false);
		loginRequired.SetActive(!logout);

		if (!logout) return;
		baseAuths.SetActive(true);
		Networking.AccessToken           = null;
		UserData.Instance.ActiveDeck     = null;
		UserData.Instance.InventoryCards = null;
	}

	public void SetThrobber(bool active)
	{
		throbber.SetActive(active);
	}
}
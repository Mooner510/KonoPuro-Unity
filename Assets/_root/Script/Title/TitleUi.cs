using System;
using System.Collections;
using System.Collections.Generic;
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
		CoverThrobber(false);
	}

	private void Start()
	{
		gameObject.SetActive(false);
		Login(true);
	}

	public void Login(bool logout)
	{
		baseAuths.SetActive(logout);
		loginRequired.SetActive(!logout);

		if (logout)
		{
			Networking.AccessToken = null;
		}
	}

	public void CoverThrobber(bool active)
	{
		throbber.On(active);
	}
}
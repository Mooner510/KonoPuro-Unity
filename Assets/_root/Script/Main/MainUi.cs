using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUi : MonoBehaviour
{
	private Throbber   throbber;
	private GameObject interactQuitButton;

	private void Awake()
	{
		interactQuitButton = transform.GetChild(0).gameObject;
		throbber           = GetComponentInChildren<Throbber>();
	}

	private void Start()
	{
		SetInteractQuitButton(false);
	}

	public void SetThrobber(bool active)
	{
		throbber.SetActive(active);
	}

	public void SetInteractQuitButton(bool active)
	{
		interactQuitButton.SetActive(active);
	}
}

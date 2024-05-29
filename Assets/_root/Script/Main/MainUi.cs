using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUi : MonoBehaviour
{
	private GameObject interactQuitButton;
	private Throbber throbber;

	private void Awake()
	{
		interactQuitButton = transform.GetChild(0).gameObject;
		throbber           = GetComponentInChildren<Throbber>();
	}

	private void Start()
	{
		SetInteractQuitButton(false);
	}

	public void SetInteractQuitButton(bool active)
	{
		interactQuitButton.SetActive(active);
	}
}

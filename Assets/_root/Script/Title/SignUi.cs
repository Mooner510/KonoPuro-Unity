using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUi : MonoBehaviour
{
	private TMP_InputField idField;
	private TMP_InputField passwordField;
	private TMP_InputField nameField;

	private void Awake()
	{
		var fields = GetComponentsInChildren<TMP_InputField>();
		idField       = fields[0];
		passwordField = fields[1];
		if (fields.Length > 2) nameField = fields[2];
	}

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		idField.text       = "";
		passwordField.text = "";
		if (nameField) nameField.text = "";
	}

	public SignInRequest GetSignInReq()
	{
		var post = new SignInRequest()
		           { id = idField.text, password = passwordField.text };
		Init();
		return post;
	}

	public SignUpRequest GetSignUpReq()
	{
		var post = new SignUpRequest()
		           { id = idField.text, password = passwordField.text, name = nameField.text };
		Init();
		return post;
	}
}
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
	private TMP_InputField passwordReEnterField;
	private TMP_InputField nameField;

	private void Awake()
	{
		var fields = GetComponentsInChildren<TMP_InputField>();
		idField              = fields[0];
		passwordField        = fields[1];
		switch (fields.Length)
		{
			case 3:
				nameField = fields[2];
				break;
			case 4:
				passwordReEnterField = fields[2];
				nameField            = fields[3];
				break;
		}
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
		if (passwordReEnterField) passwordReEnterField.text = "";
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
		if (passwordField.text != passwordReEnterField.text) post.password = "";
		return post;
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Data;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginTest : MonoBehaviour
{
	[SerializeField] private string id;

	[SerializeField] private string name;

	[SerializeField] private string password;

	private static bool logined;

	private static GatchaResponses gatchaList;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			var req = API.SignUp(new SignUpRequest()
			                     { id = id, name = name, password = password });
			req.OnSuccess((() => Debug.Log("SignUp Success")));
			req.OnError((a => Debug.Log("SignUp Error")));
			req.Build();
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			var req = API.SignIn(new SignInRequest()
			                     { id = id, password = password });
			req.OnResponse(response =>
			               { Networking.AccessToken = response.accessToken;
			                 logined                = true; });
			req.OnError((_ => Debug.Log("SignIn Error")));
			req.Build();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(sceneBuildIndex: 1);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			API.GetActiveDeck().OnResponse((response => UserData.Instance.ActiveDeck = response))
			   .OnError((_ => Debug.Log("Get Active Deck Error"))).Build();
			API.GetInventoryCardAll().OnResponse((response => UserData.Instance.InventoryCards = response))
			   .OnError(_ => Debug.Log("Get Inventory Cards Error")).Build();
		}
		
		if (Input.GetKeyDown(KeyCode.G))
		{
			API.GatchaList().OnResponse(responses => gatchaList = responses).Build();
			foreach (var gatchaResponse in gatchaList.data)
			{
				Debug.Log(gatchaResponse.major);
			}
			
			if (gatchaList != null)
				API.GatchaMulti(gatchaList.data[0].id).OnResponse(_ => Debug.Log("Success Gatcha Multi"))
				   .OnError((_ => Debug.Log("Gatcha Multi Error"))).Build();
		}
	}
}
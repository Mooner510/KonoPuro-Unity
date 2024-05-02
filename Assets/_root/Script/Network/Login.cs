using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private TMP_InputField[] inputFields;
    private TextMeshProUGUI errorText;

    public bool isLogin;
    
    private void Awake()
    {
        isLogin = false;
        inputFields = GetComponentsInChildren<TMP_InputField>();
        errorText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SignIn()
    {
        if (inputFields[0].text == "" || inputFields[1].text == "")
        {
            ChangeText("Id or Password is empty");
            return;
        }
        var req = new SignInRequest { id = inputFields[0].text, password = inputFields[1].text};
        API.SignIn(req)
            .OnResponse(res =>
            {
                isLogin = true;
                Networking.AccessToken = res.accessToken;
            })
            .OnError(() =>
            {
                isLogin = false;
                ChangeText("Id or Password is incorrect");
            })
            .Build();
    }

    private Coroutine coroutine;

    private void ChangeText(string text)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ChangeTextCoroutine(text));
    }
    
    private IEnumerator ChangeTextCoroutine(string text)
    {
        errorText.text = text;
        yield return new WaitForSeconds(2);
        errorText.text = "";
    }
}

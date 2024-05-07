using System.Collections;
using _root.Script.Network;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    private TMP_InputField[] inputFields;
    private TextMeshProUGUI errorText;
    private TimelineManager timelineManager;
    
    private void Awake()
    {
        inputFields = GetComponentsInChildren<TMP_InputField>();
        errorText = GetComponentInChildren<TextMeshProUGUI>();
        timelineManager = FindObjectOfType<TimelineManager>();
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
                Networking.AccessToken = res.accessToken;
                timelineManager.PlayTimeline(Scenestate.SignIn, Scenestate.Lobby);
            })
            .OnError(() =>
            {
                ChangeText("Id or Password is incorrect");
            })
            .Build();
    }

    public void ToSignUpPage()
    {
        timelineManager.PlayTimeline(Scenestate.SignIn, Scenestate.SignUp);
    }
    
    //temp function
    public void ToLobbyPage()
    {
        timelineManager.PlayTimeline(Scenestate.SignIn, Scenestate.Lobby);
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

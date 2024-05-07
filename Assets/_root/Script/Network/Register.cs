using System.Collections;
using _root.Script.Network;
using TMPro;
using UnityEngine;

public class Register : MonoBehaviour
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
    
    public void SignUp()
    {
        if (inputFields[0].text == "" || inputFields[1].text == "" || inputFields[2].text == "" || inputFields[3].text == "")
        {
            ChangeText("There is an empty field");
            return;
        }
        
        if (inputFields[2].text != inputFields[3].text)
        {
            ChangeText("Password is not same");
            return;
        }

        var req = new SignUpRequest
            { name = inputFields[0].text, id = inputFields[1].text, password = inputFields[2].text };
        API.SignUp(req)
            .OnResponse(res =>
            {
                ChangeText("Sign Up Success");
                timelineManager.PlayTimeline(Scenestate.SignUp, Scenestate.SignIn);
            })
            .OnError(() =>
            {
                ChangeText("Sign Up Failed");
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

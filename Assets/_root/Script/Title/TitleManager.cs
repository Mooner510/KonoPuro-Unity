using System;
using System.Collections;
using System.Linq;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.Playables;

public class TitleManager : MonoBehaviour
{
	[SerializeField] private PlayableDirector director;

	[SerializeField] private PlayableAsset start;
	[SerializeField] private PlayableAsset end;
	[SerializeField] private GameObject LoginFaild;
	[SerializeField] private GameObject LoginSuccess;
	private TitleUi   titleUi;
	private AuthPanel authPanel;
	
	private void Awake()
	{
		titleUi                                = FindObjectOfType<TitleUi>();
		authPanel                              = FindObjectOfType<AuthPanel>();
		var spotLight = FindObjectsOfType<Light>();
		spotLight.ToList().First(x=>x.type == LightType.Spot).intensity = 0;
	}

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(StartFlow());
	}

	private IEnumerator StartFlow()
	{
		yield return new WaitForSeconds(1f);
		director.playableAsset = start;
		director.Play();
		yield return new WaitForSeconds(2.5f);
		authPanel.Show(true);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void GameStart()
	{
		titleUi.gameObject.SetActive(false);
		LoginFaild.SetActive(false);
		LoginSuccess.SetActive(false);
		StartCoroutine(GameStartFlow());
	}

	private IEnumerator GameStartFlow()
	{
		yield return new WaitForSeconds(2f);
		director.playableAsset = end;
		director.Play();
		yield return new WaitForSeconds(2.5f);
	}

	public void Sign(bool signUp)
	{
		if (signUp)
		{
			var post = authPanel.SignUp();
			if (post == null)
			{
				return;
			}
			titleUi.CoverThrobber(true);
			post.OnSuccess(SignUpSuccess).OnError(SignUpError).Build();
		}
		else
		{
			var post = authPanel.SignIn();
			if (post == null)
			{
				return;
			}
			titleUi.CoverThrobber(true);
			post.OnResponse(SignInSuccess).OnError(SignInError).Build();
		}
	}

	private void SignInSuccess(TokenResponse response)
	{
		titleUi.CoverThrobber(false);
		Networking.AccessToken = response.accessToken;
		titleUi.Login(false);
		LoginSuccess.SetActive(true);
		Invoke("LogSuccess",2f);
	}

	private void SignInError(ErrorBody errorBody)
	{
		titleUi.CoverThrobber(false);
		LoginFaild.SetActive(true);
		Invoke("LogFaild",2f);
	
	}

	private void SignUpSuccess()
	{
		titleUi.CoverThrobber(false);
	}

	private void SignUpError(ErrorBody errorBody)
	{
		titleUi.CoverThrobber(false);
	}

	void LogFaild()
	{
		LoginFaild.SetActive(false);
	}

	void LogSuccess()
	{
		LoginSuccess.SetActive(false);
	}
}
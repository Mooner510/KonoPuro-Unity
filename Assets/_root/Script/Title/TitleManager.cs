using System;
using System.Collections;
using System.Linq;
using _root.Script.Data;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	[SerializeField] private PlayableDirector director;

	[SerializeField] private PlayableAsset start;
	[SerializeField] private PlayableAsset end;

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
		if(Networking.AccessToken == null) authPanel.Show(true);
		else titleUi.Login(false);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void GameStart()
	{
		titleUi.gameObject.SetActive(false);
		StartCoroutine(GameStartFlow());
	}

	private IEnumerator GameStartFlow()
	{
		yield return new WaitForSeconds(2f);
		director.playableAsset = end;
		director.Play();
		yield return new WaitForSeconds(2.5f);
		StartCoroutine(LoadCoroutine());
		yield return new WaitUntil((() => UserData.Instance.ActiveDeck != null &&
		                                  UserData.Instance.InventoryCards != null && GameStatics.gatchaList != null));
		SceneManager.LoadScene("MainScene");
	}

	private void LoadData()
	{
		if (UserData.Instance.ActiveDeck == null)
		{
			API.GetActiveDeck().OnResponse(response => UserData.Instance.ActiveDeck = response)
			   .OnError((body => Debug.Log("Active Deck Load Failed"))).Build();
		}

		if (UserData.Instance.InventoryCards == null)
		{
			API.GetInventoryCardAll().OnResponse(responses => UserData.Instance.InventoryCards = responses)
			   .OnError((body => Debug.Log("Inventory Cards Load Failed"))).Build();
		}

		if (GameStatics.gatchaList == null)
		{
			API.GatchaList()
			   .OnResponse(responses =>
			               { GameStatics.gatchaList = responses.data; })
			   // .OnResponse(responses => GameStatics.gatchaList = responses.data
			   //                                                            .Where(x =>
						// 		                                                                DateTime
						// 				                                                               .Compare(DateTime.Parse(x.startAt),
						// 						                                                                 DateTime
						// 								                                                                .Now) !=
						// 		                                                                1 &&
						// 		                                                                DateTime
						// 				                                                               .Compare(DateTime.Parse(x.endAt),
						// 						                                                                 DateTime
						// 								                                                                .Now) !=
						// 		                                                                -1).ToList())
			   .OnError((body => Debug.Log("Gatcha List Load Failed")))
			  .Build();
		}
	}

	private IEnumerator LoadCoroutine()
	{
		const float iterTime  = 3f;
		const int   iterCount = 2;
		
		for (int i = 0; i < iterCount; i++)
		{
			LoadData();
			yield return new WaitForSeconds(iterTime);
			if (UserData.Instance.ActiveDeck != null &&
			    UserData.Instance.InventoryCards != null && GameStatics.gatchaList != null)
			{
				yield break;
			}
		}
		if (UserData.Instance.ActiveDeck != null &&
		    UserData.Instance.InventoryCards != null && GameStatics.gatchaList != null)
		{
			yield break;
		}
		Application.Quit();
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
			titleUi.SetThrobber(true);
			post.OnSuccess(SignUpSuccess).OnError(SignUpError).Build();
		}
		else
		{
			var post = authPanel.SignIn();
			if (post == null)
			{
				return;
			}
			titleUi.SetThrobber(true);
			post.OnResponse(SignInSuccess).OnError(SignInError).Build();
		}
	}

	private void SignInSuccess(TokenResponse response)
	{
		titleUi.SetThrobber(false);
		Networking.AccessToken = response.accessToken;
		titleUi.Login(false);
	}

	private void SignInError(ErrorBody errorBody)
	{
		titleUi.SetThrobber(false);
	}

	private void SignUpSuccess()
	{
		titleUi.SetThrobber(false);
	}

	private void SignUpError(ErrorBody errorBody)
	{
		titleUi.SetThrobber(false);
	}
}
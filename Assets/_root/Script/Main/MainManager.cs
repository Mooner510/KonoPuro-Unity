using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Data;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	private MainUi               mainUi;
	private CinemacineController cineController;
	private PlayableDirector     director;

	[SerializeField] private PlayableAsset start;

	private PlaceableObject hoveredPlaceableObject;
	private Camera          mainCam;

	private bool isInteracting;

	private void Awake()
	{
		NetworkClient.Init();
		mainUi         = FindObjectOfType<MainUi>();
		cineController = FindObjectOfType<CinemacineController>();
		director       = FindObjectOfType<PlayableDirector>();
		var spotLight = FindObjectsOfType<Light>();
		spotLight.ToList().First(x => x.type == LightType.Spot).intensity = 0;
	}

	private void Start()
	{
		mainCam                   =  Camera.main;
		StartCoroutine(StartFlow());
		NetworkClient.DelegateEvent(NetworkClient.ClientEvent.GameStarted, _=>GameStart());
	}

	private void Update()
	{
		CheckPlaceable();
	}

	private void CheckPlaceable()
	{
		if (isInteracting) return;
		var ray = mainCam.ScreenPointToRay(Input.mousePosition);

		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

		if (Physics.Raycast(ray, out var hit))
		{
			var placeable = hit.transform.GetComponent<PlaceableObject>();
			if (!placeable)
			{
				if(hoveredPlaceableObject) hoveredPlaceableObject.OnHover(false);
				hoveredPlaceableObject = null;
			}
			if (hoveredPlaceableObject)
			{
				if (placeable != hoveredPlaceableObject)
				{
					hoveredPlaceableObject.OnHover(false);
					placeable.OnHover(true);
					hoveredPlaceableObject = placeable;
				}
			}
			else
			{
				placeable.OnHover(true);
				hoveredPlaceableObject = placeable;
			}
		}
		else
		{
			if(hoveredPlaceableObject) hoveredPlaceableObject.OnHover(false);
			hoveredPlaceableObject = null;
			return;
		}

		if (!hoveredPlaceableObject || !Input.GetMouseButtonDown(0)) return;
		var cam = hoveredPlaceableObject.Interact();
		if (cam == CinemacineController.VCamName.None) return;

		isInteracting = true;
		if (cam == CinemacineController.VCamName.Matching) Matching();
		else mainUi.SetInteractQuitButton(true);
		cineController.SetPriority(cam);
	}

	public void QuitInteract()
	{
		if (!isInteracting) return;
		isInteracting = false;
		mainUi.SetInteractQuitButton(false);
		hoveredPlaceableObject.Init();
		cineController.SetPriority(CinemacineController.VCamName.Overview);
	}

	public void Matching()
	{
		mainUi.SetThrobber(true);
		mainUi.SetMatchingCancelButton(true);
		mainUi.SetExitButton(false);
		mainUi.SetTitleButton(false);

		API.Match().OnSuccess((() => mainUi.SetThrobber(false))).OnError((body => MatchingExit(true))).Build();
	}

	public void MatchingExit(bool onError)
	{
		if (onError)
		{
			mainUi.SetMatchingCancelButton(false);
			mainUi.SetThrobber(false);
			QuitInteract();
			return;
		}

		mainUi.SetThrobber(true);

		API.MatchCancel().OnResponse((_ =>
		                              { mainUi.SetThrobber(false);
		                                mainUi.SetMatchingCancelButton(false);
		                                QuitInteract();
		                                mainUi.SetExitButton(true);
		                                mainUi.SetTitleButton(true); })).OnError(_ =>
					 { mainUi.SetThrobber(false);
					   Debug.LogError("error"); }).Build();
	}

	public void GameStart()
	{
		StartCoroutine(GameStartFlow());
	}

	private IEnumerator GameStartFlow()
	{
		mainUi.SetThrobber(true);
		yield return new WaitForSeconds(1);
		var selfStudent  = GameStatics.self?.student;
		var otherStudent = GameStatics.other?.student;
		if (selfStudent == null || otherStudent == null)
		{
			Debug.LogError("game start student cards data is null");
			yield return new WaitUntil((() => selfStudent != null && otherStudent != null));
		}
		mainUi.SetThrobber(false);
		SceneManager.LoadSceneAsync("IngameScene");
	}

	private IEnumerator StartFlow()
	{
		isInteracting = true;
		yield return new WaitForSeconds(1f);
		director.playableAsset = start;
		director.Play();
		director.stopped += (_ => isInteracting = false);
	}
}
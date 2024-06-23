using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Data;
using _root.Script.Ingame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionModeUi : MonoBehaviour
{
	private Button accept;
	private Button cancel;

	private int selectCount;

	private List<IngameCard> selectableCards;
	private List<IngameCard> selectedCards;
	
	private TextMeshProUGUI turnhandcarduse;
	private GameObject textpannel;
	private Animator carduseageAnim;
	private void Awake()
	{
		textpannel = GameObject.FindGameObjectWithTag("CardUseage");
		carduseageAnim = textpannel.GetComponent<Animator>();
		turnhandcarduse = textpannel.GetComponentInChildren<TextMeshProUGUI>();
		var buttons = GetComponentsInChildren<Button>();
		accept = buttons[0];
		cancel = buttons[1];
	}

	private void Start()
	{
		SetActive(false);
	}

	private void Update()
	{
		var acceptable                                             = selectedCards == null || selectedCards.Count == selectCount;
		if (acceptable != accept.interactable) accept.interactable = acceptable;
	}

	public void SetActive(bool active)
	{
		if (!active)
		{
			selectableCards = null;
			selectedCards   = null;
			accept.onClick.RemoveAllListeners();
			cancel.onClick.RemoveAllListeners();
		}

		accept.interactable = active && (selectedCards == null || selectableCards.Count == selectCount);
		cancel.interactable = active;
		gameObject.SetActive(active);
	}

	public void SetActive(Action<bool, List<IngameCard>> callback)
	{
		selectableCards = null;
		selectCount     = 0;

		accept.onClick.RemoveAllListeners();
		cancel.onClick.RemoveAllListeners();
		accept.onClick.AddListener(() => callback(true, null));
		accept.onClick.AddListener(() => SetActive(false));
		cancel.onClick.AddListener(() => callback(false, null));
		cancel.onClick.AddListener(() => SetActive(false));

		SetActive(true);
	}

	public void SetActive(List<IngameCard> _selectableCards, int count, Action<bool, List<IngameCard>> callback)
	{
		selectCount     = count;
		selectableCards = _selectableCards;

		accept.onClick.RemoveAllListeners();
		cancel.onClick.RemoveAllListeners();
		accept.onClick.AddListener(() => callback(true, selectedCards));
		accept.onClick.AddListener(() => SetActive(false));
		cancel.onClick.AddListener(() => callback(false, null));
		cancel.onClick.AddListener(() => SetActive(false));

		SetActive(true);
	}

	public void SayOutLoud()
	{
		turnhandcarduse.text = PlayerActivity.usingcard;
		carduseageAnim.Play("CardUseage");
		if (GameStatics.isTurn == true)
		{
			textpannel.GetComponent<Image>().color = new Color(0.34f, 0.73f, 1f, 1f);
		}
		else
		{
			textpannel.GetComponent<Image>().color = new Color(1f, 0.42f, 0.34f, 1f);
		}
	}
	

}
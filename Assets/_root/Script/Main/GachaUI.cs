using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Data;
using _root.Script.Main;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GachaUI : MonoBehaviour
{
	private Canvas canvas;
	private MainUi mainUi;

	[Header("# Gold")] [SerializeField] private TextMeshProUGUI goldText;

	[Header("# Info")] [SerializeField] private GameObject infoPanel;
	private                                     bool       infoToggle;

	[Header("# Box")] [SerializeField] private GameObject     box;
	[SerializeField]                   private List<Mesh>     boxMeshes;
	[SerializeField]                   private List<Material> boxMaterials;
	private                                    MeshFilter     meshFilter;
	private                                    MeshRenderer   meshRenderer;
	private                                    int            boxIndex = 0;

	[Header("# Gacha")] [SerializeField] private TextMeshProUGUI singlePriceTxt;
	[SerializeField]                     private TextMeshProUGUI multiPriceTxt;

	private int                      gatchaPrice;
	private List<PlayerCardResponse> gatchaCards = new();

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		mainUi = FindObjectOfType<MainUi>();
	}

	private void Start()
	{
		meshFilter          = box.GetComponent<MeshFilter>();
		meshRenderer        = box.GetComponent<MeshRenderer>();
		singlePriceTxt.text = string.Format($"{GameStatics.gatchaOncePrice:N0}");
		multiPriceTxt.text =
				string.Format($"<color=#54d5ff><s>{GameStatics.gatchaMultiPrice + 1:N0}</s></color>\n{GameStatics.gatchaMultiPrice:N0}");
		SetActive(false);
	}

	public void SetActive(bool active)
	{
		canvas.enabled = active;
		gameObject.SetActive(active);
		if (active) ChangeGoldTxt(UserData.Instance.gold);
	}

	public void LeftBtn()
	{
		if (boxIndex <= 0) return;
		boxIndex--;
		ChangeBox();
	}

	public void RightBtn()
	{
		if (boxIndex >= boxMeshes.Count - 1) return;
		boxIndex++;
		ChangeBox();
	}

	void ChangeBox()
	{
		meshFilter.mesh       = boxMeshes[boxIndex];
		meshRenderer.material = boxMaterials[boxIndex];
	}

	private void GatchaInit()
	{
		gatchaPrice = 0;
		gatchaCards.Clear();
	}

	public void Gatcha(bool multi)
	{
		GatchaInit();
		gatchaPrice = multi ? GameStatics.gatchaMultiPrice : GameStatics.gatchaOncePrice;
		if (UserData.Instance.gold < gatchaPrice)
		{
			//TODO: 골드 부족 표시
			return;
		}

		if (multi)
			API.GatchaMulti(GameStatics.gatchaList[boxIndex].id).OnResponse(response =>
			                                                                { gatchaCards.Clear();
			                                                                  gatchaCards.AddRange(response.cards);
			                                                                  GatchaSuccess(); }).OnError(GatchaError)
			   .Build();
		else
			API.GatchaOnce(GameStatics.gatchaList[boxIndex].id).OnResponse(response =>
			                                                               { gatchaCards.Clear();
			                                                                 gatchaCards.Add(response);
			                                                                 GatchaSuccess(); }).OnError(GatchaError)
			   .Build();
		mainUi.SetThrobber(true);
	}

	private void GatchaSuccess()
	{
		mainUi.SetThrobber(false);
		var gold = UserData.Instance.gold -= gatchaPrice;
		ChangeGoldTxt(gold);
		UserData.Instance.InventoryCards.cards.AddRange(gatchaCards);
		GachaDirecting.gatchaCards = gatchaCards;
		SceneManager.LoadScene("GachaDirectingStartScene");
	}

	private void GatchaError(ErrorBody body)
	{
		mainUi.SetThrobber(false);
	}

	public void ToggleInfo()
	{
		infoToggle = !infoToggle;
		infoPanel.SetActive(infoToggle);
	}

	private void ChangeGoldTxt(int gold)
	{
		goldText.text = string.Format($"{gold:N0}");
	}

	public void DoGacha()
	{
		SceneManager.LoadScene("GachaScene");
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Data;
using _root.Script.Ingame;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngameUi : MonoBehaviour
{
	private TextMeshProUGUI dayText;

	private Button          sleepButton;
	private TextMeshProUGUI selfTimeText;
	private TextMeshProUGUI otherTimeText;

	private Slider selfProgressSlider;
	private Slider otherProgressSlider;

	private TextMeshProUGUI selfProgressText;
	private TextMeshProUGUI otherProgressText;

	private ProgressDetailUi selfProgressDetail;
	private ProgressDetailUi otherProgressDetail;
	
	private IngameCardInfoUi ingameCardInfoUi;
	private TurnDisplayUi    turnDisplayUi;

	private EventTrigger selfDetailHover;
	private EventTrigger otherDetailHover;

	private void Awake()
	{
		var textMeshProUis = GetComponentsInChildren<TextMeshProUGUI>();
		dayText = textMeshProUis[0];

		var sliderUis = GetComponentsInChildren<Slider>();
		selfProgressSlider  = sliderUis[0];
		otherProgressSlider = sliderUis[1];

		selfProgressText  = textMeshProUis[1];
		otherProgressText = textMeshProUis[2];

		sleepButton = GetComponentInChildren<Button>();

		selfTimeText  = textMeshProUis[3];
		otherTimeText = textMeshProUis[4];

		var details = GetComponentsInChildren<ProgressDetailUi>();
		selfProgressDetail  = details[0];
		otherProgressDetail = details[1];

		var detailHovers = GetComponentsInChildren<EventTrigger>();
		selfDetailHover  = detailHovers[0];
		otherDetailHover = detailHovers[1];

		ingameCardInfoUi = FindObjectOfType<IngameCardInfoUi>();

		turnDisplayUi = FindObjectOfType<TurnDisplayUi>();
	}

	public void Init(int day, GameStatus self, GameStatus other)
	{
		dayText.text = $"D - {(day == GameStatics.dDay ? "Day" : GameStatics.dDay - day)}";

		selfTimeText.text  = $"{self.time}";
		otherTimeText.text = $"{other.time}";

		selfProgressSlider.value  = self.totalProgress;
		otherProgressSlider.value = other.totalProgress;

		selfProgressText.text  = "0%";
		otherProgressText.text = "0%";

		selfProgressDetail.Init(self.detailProgresses);
		otherProgressDetail.Init(other.detailProgresses);
	}

	public void SetHover(bool active)
	{
		sleepButton.enabled      = active;
		selfDetailHover.enabled  = active;
		otherDetailHover.enabled = active;
	}

	public void DayChange(int day)
	{
		dayText.text = $"Day - {day}";
	}

	public void TimeChanged(int time, bool self)
	{
		(self ? selfTimeText : otherTimeText).text  = $"{time}";
	}

	public void SetProgress(float progress, bool self)
	{
		(self ? selfProgressSlider : otherProgressSlider).value = progress;
		(self ? selfProgressText : otherProgressText).text      = $"{progress:P2}";
	}

	public void SetProgressDetail(Dictionary<MajorType, int> projects, bool self)
	{
		var totalProgress = GameStatics.CalcTotalProgress((self ? selfProgressDetail : otherProgressDetail).SetProgresses(projects));
		SetProgress(totalProgress, self);
	}

	public void SetCardInfo(IngameCard card)
	{
		ingameCardInfoUi.SetCard(card);
	}

	public void TurnSet(bool myTurn)
	{
		turnDisplayUi.TurnNotify(myTurn);
	}

	public void InteractBlock(bool active)
	{
		sleepButton.interactable = active;
	}
}
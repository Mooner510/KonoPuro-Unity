using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngameUi : MonoBehaviour
{
	private TextMeshProUGUI dDayText;
	private TextMeshProUGUI currentDayText;

	private TextMeshProUGUI selfTimeText;
	private TextMeshProUGUI otherTimeText;

	private Slider selfProgressSlider;
	private Slider otherProgressSlider;

	private TextMeshProUGUI selfProgressText;
	private TextMeshProUGUI otherProgressText;

	private ProgressDetailUi selfProgressDetail;
	private ProgressDetailUi otherProgressDetail;

	private void Awake()
	{
		var textMeshProUis = GetComponentsInChildren<TextMeshProUGUI>();
		dDayText       = textMeshProUis[0];
		currentDayText = textMeshProUis[1];


		var sliderUis = GetComponentsInChildren<Slider>();
		selfProgressSlider  = sliderUis[0];
		otherProgressSlider = sliderUis[1];

		selfProgressText  = textMeshProUis[2];
		otherProgressText = textMeshProUis[3];
		
		selfTimeText      = textMeshProUis[4];
		otherTimeText     = textMeshProUis[5];
		
		var details = GetComponentsInChildren<ProgressDetailUi>();
		selfProgressDetail  = details[0];
		otherProgressDetail = details[1];
	}

	public void Init(int dDay, int day, GameStatus self, GameStatus other)
	{
		dDayText.text       = $"D-Day {dDay}";
		currentDayText.text = $"Day - {day}";

		selfTimeText.text  = $"{self.time}";
		otherTimeText.text = $"{other.time}";

		selfProgressSlider.value  = self.totalProgress;
		otherProgressSlider.value = other.totalProgress;

		selfProgressText.text  = "0%";
		otherProgressText.text = "0%";
		
		selfProgressDetail.Init(self.detailProgresses);
		otherProgressDetail.Init(other.detailProgresses);
	}

	public void DayChange(int day)
	{
		currentDayText.text = $"Day - {day}";
	}

	public void SetCurrentTime(int otherTime, int selfTime)
	{
		selfTimeText.text  = $"{selfTime}";
		otherTimeText.text = $"{otherTime}";
	}

	public void SetProgress(float otherProgress, float selfProgress)
	{
		selfProgressSlider.value  = selfProgress;
		otherProgressSlider.value = otherProgress;

		selfProgressText.text  = $"{selfProgress * 100}%";
		otherProgressText.text = $"{otherProgress * 100}%";
	}

	public void SetProgressDetail()
	{
 		//TODO: 소켓 연결 시 추가
	}
}

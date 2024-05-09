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

	private TextMeshProUGUI otherTimeText;
	private TextMeshProUGUI selfTimeText;

	private Slider otherProgressSlider;
	private Slider selfProgressSlider;

	private TextMeshProUGUI otherProgressText;
	private TextMeshProUGUI selfProgressText;

	private ProgressDetailUi otherProgressDetail;
	private ProgressDetailUi selfProgressDetail;

	private void Awake()
	{
		var textMeshProUis = GetComponentsInChildren<TextMeshProUGUI>();
		dDayText       = textMeshProUis.First(s => s.gameObject.name == "D Day");
		currentDayText = textMeshProUis.First(s => s.gameObject.name == "Current Day");

		otherTimeText = textMeshProUis.First(s => s.gameObject.name == "Other Time");
		selfTimeText  = textMeshProUis.First(s => s.gameObject.name == "Self Time");

		var sliderUis = GetComponentsInChildren<Slider>();
		otherProgressSlider = sliderUis.First(x => x.gameObject.name == "Other Progress");
		selfProgressSlider  = sliderUis.First(x => x.gameObject.name == "Self Progress");

		otherProgressText = textMeshProUis.First(x => x.gameObject.name == "Other Progress Text");
		selfProgressText  = textMeshProUis.First(x => x.gameObject.name == "Self Progress Text");

		var details = GetComponentsInChildren<ProgressDetailUi>();
		otherProgressDetail = details.First(x => x.gameObject.name == "Other Progress Detail");
		selfProgressDetail  = details.First(x => x.gameObject.name == "Self Progress Detail");
	}

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		const int dDay = 15;
		dDayText.text       = $"D-Day {dDay}";
		currentDayText.text = "Day - 1";

		otherTimeText.text = "24";
		selfTimeText.text  = "24";

		otherProgressSlider.value = 0;
		selfProgressSlider.value  = 0;

		otherProgressText.text = "0%";
		selfProgressText.text  = "0%";
	}

	public void SetCurrentDay(int day)
	{
		currentDayText.text = $"Day - {day}";
	}

	public void SetCurrentTime(int otherTime, int selfTime)
	{
		otherTimeText.text = $"{otherTime}";
		selfTimeText.text  = $"{selfTime}";
	}

	public void SetProgress(float otherProgress, float selfProgress)
	{
		otherProgressSlider.value = otherProgress;
		selfProgressSlider.value  = selfProgress;

		otherProgressText.text = $"{otherProgress * 100}%";
		selfProgressText.text  = $"{selfProgress * 100}%";
	}

	public void SetProgressDetail()
	{
 		//TODO: 소켓 연결 시 추가
	}
}

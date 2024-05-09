using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDetailUi : MonoBehaviour
{
	private Coroutine coroutine;

	[SerializeField] private float insideX;
	[SerializeField] private float outsideX;

	[SerializeField] private float          richTime;

	private RectTransform rect;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	private void Start()
	{
		var pos  = rect.anchoredPosition;
		pos.x                 = outsideX;
		rect.anchoredPosition = pos;
	}

	public void Show(bool show)
	{
		Debug.Log("show");
		if (coroutine != null) StopCoroutine(coroutine);
		coroutine = StartCoroutine(ShowCoroutine(show ? -1 : 1));
	}

	private IEnumerator ShowCoroutine(float destination)
	{
		var timer = (insideX - rect.anchoredPosition.x) / (insideX - outsideX) * richTime;
		Debug.Log($"{insideX}, {outsideX}, {rect.anchoredPosition.x}, {destination}, {timer}, {richTime}");	
		while (timer < richTime || timer > 0)
		{
			timer += Time.deltaTime * destination;

			var pos = rect.anchoredPosition;
			pos.x = Mathf.Lerp(insideX, outsideX, Mathf.Clamp01(timer / richTime));
			rect.anchoredPosition = pos;
			yield return null;
		}
	}
}

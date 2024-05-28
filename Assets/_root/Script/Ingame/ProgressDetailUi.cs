using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Client;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProgressDetailUi : MonoBehaviour
{
	[SerializeField] private GameObject elementPrefab;

	private readonly List<ProgressDetailElementUi> elementUis = new();
			
	private                  RectTransform rect;
	[SerializeField] private float         insideX;
	[SerializeField] private float         outsideX;
	[SerializeField] private float         richTime;
	private                  Coroutine     coroutine;

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

	public void Init(List<DetailProgressInfo> infos)
	{
		foreach (var progressDetailElementUi in elementUis)
			Destroy(progressDetailElementUi.gameObject);

		foreach (var info in infos)
		{
			var element = Instantiate(elementPrefab, transform).GetComponent<ProgressDetailElementUi>();
			element.Init(info);
		}
	}
	
	public void Show(bool show)
	{
		if (coroutine != null) StopCoroutine(coroutine);
		coroutine = StartCoroutine(ShowCoroutine(show ? -1 : 1));
	}

	private IEnumerator ShowCoroutine(float destination)
	{
		var timer = (insideX - rect.anchoredPosition.x) / (insideX - outsideX) * richTime;
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

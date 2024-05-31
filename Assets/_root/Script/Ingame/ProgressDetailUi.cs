using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Client;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ProgressDetailUi : MonoBehaviour
{
	[SerializeField] private GameObject elementPrefab;

	private readonly List<ProgressDetailElementUi> elementUis = new();

	private                  CanvasGroup canvasGroup;
	private                  float       originAlpha;
	[SerializeField] private float       richTime;
	private                  Coroutine   coroutine;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Start()
	{
		canvasGroup.alpha = 0;
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
		coroutine = StartCoroutine(ShowCoroutine(show ? 1 : -1));
	}

	private IEnumerator ShowCoroutine(float destination)
	{
		var timer = canvasGroup.alpha * richTime;
		while (timer <= richTime && timer >= 0)
		{
			timer += Time.deltaTime * destination;

			canvasGroup.alpha = Mathf.Lerp(0, 1, Mathf.Clamp01(timer / richTime));

			yield return null;
		}
	}
}
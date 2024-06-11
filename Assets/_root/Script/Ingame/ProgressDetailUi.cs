using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Network;
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
			elementUis.Add(element);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>total progress</returns>
	public int SetProgresses(Dictionary<MajorType, int> projects)
	{
		foreach (var pair in projects)
		{
			var element = elementUis.First(x => x.type == pair.Key);
			if(element) element.SetProgress(pair.Value);
		}

		return elementUis.Select(x => x.progress).Sum();
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
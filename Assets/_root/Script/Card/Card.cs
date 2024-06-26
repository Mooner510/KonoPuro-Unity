using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace _root.Script.Card
{
public class Card : MonoBehaviour
{
	private const            float          showFadeTime = .3f;
	private static readonly  int            Alpha        = Shader.PropertyToID("_Alpha");
	[HideInInspector] public SpriteRenderer frontSide;
	[HideInInspector] public SpriteRenderer backSide;

	private Coroutine showCoroutine;

	public void Start()
	{
		frontSide       = transform.GetChild(0).GetComponent<SpriteRenderer>();
		backSide        = transform.GetChild(1).GetComponent<SpriteRenderer>();
		backSide.sprite = Resources.Load<Sprite>("Card/card_frame");
	}

	public void Show(bool show, Action callback, float showTime = showFadeTime)
	{
		if (showCoroutine != null) StopCoroutine(showCoroutine);
		showCoroutine = StartCoroutine(ShowCoroutine(show, callback, showTime));
	}

	private IEnumerator ShowCoroutine(bool show, Action callback, float showTime)
	{
		var front = frontSide.material;
		var back  = backSide.material;

		var timer     = front.GetFloat(Alpha) * showFadeTime;
		var different = show ? 1 : -1;
		while (timer is <= showFadeTime and >= 0)
		{
			timer += Time.deltaTime * different;

			var alpha = Mathf.Clamp01(timer / showFadeTime);
			front.SetFloat(Alpha, alpha);
			back.SetFloat(Alpha, alpha);

			yield return null;
		}

		frontSide.shadowCastingMode = show ? ShadowCastingMode.On : ShadowCastingMode.Off;
		backSide.shadowCastingMode  = show ? ShadowCastingMode.On : ShadowCastingMode.Off;
		front.SetFloat(Alpha, show ? 1 : 0);
		back.SetFloat(Alpha, show ? 1 : 0);

		callback();
	}
}
}
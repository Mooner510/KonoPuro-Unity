using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throbber : MonoBehaviour
{
	private RectTransform throbberRect;

	[SerializeField] private float speed = 100;
	
	private Coroutine coroutine;

	private void Awake()
	{
		throbberRect = transform.GetChild(0).GetComponent<RectTransform>();
	}

	public void On(bool on)
	{
		gameObject.SetActive(on);
		if (coroutine != null) StopCoroutine(coroutine);
		if (on) coroutine = StartCoroutine(Rotate());
	}

	private IEnumerator Rotate()
	{
		while (true)
		{
			throbberRect.Rotate(new Vector3(0, 0, 1), speed * Time.deltaTime);
			yield return null;
		}
	}
}

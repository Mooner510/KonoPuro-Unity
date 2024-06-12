using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableObject : MonoBehaviour
{
	[SerializeField]
	private bool interactable;

	[SerializeField] private CinemacineController.VCamName cam;

	[SerializeField] private UnityEvent interactEvent;
	[SerializeField] private UnityEvent initEvent;
	
	private void Start()
	{
		Init();
	}

	public void Init()
	{
		initEvent.Invoke();
	}

	public void OnHover(bool active)
	{

	}

	public CinemacineController.VCamName Interact()
	{
		if (!interactable) return CinemacineController.VCamName.None;
		interactEvent.Invoke();
		return cam;
	}
}
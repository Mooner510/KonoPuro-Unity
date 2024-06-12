using System;
using Unity.VisualScripting;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableObject : MonoBehaviour
{
	[SerializeField]
	private bool interactable;

	[SerializeField] private CinemacineController.VCamName cam;

	[SerializeField] private UnityEvent interactEvent;
	[SerializeField] private UnityEvent initEvent;
	private Material accentMaterial;

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
		if (active)
		{
			Debug.Log("UM");
			gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("_OutlineColor", new Color(255,128,0));
			gameObject.GetComponent<MeshRenderer>().materials[1].SetFloat("_Scale",1.015f);
		}
		else
		{
			Debug.Log("UM");
			gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("_OutlineColor",Color.white);
			gameObject.GetComponent<MeshRenderer>().materials[1].SetFloat("_Scale",1.01f);
		}
	}

	public CinemacineController.VCamName Interact()
	{
		if (!interactable) return CinemacineController.VCamName.None;
		interactEvent.Invoke();
		return cam;
	}
}
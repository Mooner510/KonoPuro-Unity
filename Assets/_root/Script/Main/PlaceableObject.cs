using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableObject : MonoBehaviour
{
    [SerializeField] private bool interactable;

    [SerializeField] private CinemacineController.VCamName cam;
    [SerializeField] private GameObject DiscriptionText;
    [SerializeField] private UnityEvent interactEvent;
    [SerializeField] private UnityEvent initEvent;
    private Material accentMaterial;
    private TMP_Text DiscriptionText_TMP_Text;

    private void Start()
    {
        StartCoroutine(ShowDiscription(2.5f));
        accentMaterial = gameObject.GetComponent<MeshRenderer>().materials[1];
        DiscriptionText_TMP_Text = DiscriptionText.GetComponent<TMP_Text>();
        Init();
    }

    public void Init()
    {
        initEvent?.Invoke();
    }

    public void OnHover(bool active)
    {
        if (active)
        {
            accentMaterial.SetColor("_OutlineColor", new Color(255, 128, 0));
            accentMaterial.SetFloat("_Scale", 1.015f);
            DiscriptionText_TMP_Text.color = Color.white;
        }
        else
        {
            DiscriptionText_TMP_Text.color = Color.gray;
            accentMaterial.SetFloat("_Scale", 1f);
        }
    }

    public CinemacineController.VCamName Interact()
    {
        if (!interactable) return CinemacineController.VCamName.None;
        DiscriptionText_TMP_Text.color = Color.black;
        interactEvent.Invoke();
        return cam;
    }

    private IEnumerator ShowDiscription(float Time)
    {
        yield return new WaitForSeconds(Time);
        DiscriptionText.SetActive(true);
    }
}
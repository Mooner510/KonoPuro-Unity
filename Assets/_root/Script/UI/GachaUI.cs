using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour {
    [Header("# Gold")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private int gold;
    
    [Header("# Info")]
    [SerializeField] private GameObject infoPanel;
    private bool infoToggle;
    
    [Header("# Box")]
    [SerializeField] private GameObject box;
    [SerializeField] private List<Mesh> boxMeshes;
    [SerializeField] private List<Material> boxMaterials;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private int boxIndex;

    [Header("# Gacha")]
    [SerializeField] private TextMeshProUGUI singlePriceTxt;
    [SerializeField] private TextMeshProUGUI multiPriceTxt;
    [SerializeField] int gachaPrice;

    private void Start() {
        meshFilter = box.GetComponent<MeshFilter>();
        meshRenderer = box.GetComponent<MeshRenderer>();
        
        singlePriceTxt.text = string.Format($"{gachaPrice:N0}");
        multiPriceTxt.text = string.Format($"<color=#54d5ff><s>{gachaPrice*10:N0}</s>\u21b4</color>\n{gachaPrice*10-1:N0}");
        ChangeGoldTxt(gold);
    }

    public void LeftBtn() {
        if (boxIndex <= 0) return;
        boxIndex--;
        ChangeBox();
    }

    public void RightBtn() {
        if (boxIndex >= boxMeshes.Count-1) return;
        boxIndex++;
        ChangeBox();
    }

    void ChangeBox() {
        meshFilter.mesh = boxMeshes[boxIndex];
        meshRenderer.material = boxMaterials[boxIndex];
    }

    public void DoGacha(int gachaNum) {
        gold -= gachaNum * gachaPrice;
        ChangeGoldTxt(gold);
        // 가챠
    }

    public void BackBtn() {
        // 이전화면
    }

    public void ToggleInfo() {
        infoToggle = !infoToggle;
        infoPanel.SetActive(infoToggle);
    }

    public void ChangeGoldTxt(int gold) {
        goldText.text = string.Format($"{gold:N0}");
    }
}

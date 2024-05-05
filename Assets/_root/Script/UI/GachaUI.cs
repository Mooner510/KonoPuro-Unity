using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _root.Script.Network;
using Random = UnityEngine.Random;

public class GachaUI : MonoBehaviour {
    
    [Header("# CutScene")]
    [SerializeField] private GameObject camera;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private GameObject ui;
    [SerializeField] private float time;
    [SerializeField] private GameObject light;
    
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
    [SerializeField] private List<GatchaResponse> data;
    [SerializeField] private List<string> gachaList;
    [SerializeField] private string currentBanner;
    [SerializeField] private List<PlayerCardResponse> gachaResult;

    [Header("CardOpen")]
    [SerializeField] private GameObject posObj;
    [SerializeField] private GameObject cardGroup;
    [SerializeField] private List<Transform> cardPos;
    [SerializeField] private GameObject card;
    [SerializeField] private Transform gachaRoomPos;
    [SerializeField] private GameObject particleCanvas;
    [SerializeField] private float spawnDistance;

    private void Start() {
        meshFilter = box.GetComponent<MeshFilter>();
        meshRenderer = box.GetComponent<MeshRenderer>();
        
        GetBanner();
        
        singlePriceTxt.text = string.Format($"{gachaPrice:N0}");
        multiPriceTxt.text = string.Format($"<color=#54d5ff><s>{gachaPrice*10:N0}</s></color>\n{gachaPrice*10-1:N0}");
        ChangeGoldTxt(gold);
        ui.SetActive(false);
        GachaToggle(startPos, endPos, time);
    }

    private void GetBanner() {
        API.GatchaList()
            .OnResponse(res => {
                data = res.data;
                Debug.Log("gachaList load success");
            })
            .OnError((() => Debug.Log("gachaList load fail")))
            .Build();
        foreach (var d in data) {
            gachaList.Add(d.id);
        }
    }

    public void GachaToggle(Transform start, Transform end, float t) {
        StartCoroutine(CamMove(start, end));
        StartCoroutine(UiDisable(t));
    }

    IEnumerator CamMove(Transform start, Transform end) {
        float t=0;
        
        while (t < time) {
            t += Time.deltaTime;
            
            camera.transform.position = Vector3.Lerp(start.position, end.position, t/time);
            camera.transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, t/time);
            
            yield return null;
        }
    }

    IEnumerator UiDisable(float s) {
        yield return new WaitForSeconds(s);
        ui.SetActive(!ui.activeSelf);
        light.SetActive(!light.activeSelf);
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
        currentBanner = gachaList[boxIndex];
    }

    public void DoGacha(int gachaNum) {
        bool isFailed = false;
        // 가챠
        switch (gachaNum) {
            case 1:
                API.GatchaOnce(currentBanner)
                    .OnResponse(res => {
                        PlayerCardResponse once = res;
                        gachaResult.Add(once);
                        Debug.Log("gacha success");
                    })
                    .OnError((() => {
                        Debug.Log("gacha fail");
                        isFailed = true;
                    }))
                    .Build();
                break;
            case 10:
                API.GatchaMulti(currentBanner)
                    .OnResponse(res => {
                        gachaResult = res.cards;
                        Debug.Log("gacha success");
                    })
                    .OnError((() => {
                        Debug.Log("gacha fail");
                        isFailed = true;
                    }))
                    .Build();
                break;
        }

        //if (isFailed) return;
        gold -= gachaNum * gachaPrice;
        ChangeGoldTxt(gold);

        ui.SetActive(false);
        camera.transform.position = gachaRoomPos.transform.position;
        camera.transform.rotation = gachaRoomPos.transform.rotation;

        for (int i = 0; i < gachaNum; i++) {
            GameObject obj = Instantiate(posObj, cardGroup.transform);
            cardPos.Add(obj.transform);
            GameObject c = Instantiate(card);
            c.transform.localScale = new Vector3(10,10,1);
            int ran = Random.Range(0, 360);
            float x = Mathf.Cos(ran*Mathf.Deg2Rad) * spawnDistance;
            float z = Mathf.Sin(ran*Mathf.Deg2Rad) * spawnDistance;
            float y = Mathf.Tan(ran * Mathf.Deg2Rad) * spawnDistance;
            Vector3 newPos = particleCanvas.transform.position + new Vector3(x, y ,z);
            StartCoroutine(c.GetComponent<CardMove>().Move(newPos ,obj.transform));
        }
    }

    public void BackBtn() {
        // 이전화면
        GachaToggle(endPos, startPos, 0);
    }

    public void ToggleInfo() {
        infoToggle = !infoToggle;
        infoPanel.SetActive(infoToggle);
    }

    public void ChangeGoldTxt(int gold) {
        goldText.text = string.Format($"{gold:N0}");
    }
}

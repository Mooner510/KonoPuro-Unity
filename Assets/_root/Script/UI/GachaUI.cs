using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _root.Script.Network;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class GachaUI : MonoBehaviour {
    public static GachaUI Instance;
    
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
    [SerializeField] private List<GachaCard> cards;
    [SerializeField] private GameObject card;
    [SerializeField] private Transform gachaRoomPos;
    [SerializeField] private GameObject subCanvas;
    [SerializeField] private float spawnDistance;
    [SerializeField] private GameObject openBtn;
    [SerializeField] private GameObject backToGachaBtn;
    [SerializeField] private float allOpenSpeed;

    private void Start() {
        Instance = this;
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
        gold -= gachaNum > 1 ? (gachaNum * gachaPrice)-1 : gachaPrice;
        ChangeGoldTxt(gold);

        ui.SetActive(false);
        TpCam(gachaRoomPos.transform);

        StartCoroutine(SpawnCard(gachaNum));
    }

    public void AllOpenedCheck() {
        foreach (var c in cards) {
            if (!c.Opened) {
                return;
            }
        }
        openBtn.SetActive(false);
        backToGachaBtn.SetActive(true);
    }

    public void OpenAll() {
        openBtn.SetActive(false);
        StartCoroutine(OpeningCard());
    }

    IEnumerator OpeningCard() {
        foreach (var c in cards) {
            if (!c.Opened) {
                c.Click();
                yield return new WaitForSeconds(allOpenSpeed);
            }
        }
        backToGachaBtn.SetActive(true);
    }

    public void BackToGacha() {
        backToGachaBtn.SetActive(false);
        TpCam(endPos);
        ui.SetActive(true);
        for (int i = 0; i < cards.Count; i++) {
            Destroy(cards[i].gameObject);
            Destroy(cardPos[i].gameObject);
        }
        cards.Clear();
        cardPos.Clear();
    }

    public void TpCam(Transform pos) {
        camera.transform.position = pos.position;
        camera.transform.rotation = pos.rotation;
    }

    IEnumerator SpawnCard(int gachaNum) {
        yield return null;
        for (int i = 0; i < gachaNum; i++) {
            GameObject obj = Instantiate(posObj, cardGroup.transform);
            cardPos.Add(obj.transform);
            int ran = Random.Range(0, 360);
            float x = Mathf.Cos(ran*Mathf.Deg2Rad) * spawnDistance;
            float y = Mathf.Tan(ran * Mathf.Deg2Rad) * spawnDistance;
            float z = Mathf.Sin(ran*Mathf.Deg2Rad) * spawnDistance;
            Vector3 newPos = subCanvas.transform.position + new Vector3(x, y ,z);
            GameObject c = Instantiate(card, newPos, quaternion.identity);
            cards.Add(c.GetComponent<GachaCard>());
            c.transform.localScale = new Vector3(10,10,1);
            StartCoroutine(c.GetComponent<GachaCard>().Move(newPos ,obj.transform));
        }

        yield return new WaitForSeconds(3f);
        openBtn.SetActive(true);
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

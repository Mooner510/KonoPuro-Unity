using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DeckEditMenu : MonoBehaviour
{
    private List<DeckCardUi> equippedCardUis;
    private List<DeckCardUi> inventoryCardUis;

    private DeckCardInfoUi deckCardInfoUi;

    private DeckCardUi focusedDeckCardUi;

    private int equippedPage;
    private int inventoryPage;

    //Deck List 의 인덱스 0은 인물 카드 덱, 인덱스 1은 능력 카드 덱이다
    [SerializeField] private List<Deck> deckList = new List<Deck>();
    private                  Deck       currentEditDeck;

    private Canvas canvas;

    private bool isActive;

    private void Start()
    {
        equippedCardUis = transform.GetChild(0).GetComponentsInChildren<DeckCardUi>().ToList();
        foreach (var equippedCardUi in equippedCardUis)
        {
            Debug.Log(equippedCardUi.gameObject.name);
            equippedCardUi.isEquippedDeckUi = true;
        }

        inventoryCardUis = transform.GetChild(1).GetComponentsInChildren<DeckCardUi>().ToList();
        foreach (var inventoryCardUi in inventoryCardUis)
        {
            inventoryCardUi.isEquippedDeckUi = false;
        }

        deckCardInfoUi = GetComponentInChildren<DeckCardInfoUi>();
        canvas         = GetComponentInChildren<Canvas>();
        
        SetActive(false);
    }

    private void Update()
    {
        //실험용 메뉴 열고 닫기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            SetActive(!isActive);
        }

        if (Input.GetMouseButtonDown(0)) CheckFocus();
    }

    public void SetActive(bool active)
    {
        isActive = active;
        if (active)
        {
            Init(0);
            canvas.enabled = true;
        }
        else
        {
            foreach (var equippedCardUi in equippedCardUis) equippedCardUi.ResetUi();
            foreach (var inventoryCardUi in inventoryCardUis) inventoryCardUi.ResetUi();
            deckCardInfoUi.ResetUi();
            canvas.enabled = false;
        }
    }

    public void Init(int deckIndex)
    {
        if (deckIndex < 0 || deckIndex >= deckList.Count)
        {
            Debug.LogWarning("Not Exist Index Of DeckList");
            return;
        }
        
        foreach (var equippedCardUi in equippedCardUis) equippedCardUi.ResetUi();
        foreach (var inventoryCardUi in inventoryCardUis) inventoryCardUi.ResetUi();
        focusedDeckCardUi = null;
        deckCardInfoUi.SetUi(null);

        currentEditDeck = deckList[deckIndex];
        equippedPage    = 0;
        inventoryPage   = 0;
        RefreshAll();
    }

    private void CheckFocus()
    {
        RaycastHit hit;
        Ray        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        FocusUi(Physics.Raycast(ray, out hit) ? hit.transform.GetComponent<DeckCardUi>() : null);
    }

    public void FocusUi(DeckCardUi focusUi)
    {
        if (focusedDeckCardUi)
        {
            focusedDeckCardUi.SetFocus(false);
            deckCardInfoUi.ResetUi();
        }

        if (focusUi)
        {
            focusUi.SetFocus(true);
            deckCardInfoUi.SetUi(focusUi.cardInfo);
        }

        focusedDeckCardUi = focusUi;
    }

    public void Equip(DeckCard card)
    {
        card.isEquipped = !card.isEquipped;
        if(focusedDeckCardUi) deckCardInfoUi.SetUi(focusedDeckCardUi.cardInfo);
        if(focusedDeckCardUi.isEquippedDeckUi)
        {
            var deckCardUi = inventoryCardUis.Find((DeckCardUi cardUi) => { return cardUi.cardInfo == card; });
            if(deckCardUi) FocusUi(deckCardUi);
            else
            {
                focusedDeckCardUi.SetFocus(false);
                focusedDeckCardUi = null;
            }
        }
        RefreshAll();
    }

    private void SortCards()
    {
        //ToDo: 카드들 정렬하기
    }

    private void SetEquippedCars()
    {
        foreach (var deck in deckList)
        {
            deck.equipCards = deck.inventoryCards.Where(card => card.isEquipped).ToList();
        }
    }

    public void RefreshAll()
    {
        canvas.transform.GetChild(1).Find("EquipPageCount").GetComponent<TextMeshProUGUI>().text = $"{equippedPage + 1} / {(currentEditDeck.equipCards.Count - 1) / equippedCardUis.Count + 1}";
        canvas.transform.GetChild(1).Find("InventoryPageCount").GetComponent<TextMeshProUGUI>().text = $"{inventoryPage + 1} / {(currentEditDeck.inventoryCards.Count - 1) / inventoryCardUis.Count + 1}";
        
        SetEquippedCars();
        SortCards();
        RefreshWithListAndPage(equippedPage, currentEditDeck.equipCards, equippedCardUis);
        RefreshWithListAndPage(inventoryPage, currentEditDeck.inventoryCards, inventoryCardUis);
    }

    public void FlipEquipPage(bool pre)
    {
        var applyPage = equippedPage + (pre ? -1 : 1);
        if(applyPage < 0 || applyPage * equippedCardUis.Count >= currentEditDeck.equipCards.Count) return;
        equippedPage = applyPage;
        
        if(focusedDeckCardUi && focusedDeckCardUi.isEquippedDeckUi)
        {
            focusedDeckCardUi.SetFocus(false);
            focusedDeckCardUi = null;
            deckCardInfoUi.SetUi(null);
        }
        
        RefreshAll();
    }

    public void FlipInventoryPage(bool pre)
    {
        var applyPage = inventoryPage + (pre ? -1 : 1);
        if(applyPage < 0 || applyPage * inventoryCardUis.Count >= currentEditDeck.inventoryCards.Count) return;
        inventoryPage = applyPage;
        
        if(focusedDeckCardUi && !focusedDeckCardUi.isEquippedDeckUi)
        {
            deckCardInfoUi.SetUi(null);
            focusedDeckCardUi.SetFocus(false);
            focusedDeckCardUi = null;
        }
        
        RefreshAll();
    }
    
    public void RefreshWithListAndPage(int currentPage, List<DeckCard> cards, List<DeckCardUi> cardUis)
    {
        var pageCardCount = cardUis.Count;
        for (int i = 0; i < pageCardCount; i++)
        {
            var selectedIndex = pageCardCount * currentPage + i;
            if (cards.Count <= selectedIndex)
            {
                cardUis[i].SetUi(null);
                continue;
            }

            cardUis[i].SetUi(cards[selectedIndex]);
        }
    }
}

[Serializable]
public class Deck
{
    public int equipLimit;
    
    public List<DeckCard> equipCards     = new List<DeckCard>();
    public List<DeckCard> inventoryCards = new List<DeckCard>();
}

[Serializable]
public class DeckCard
{
    public Sprite sprite;

    //지금은 임시로 만들어 놓은 클래스
    //공용으로 사용하는 카드의 정보가 담긴 클래스가 들어갈 예정
    public CardInfo cardStats;

    public bool isEquipped;
}

//카드의 이름, 티어, 능력 등 카드의 정보를 담고있는 클래스
[Serializable]
public class CardInfo
{
    public string cardName;
}
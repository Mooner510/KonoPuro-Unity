using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Data;
using _root.Script.Network;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Deck
{
public class OriginDeckInfo
{
	public DeckResponse       activeDeck;
	public List<DeckResponse> decks;
}

public class DeckEditMenu : MonoBehaviour
{
	private Canvas canvas;

	private List<DeckCardUi> equippedCardUis;
	private List<DeckCardUi> inventoryCardUis;

	private DeckCardInfoUi cardInfoUi;

	private int equippedPage;
	private int inventoryPage;

	private List<string>       modifyingDeck;
	private PlayerCardResponse selectedCard;

	private DeckType currentDeckType = DeckType.Character;

	private bool isActive;

	private List<PlayerCardResponse> equippedFilteredCards;
	private List<PlayerCardResponse> filteredCards;

	public enum DeckType
	{
		Character,
		Skill
	}

	private void Start()
	{
		equippedCardUis = transform.GetChild(0).GetComponentsInChildren<DeckCardUi>().ToList();
		foreach (var equippedCardUi in equippedCardUis)
			equippedCardUi.isEquippedDeckUi = true;

		inventoryCardUis = transform.GetChild(1).GetComponentsInChildren<DeckCardUi>().ToList();
		foreach (var inventoryCardUi in inventoryCardUis)
			inventoryCardUi.isEquippedDeckUi = false;

		cardInfoUi = GetComponentInChildren<DeckCardInfoUi>();
		canvas     = GetComponentInChildren<Canvas>();

		var characterDeckButton = canvas.transform.Find("CharacterDeck").GetComponent<Button>();
		characterDeckButton.onClick.RemoveAllListeners();
		characterDeckButton.onClick.AddListener(() =>
		                                        { if (currentDeckType == DeckType.Character) return;
		                                          SetDeckType(DeckType.Character);
		                                          ResetPage();
		                                          RefreshAll(); });

		var skillDeckButton = canvas.transform.Find("SkillDeck").GetComponent<Button>();
		skillDeckButton.onClick.RemoveAllListeners();
		skillDeckButton.onClick.AddListener(() =>
		                                    { if (currentDeckType == DeckType.Skill) return;
		                                      SetDeckType(DeckType.Skill);
		                                      ResetPage();
		                                      RefreshAll(); });

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

		if (Input.GetMouseButtonDown(0)) CheckSelect();
	}

	public void SetActive(bool active)
	{
		ResetUis();

		if (active) Init();
		else ApplyDeck();
		isActive       = active;
		canvas.enabled = active;
	}

	void Init()
	{
		SetDeckType(DeckType.Character);
		ResetPage();

		modifyingDeck = new List<string>(UserData.Instance.ActiveDeck.deck);

		RefreshAll();
	}

	private void ApplyDeck()
	{
		var originDeck = UserData.Instance.ActiveDeck.deck;

		var deckId      = UserData.Instance.ActiveDeck.deckId;
		var addedDeck   = modifyingDeck.Except(originDeck).ToList();
		var removedDeck = originDeck.Except(modifyingDeck).ToList();

		var applyDeckRequest = new ApplyDeckRequest
		                       { activeDeckId = deckId,
		                         addition     = addedDeck,
		                         deletion     = removedDeck };

		var req = API.ApplyDeck(applyDeckRequest);
		req.OnSuccess((() => { UserData.Instance.ActiveDeck.deck = modifyingDeck; }));
		req.OnError((() => { Debug.LogWarning("Error : Deck Apply Failed"); }));
  		req.Build();
	}

	public void Equip(PlayerCardResponse card)
	{
		var deck = UserData.Instance.ActiveDeck.deck;

		var equipped = deck.Contains(card.id);
		if (equipped)
		{
			deck.Remove(card.id);
			RefreshAll();
		}
		else
		{
			deck.Add(card.id);
			RefreshAll();
		}
	}

	private void ResetUis()
	{
		foreach (var equippedCardUi in equippedCardUis) equippedCardUi.ResetUi();
		foreach (var inventoryCardUi in inventoryCardUis) inventoryCardUi.ResetUi();
		selectedCard = null;
		cardInfoUi.ResetUi();
	}

	public void ResetPage()
	{
		equippedPage  = 0;
		inventoryPage = 0;
	}

	public void SetDeckType(DeckType deckType) => currentDeckType = deckType;

	private void CheckSelect()
	{
		RaycastHit hit;
		Ray        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
		SelectCard(Physics.Raycast(ray, out hit) ? hit.transform.GetComponent<DeckCardUi>()?.card : null);
	}

	public void SelectCard(PlayerCardResponse card) => selectedCard = card;

	private void SortCards()
	{
		//ToDo: 카드들 정렬하기
	}

	private void SetFilteredCards(List<PlayerCardResponse> cards, List<string> deck, DeckType deckType)
	{
		switch (deckType)
		{
			case DeckType.Character:
				filteredCards = cards.Where(response => response.type == CardType.Student).ToList();
				break;
			case DeckType.Skill:
				filteredCards = cards.Where(response => response.type != CardType.Student).ToList();
				break;
		}

		equippedFilteredCards = filteredCards.Where(response => deck.Contains(response.id)).ToList();
	}

	public void RefreshAll()
	{
		SetFilteredCards(UserData.Instance.InventoryCards.cards, modifyingDeck, currentDeckType);

		canvas.transform.GetChild(1).Find("EquipPageCount").GetComponent<TextMeshProUGUI>().text =
				$"{equippedPage + 1} / {(equippedFilteredCards.Count - 1) / equippedCardUis.Count + 1}";
		canvas.transform.GetChild(1).Find("InventoryPageCount").GetComponent<TextMeshProUGUI>().text =
				$"{inventoryPage + 1} / {(filteredCards.Count - 1) / inventoryCardUis.Count + 1}";

		SortCards();
		RefreshWithListAndPage(equippedPage, equippedFilteredCards, equippedCardUis, modifyingDeck);
		RefreshWithListAndPage(inventoryPage, filteredCards, inventoryCardUis, modifyingDeck);
	}

	public void FlipEquipPage(bool pre)
	{
		var applyPage = equippedPage + (pre ? -1 : 1);
		if (applyPage < 0 || applyPage * equippedCardUis.Count >= equippedFilteredCards.Count) return;
		equippedPage = applyPage;

		RefreshAll();
	}

	public void FlipInventoryPage(bool pre)
	{
		var applyPage = inventoryPage + (pre ? -1 : 1);
		if (applyPage < 0 || applyPage * inventoryCardUis.Count >= filteredCards.Count) return;
		inventoryPage = applyPage;

		RefreshAll();
	}

	private void RefreshWithListAndPage(int currentPage, List<PlayerCardResponse> cards, List<DeckCardUi> cardUis,
		List<string>                        deck)
	{
		var pageCardCount = cardUis.Count;
		for (int i = 0; i < pageCardCount; i++)
		{
			var selectedIndex = pageCardCount * currentPage + i;
			if (cards.Count <= selectedIndex)
			{
				cardUis[i].ResetUi();
				continue;
			}

			var card = cards[selectedIndex];
			cardUis[i].SetUi(card, deck.Contains(cards[selectedIndex].id), selectedCard == card);
		}
	}
}
}

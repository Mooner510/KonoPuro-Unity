using System.Collections.Generic;
using System.Linq;
using _root.Script.Data;
using _root.Script.Manager;
using _root.Script.Network;
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
	private Camera cam;
	private Canvas canvas;

	private SpriteRenderer   equipBackground;
	private List<DeckCardUi> equippedCardUis;
	private List<DeckCardUi> inventoryCardUis;

	private DeckCardInfoUi cardInfoUi;

	private int equippedPage;
	private int inventoryPage;

	private List<string>       modifyingDeck;
	private PlayerCardResponse selectedCard;

	private DeckType currentDeckType = DeckType.Character;

	private bool isActive;

	private List<PlayerCardResponse> equippedCharacterCards;
	private List<PlayerCardResponse> inventoryCharacterCards;

	private List<PlayerCardResponse> equippedUseCards;
	private List<PlayerCardResponse> inventoryUseCards;

	public enum DeckType
	{
		Character = 0,
		Use       = 1
	}

	private void Start()
	{
		cam = Camera.main;

		equipBackground = transform.Find("Equip Background").GetComponent<SpriteRenderer>();

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
		                                          SetDeckType(true);
		                                          ResetPage();
		                                          RefreshAll(); });

		var skillDeckButton = canvas.transform.Find("SkillDeck").GetComponent<Button>();
		skillDeckButton.onClick.RemoveAllListeners();
		skillDeckButton.onClick.AddListener(() =>
		                                    { if (currentDeckType == DeckType.Use) return;
		                                      SetDeckType(false);
		                                      ResetPage();
		                                      RefreshAll(); });

		SetActive(false);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && isActive) CheckSelect();
	}

	public void SetActive(bool active)
	{
		ResetUis();

		if (active) Init();
		else if (isActive) ApplyDeck();
		else ResourceManager.ClearSprites();
		equipBackground.enabled = active;
		isActive                = active;
		canvas.enabled          = active;
	}

	void Init()
	{
		FilterCards(true);
		SortCards();
		SetDeckType(true);
		ResetPage();

		if (UserData.Instance.ActiveDeck.deck != null)
			modifyingDeck = new List<string>(UserData.Instance.ActiveDeck.deck);
		else
		{
			Debug.LogError("Deck Not Loaded");

			//TODO: 실험용 삭제 필요
			modifyingDeck = new();
		}

		RefreshAll();
	}

	private void ApplyDeck()
	{
		if (equippedUseCards.Count != GameStatics.deckUseCardRequired ||
		    equippedCharacterCards.Count != GameStatics.deckCharacterCardRequired)
		{
			//TODO: 덱이 형식에 맞지 않음 (카드 갯수가 모자라거나 초과함) 일 때 적용 안됨 표시
			return;
		}

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
		req.OnError((_ => { Debug.LogWarning("Error : Deck Apply Failed"); }));
		req.Build();
	}

	public void Equip(PlayerCardResponse card)
	{
		var equipped = modifyingDeck.Contains(card.id);
		if (equipped)
		{
			modifyingDeck.Remove(card.id);
			RefreshAll();
		}
		else
		{
			modifyingDeck.Add(card.id);
			RefreshAll();
		}

		cardInfoUi.SetUi(card, !equipped);
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

	public void SetDeckType(bool character)
	{
		currentDeckType = character ? DeckType.Character : DeckType.Use;
	}

	private void CheckSelect()
	{
		var ray = cam.ScreenPointToRay(Input.mousePosition);

		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
		SelectCard(Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<DeckCardUi>()?.cardData : null);
	}

	public void SelectCard(PlayerCardResponse card)
	{
		selectedCard = card;
		RefreshAll();
		cardInfoUi.SetUi(card, card != null && equippedUseCards.Contains(card));
	}

	public void FilterCards(bool init)
	{
		if (init)
		{
			
		}

		RefreshAll();
	}

	private void SortCards()
	{
		//ToDo: 카드들 정렬하기
	}

	private void SetCards(List<PlayerCardResponse> cards, List<string> deck)
	{
		inventoryCharacterCards = cards.Where(response => response.type == CardType.Student).ToList();
		inventoryUseCards       = cards.Except(inventoryCharacterCards).ToList();
		equippedCharacterCards  = inventoryCharacterCards.Where(response => deck.Contains(response.id)).ToList();
		equippedUseCards        = inventoryUseCards.Where(response => deck.Contains(response.id)).ToList();

		Debug.Log(11111111111);
		foreach (var playerCardResponse in inventoryCharacterCards)
			Debug.Log(playerCardResponse);
		Debug.Log(2222222222222);
		foreach (var playerCardResponse in inventoryUseCards)
			Debug.Log(playerCardResponse);
		Debug.Log(33333333333333);
		foreach (var playerCardResponse in equippedCharacterCards)
			Debug.Log(playerCardResponse);
		Debug.Log(444444444444444);
		foreach (var playerCardResponse in equippedUseCards)
			Debug.Log(playerCardResponse);
		Debug.Log(55555555555555555);
	}

	public void RefreshAll()
	{
		if (UserData.Instance.InventoryCards != null) SetCards(UserData.Instance.InventoryCards.cards, modifyingDeck);
		else
		{
			Debug.LogError("Inventory Cards Not Loaded");

			//TODO: 실험용 삭제 필요
			modifyingDeck = new();
		}

		var character      = currentDeckType == DeckType.Character;
		var equipCards     = character ? equippedCharacterCards : equippedUseCards;
		var inventoryCards = character ? inventoryCharacterCards : inventoryUseCards;

		canvas.transform.GetChild(1).Find("EquipPageCount").GetComponent<TextMeshProUGUI>().text =
				$"{equippedPage + 1} / {(equipCards.Count - 1) / equippedCardUis.Count + 1}";
		canvas.transform.GetChild(1).Find("InventoryPageCount").GetComponent<TextMeshProUGUI>().text =
				$"{inventoryPage + 1} / {(inventoryCards.Count - 1) / inventoryCardUis.Count + 1}";

		SortCards();
		RefreshWithListAndPage(equippedPage, equipCards, equippedCardUis, modifyingDeck);
		RefreshWithListAndPage(inventoryPage, inventoryCards, inventoryCardUis, modifyingDeck);
	}

	public void FlipEquipPage(bool pre)
	{
		var character  = currentDeckType == DeckType.Character;
		var equipCards = character ? equippedCharacterCards : equippedUseCards;

		var applyPage = equippedPage + (pre ? -1 : 1);
		if (applyPage < 0 || applyPage * equippedCardUis.Count >= equipCards.Count) return;
		equippedPage = applyPage;

		RefreshAll();
	}

	public void FlipInventoryPage(bool pre)
	{
		var character      = currentDeckType == DeckType.Character;
		var inventoryCards = character ? inventoryCharacterCards : inventoryUseCards;

		var applyPage = inventoryPage + (pre ? -1 : 1);
		if (applyPage < 0 || applyPage * inventoryCardUis.Count >= inventoryCards.Count) return;
		inventoryPage = applyPage;

		RefreshAll();
	}

	private void RefreshWithListAndPage(int currentPage, IReadOnlyList<PlayerCardResponse> cards,
		IReadOnlyList<DeckCardUi>           cardUis,     ICollection<string>               deck)
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
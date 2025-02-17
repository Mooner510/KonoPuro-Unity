using System.Collections.Generic;
using System.Linq;
using _root.Script.Data;
using _root.Script.Manager;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _root.Script.Main.Deck
{
    public class DeckEditMenu : MonoBehaviour
    {
        private Camera cam;
        private Canvas canvas;

        private DeckCardInfoUi cardInfoUi;

        private DeckType currentDeckType = DeckType.Character;

        private SpriteRenderer equipBackground;
        private List<DeckCardUi> equippedCardUis;

        private List<PlayerCardResponse> equippedCharacterCards;

        private int equippedPage;

        private List<PlayerCardResponse> equippedUseCards;

        private DeckCardFilterMenu filterMenu;
        private List<DeckCardUi> inventoryCardUis;
        private List<PlayerCardResponse> inventoryCharacterCards;
        private int inventoryPage;
        private List<PlayerCardResponse> inventoryUseCards;

        private bool isActive;

        private List<string> modifyingDeck;

        private TextMeshProUGUI[] pageCounts;

        private TMP_InputField searchField;
        private PlayerCardResponse selectedCard;

        private void Awake()
        {
            equipBackground = transform.Find("Equip Background").GetComponent<SpriteRenderer>();

            equippedCardUis = transform.Find("EquippedCards").GetComponentsInChildren<DeckCardUi>().ToList();
            foreach (var equippedCardUi in equippedCardUis)
                equippedCardUi.isEquippedDeckUi = true;

            inventoryCardUis = transform.Find("InventoryCards").GetComponentsInChildren<DeckCardUi>().ToList();
            foreach (var inventoryCardUi in inventoryCardUis)
                inventoryCardUi.isEquippedDeckUi = false;

            cardInfoUi = GetComponentInChildren<DeckCardInfoUi>();
            canvas = GetComponentInChildren<Canvas>();

            var cardTypeSelection = canvas.transform.Find("Card Type Selection");
            var characterDeckButton = cardTypeSelection.Find("CharacterDeck").GetComponent<Button>();
            var skillDeckButton = cardTypeSelection.Find("SkillDeck").GetComponent<Button>();

            characterDeckButton.onClick.RemoveAllListeners();
            characterDeckButton.onClick.AddListener(() =>
            {
                if (currentDeckType == DeckType.Character) return;
                SetDeckType(true);
                ResetPage();
                RefreshAll();
            });
            skillDeckButton.onClick.RemoveAllListeners();
            skillDeckButton.onClick.AddListener(() =>
            {
                if (currentDeckType == DeckType.Use) return;
                SetDeckType(false);
                ResetPage();
                RefreshAll();
            });

            pageCounts = canvas.transform.Find("PageButtons").GetComponentsInChildren<TextMeshProUGUI>();

            filterMenu = canvas.GetComponentInChildren<DeckCardFilterMenu>();

            searchField = canvas.GetComponentInChildren<TMP_InputField>();
        }

        private void Start()
        {
            cam = Camera.main;

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
            isActive = active;
            canvas.enabled = active;
        }

        private void Init()
        {
            SetDeckType(true);
            ResetPage();

            if (UserData.Instance.ActiveDeck.deck != null)
            {
                modifyingDeck = new List<string>(UserData.Instance.ActiveDeck.deck);
            }
            else
            {
                Debug.LogError("Deck Not Loaded");

                //TODO: 실험용 삭제 필요
                modifyingDeck = new List<string>();
            }

            searchField.text = "";
            filterMenu.Init();
            ShowFilter(false);
            RefreshAll();
        }

        private void ApplyDeck()
        {
            if (equippedUseCards.Count != GameStatics.deckUseCardRequired ||
                equippedCharacterCards.Count != GameStatics.deckCharacterCardRequired)
                //TODO: 덱이 형식에 맞지 않음 (카드 갯수가 모자라거나 초과함) 일 때 적용 안됨 표시
                return;

            var originDeck = UserData.Instance.ActiveDeck.deck;

            var deckId = UserData.Instance.ActiveDeck.deckId;
            var addedDeck = modifyingDeck.Except(originDeck).ToList();
            var removedDeck = originDeck.Except(modifyingDeck).ToList();

            var applyDeckRequest = new ApplyDeckRequest
            {
                activeDeckId = deckId,
                addition = addedDeck,
                deletion = removedDeck
            };

            var req = API.ApplyDeck(applyDeckRequest);
            req.OnSuccess(() => { UserData.Instance.ActiveDeck.deck = modifyingDeck; });
            req.OnError(_ => { Debug.LogWarning("Error : Deck Apply Failed"); });
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
            equippedPage = 0;
            inventoryPage = 0;
        }

        public void SetDeckType(bool character)
        {
            currentDeckType = character ? DeckType.Character : DeckType.Use;
        }

        private void CheckSelect()
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (EventSystem.current.IsPointerOverGameObject()) return;
            SelectCard(Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<DeckCardUi>()?.cardData : null);
        }

        private void SelectCard(PlayerCardResponse card)
        {
            selectedCard = card;
            RefreshAll();
            cardInfoUi.SetUi(card, card != null && modifyingDeck.Contains(card.id));
        }

        private void SortCards()
        {
            //ToDo: 카드들 정렬하기
        }

        private void SetCards(List<PlayerCardResponse> cards, List<string> deck)
        {
            inventoryCharacterCards = cards.Where(response => response.type == CardType.Student).ToList();
            inventoryUseCards = cards.Except(inventoryCharacterCards).ToList();
            equippedCharacterCards = inventoryCharacterCards.Where(response => deck.Contains(response.id)).ToList();
            equippedUseCards = inventoryUseCards.Where(response => deck.Contains(response.id)).ToList();

            FilterCards();
        }

        public void ShowFilter(bool show)
        {
            if (!show) RefreshAll();
            filterMenu.SetType(currentDeckType == DeckType.Character);
            filterMenu.Show(show);
        }

        private void FilterCards()
        {
            var lowerText = searchField.text.ToLower();
            if (currentDeckType == DeckType.Character)
            {
                var selectedMajors = filterMenu.SelectedMajors();
                var selectedTiers = filterMenu.SelectedTier(true);
                inventoryCharacterCards = inventoryCharacterCards
                    .Where(x =>
                    {
                        var info = GameStatics.studentCardDictionary[x.cardType];
                        return x.cardGroups.Any(majorType =>
                                selectedMajors.Contains(majorType) &&
                                selectedTiers.Contains(x.tier)) &&
                            (info.name != null && info.name.ToLower().Contains(lowerText) || info.description != null && info.description.ToLower().Contains(lowerText) || x.cardType.ToLower().Contains(lowerText));
                    })
                    .ToList();
            }
            else
            {
                var selectedTiers = filterMenu.SelectedTier(false);
                inventoryUseCards = inventoryUseCards
                    .Where(x =>
                    {
                        var info = GameStatics.defaultCardDictionary[x.cardType];
                        return selectedTiers.Contains(x.tier) && (info.name != null && info.name.ToLower().Contains(lowerText) || info.description != null && info.description.ToLower().Contains(lowerText) || x.cardType.ToLower().Contains(lowerText));
                    })
                    .ToList();
            }
            Debug.Log(searchField.text.ToLower());
        }

        public void RefreshAll()
        {
            if (UserData.Instance.InventoryCards != null)
            {
                SetCards(UserData.Instance.InventoryCards.cards, modifyingDeck);
            }
            else
            {
                Debug.LogError("Inventory Cards Not Loaded");

                //TODO: 실험용 삭제 필요
                modifyingDeck = new List<string>();
            }

            var character = currentDeckType == DeckType.Character;
            var equipCards = character ? equippedCharacterCards : equippedUseCards;
            var inventoryCards = character ? inventoryCharacterCards : inventoryUseCards;

            pageCounts[0].text = $"{equippedPage + 1} / {(equipCards.Count - 1) / equippedCardUis.Count + 1}";
            pageCounts[1].text = $"{inventoryPage + 1} / {(inventoryCards.Count - 1) / inventoryCardUis.Count + 1}";

            SortCards();
            RefreshWithListAndPage(equippedPage, equipCards, equippedCardUis, modifyingDeck);
            RefreshWithListAndPage(inventoryPage, inventoryCards, inventoryCardUis, modifyingDeck);
        }

        public void FlipEquipPage(bool pre)
        {
            var character = currentDeckType == DeckType.Character;
            var equipCards = character ? equippedCharacterCards : equippedUseCards;

            var applyPage = equippedPage + (pre ? -1 : 1);
            if (applyPage < 0 || applyPage * equippedCardUis.Count >= equipCards.Count) return;
            equippedPage = applyPage;

            RefreshAll();
        }

        public void FlipInventoryPage(bool pre)
        {
            var character = currentDeckType == DeckType.Character;
            var inventoryCards = character ? inventoryCharacterCards : inventoryUseCards;

            var applyPage = inventoryPage + (pre ? -1 : 1);
            if (applyPage < 0 || applyPage * inventoryCardUis.Count >= inventoryCards.Count) return;
            inventoryPage = applyPage;

            RefreshAll();
        }

        private void RefreshWithListAndPage(int currentPage, IReadOnlyList<PlayerCardResponse> cards,
            IReadOnlyList<DeckCardUi> cardUis, ICollection<string> deck)
        {
            var pageCardCount = cardUis.Count;
            for (var i = 0; i < pageCardCount; i++)
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

        private enum DeckType
        {
            Character = 0,
            Use = 1
        }
    }
}
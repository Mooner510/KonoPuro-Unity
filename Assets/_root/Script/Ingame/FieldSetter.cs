using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using UnityEngine;
using UnityEngine.UIElements;

namespace _root.Script.Ingame
{
    public class FieldSetter : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;

        [SerializeField] private List<IngameCard> fieldCards = new();
        [SerializeField] private List<IngameCard> studentField = new();

        public bool isMine = true;

        private List<Transform> trs = new();

        public List<IngameCard> GetStudentCards() => studentField;

        private void Awake()
        {
            var transforms = GetComponentsInChildren<Transform>().ToList();
            trs = transforms;
            trs.Remove(transform);
        }

        private void Start()
        {
            UpdateStudentPos();
            UpdateFieldCardPos();
        }

        public void UpdateField(List<GameCard> cards)
        {
            var updated = new List<IngameCard>();

            var updatedCount = cards.Count;
            var fieldCount = fieldCards.Count;

            if (updatedCount <= fieldCount)
            {
                for (var i = 0; i < fieldCount; i++)
                {
                    var card = fieldCards[i];
                    if (i < updatedCount)
                    {
                        var index = i;
                        card.Show(false, callback: () => card.LoadDisplay(cards[index]));
                        updated.Add(card);
                    }
                    else card.Show(false, true);
                }
            }
            else
            {
                for (var i = 0; i < updatedCount; i++)
                {
                    if (i < fieldCount)
                    {
                        var card = fieldCards[i];
                        var index = i;
                        card.Show(false, callback: () => card.LoadDisplay(cards[index]));
                        updated.Add(card);
                    }
                    else
                    {
                        var ingameCard = IngameCard.CreateIngameCard(cards[i]);
                        ingameCard.isMine = isMine;
                        ingameCard.type = IngameCardType.Field;
                        ingameCard.transform.rotation = Quaternion.Euler(-90, 0, 90);
                        updated.Add(ingameCard);
                    }
                }
            }

            fieldCards = updated;

            StartCoroutine(UpdateFlow());
        }

        private IEnumerator UpdateFlow()
        {
            yield return new WaitForSeconds(.5f);
            UpdateFieldCardPos();
            foreach (var ingameCard in fieldCards)
            {
                ingameCard.Show(true);
            }
        }

        public void AddNewCard(IngameCard addition)
        {
            addition.isMine = isMine;
            if (addition.type == IngameCardType.Student)
            {
                studentField.Add(addition);
                UpdateStudentPos();
            }
            else
            {
                fieldCards.Add(addition);
                addition.type = IngameCardType.Field;
                UpdateFieldCardPos();
            }
        }

        public void AddNewCards(IEnumerable<IngameCard> addition)
        {
            var ingameCards = addition as IngameCard[] ?? addition.ToArray();
            var students = ingameCards.Where(x => x.type == IngameCardType.Student).ToList();
            var fields = ingameCards.Except(students).ToList();
            studentField.AddRange(students);
            fieldCards.AddRange(fields);
            foreach (var ingameCard in fields) ingameCard.type = IngameCardType.Field;

            if (students.Count > 1) UpdateStudentPos();
            if (fields.Count > 1) UpdateFieldCardPos();
        }

        public void AddNewCard(GameStudentCard addition)
        {
            AddNewCard(IngameCard.CreateIngameCard(addition, transform.position + new Vector3(0, 1f),
                Quaternion.Euler(-90, 0, 90)));
        }

        public void AddNewCards(IEnumerable<GameStudentCard> addition)
        {
            AddNewCards(addition.Select(x => IngameCard.CreateIngameCard(x, transform.position + new Vector3(0, 1f),
                Quaternion.Euler(-90, 0, 90))));
        }

        private void UpdateStudentPos()
        {
            StudentSetPos(trs[0], studentField);
        }

        public void AddNewCard(GameCard addition)
        {
            AddNewCard(IngameCard.CreateIngameCard(addition, transform.position + new Vector3(0, 1f),
                Quaternion.Euler(-90, 0, 90)));
        }

        public void AddNewCards(IEnumerable<GameCard> addition)
        {
            AddNewCards(addition.Select(x => IngameCard.CreateIngameCard(x, transform.position + new Vector3(0, 1f),
                Quaternion.Euler(-90, 0, 90))));
        }

        private void UpdateFieldCardPos()
        {
            var skills1 = fieldCards.ToList();
            var skills2 = skills1.GetRange(0, skills1.Count / 2);
            skills1 = skills1.Except(skills2).ToList();

            SetPos(trs[1], skills1);
            SetPos(trs[2], skills2);
        }

        private void StudentSetPos(Transform field, IReadOnlyList<IngameCard> cards)
        {
            const int cardSize = 2;

            var count = cards.Count;
            var multiplyZ = field.localScale.y / count;
            var defaultPos = field.position;
            defaultPos.z -= (count - 1) * multiplyZ * 0.5f;
            defaultPos.y += .1f;
            for (var i = 0; i < count; i++)
            {
                var appliedPos = defaultPos;
                appliedPos.z += i * multiplyZ;
                cards[i].transform.localScale = Vector3.one * cardSize;
                cards[i].transform.position = appliedPos;
            }
        }

        private void SetPos(Transform field, IReadOnlyList<IngameCard> cards)
        {
            // field의 범위 안(아직은 정사각형만 가능)에 카드가 가로 세로 비율을 유지한 채 n개만큼 배열되는 기능이다
            var cardSize = new Vector2(420, 720);
            var cardRatioHeightToWidth = cardSize.y / cardSize.x;
            var fieldWidth = field.localScale.y;
            var fieldHeight = field.localScale.x;
            var count = cards.Count;

            // 이상적인 열과 행의 개수를 추론하는 수식을 사용하였다
            // 카드가 정사각형을 모두 채웠을 경우를 기준으로 row : col = cardWidth : cardHeight
            // r * c = n
            // r = c / (h/w)
            // c^2 / (h/w) = n
            // c = root(n*(h/w))
            int col = Mathf.Max(1, Mathf.FloorToInt(Mathf.Sqrt(count * cardRatioHeightToWidth)));
            int row = Mathf.Max(1, Mathf.FloorToInt((float)count / col));
            if (row * col < count) col++;
            if (row * col < count) row++;

            // cellWidth / cellHeight가 cardSize.x / cardSize.y보다 클 경우 cellWidth를 cardWidth에 대입한다
            // 그렇지 않을 경우 cellHeight / cardRatioHeightToWidth를 cardWidth에 대입한다 (cellHeight기준으로 비율을 구하여 cardWidth계산)
            var cellWidth = fieldWidth / col;
            var cellHeight = fieldHeight / row;
            float size = cellHeight / cellWidth > cardRatioHeightToWidth
                ? cellWidth
                : cellHeight / cardRatioHeightToWidth;

            //초기 위치 계산
            var defaultPos = field.position;
            defaultPos.z -= (col - 1) * cellWidth * 0.5f;
            defaultPos.x -= (row - 1) * cellHeight * 0.5f;
            defaultPos.y += .1f;
            for (var i = 0; i < count; i++)
            {
                // 스케일이기 때문에 cardWidth / cardSize.x
                // 프리팹의 width비율을 월드 기준 1에 맞추어 놓았기에 cardWidth / cardSize.x = cardWidth이다
                cards[i].transform.localScale = new Vector3(size, size, 1);
                
                //위치 설정
                var appliedPos = defaultPos;
                appliedPos.z += i % col * cellWidth;
                appliedPos.x += i / col * cellHeight;
                cards[i].transform.position = appliedPos;
            }
        }
    }
}
using System.Linq;
using _root.Script.Data;
using _root.Script.Network;
using TMPro;
using UnityEngine;

namespace _root.Script.Ingame
{
    public class IngameCardInfoUi : MonoBehaviour
    {
        private TextMeshProUGUI descriptionT;
        private TextMeshProUGUI nameT;
        private TextMeshProUGUI timeT;

        private void Awake()
        {
            var tmps = GetComponentsInChildren<TextMeshProUGUI>();
            nameT = tmps[0];
            timeT = tmps[1];
            descriptionT = tmps[2];
        }

        private void Start()
        {
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetInfo(IngameCard card)
        {
            if (card == null || card.type == IngameCardType.Deck ||
                (card.type is not (IngameCardType.Field or IngameCardType.Student) && !card.isMine))
            {
                SetActive(false);
                return;
            }

            SetActive(true);

            if (card.type == IngameCardType.Student)
            {
                var studentData = card.GetStudentData();
                if (studentData != null)
                {
                    var sInfo = GameStatics.studentCardDictionary[studentData.cardType];
                    nameT.text = sInfo.name;
                    timeT.enabled = false;
                    var description = studentData.passives.Select(passive => GameStatics.passiveDictionary[passive])
                        .Aggregate(sInfo.description,
                            (current, pInfo) =>
                                $"{current}{pInfo.name}\n{pInfo.description}\n \n");
                    descriptionT.text = description;
                }
                else
                {
                    var defaultData = card.GetData();
                    nameT.text = defaultData.cardType;
                }
            }
            else if (card.type == IngameCardType.Field)
            {
                var data = card.GetCardData();
                timeT.enabled = false;
                if (data == null) return;
                var info = GameStatics.defaultCardDictionary[data.defaultCardType];
                nameT.text        = info.name;
                descriptionT.text = info.description;
            }
            else
            {
                var data = card.GetCardData();
                if (data == null) return;
                timeT.enabled = true;
                var info = GameStatics.defaultCardDictionary[data.defaultCardType];
                nameT.text = info.name;
                timeT.text = $"사용 시간 : {info.time}";
                descriptionT.text = info.description;
            }
        }

        public void SetInfo(Tiers ability)
        {
            SetActive(true);

            timeT.enabled = true;
            var info = GameStatics.tierDictionary[ability];
            nameT.text = info.name;
            timeT.text = $"사용 시간 : {info.time}";
            descriptionT.text = info.description;
        }
    }
}
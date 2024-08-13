using System.Collections.Generic;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Main.Deck
{
    public class DeckCardFilterMenu : MonoBehaviour
    {
        private GameObject studentFilter;

        private Toggle[] studentMajors;
        private Toggle[] studentTiers;
        private GameObject useCardFilter;

        private Toggle[] useCardTiers;

        // Start is called before the first frame update
        private void Awake()
        {
            studentFilter = transform.GetChild(0).gameObject;
            useCardFilter = transform.GetChild(1).gameObject;

            studentMajors = studentFilter.transform.GetChild(0).GetComponentsInChildren<Toggle>();
            for (var i = 0; i < studentMajors.Length; i++)
                studentMajors[i].GetComponentInChildren<TextMeshProUGUI>().text = ((MajorType)i).ToString();

            studentTiers = studentFilter.transform.GetChild(1).GetComponentsInChildren<Toggle>();
            for (var i = 3; i > -1; i--)
                studentTiers[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = $"티어 {i + 1}";

            useCardTiers = useCardFilter.transform.GetChild(0).GetComponentsInChildren<Toggle>();
            for (var i = 3; i > -1; i--) useCardTiers[i].GetComponentInChildren<TextMeshProUGUI>().text = $"티어 {i + 1}";
        }

        private void Start()
        {
            SetType(true);
            Show(false);
        }

        public List<MajorType> SelectedMajors()
        {
            var selected = new List<MajorType>();
            for (var i = 0; i < studentMajors.Length; i++)
                if (studentMajors[i].isOn)
                    selected.Add((MajorType)i);
            return selected;
        }

        public List<int> SelectedTier(bool character)
        {
            var selected = new List<int>();
            if (character)
            {
                for (var i = 0; i < studentTiers.Length; i++)
                    if (studentTiers[i].isOn)
                        selected.Add(i + 1);
            }
            else
            {
                for (var i = 0; i < useCardTiers.Length; i++)
                    if (useCardTiers[i].isOn)
                        selected.Add(i + 1);
            }

            return selected;
        }

        public void Init()
        {
            foreach (var toggle in studentMajors) toggle.isOn = true;
            foreach (var toggle in studentTiers) toggle.isOn = true;
            foreach (var toggle in useCardTiers) toggle.isOn = true;
        }

        public void SetType(bool character)
        {
            studentFilter.SetActive(character);
            useCardFilter.SetActive(!character);
        }

        public void Show(bool show)
        {
            gameObject.SetActive(show);
        }
    }
}
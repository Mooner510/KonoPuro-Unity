using System;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Ingame.Ability
{
    public class AbilityButton : MonoBehaviour
    {
        [HideInInspector] public Tiers ability;

        [HideInInspector] public bool selected;

        private Button button;
        private Image icon;

        private void Awake()
        {
            icon = GetComponent<Image>();
            button = GetComponent<Button>();

            selected = false;
            SetButton(null, null, null);
        }

        public void SetButton(Tiers? tiers, Action<AbilityButton> onSelect, Action<Tiers> onClick)
        {
            if (tiers == null)
            {
                gameObject.SetActive(false);
                return;
            }

            selected = false;
            ability = tiers.Value;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (selected) onClick(tiers.Value);
                else onSelect(this);
            });

            //TODO: Tiers 이미지 로드
            // icon

            gameObject.SetActive(true);
        }

        public void SetSelect(bool active)
        {
            selected = active;
        }
    }
}
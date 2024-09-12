using System;
using _root.Script.Utils.SingleTon;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _root.Script.Manager
{
    public class ModalManager : SingleMono<ModalManager>
    {
        private TextMeshProUGUI _modalText;
        private Button _modalButtonInstance;
        private Image _modalBanner;
        private Transform _modalButtonParent;    

        protected override void Awake()
        {
            base.Awake();
            _modalBanner = transform.GetChild(1).GetComponent<Image>();
            _modalText = _modalBanner.GetComponentInChildren<TextMeshProUGUI>();
            _modalButtonInstance = GetComponentInChildren<Button>();
            _modalButtonInstance.gameObject.SetActive(false);
            _modalButtonParent = GetComponentInChildren<HorizontalLayoutGroup>().transform;
            GetComponent<Canvas>().enabled = true;
            gameObject.SetActive(false);
        }

        public static void ShowModal(string content, Color textColor = default, Color bannerColor = default, params Tuple<string, UnityAction>[] buttons)
        {
            Instance._modalText.text = content;
            Instance._modalText.color = textColor == default ? new Color(0, 0, 0)  : textColor;
            Instance._modalBanner.color = bannerColor == default ? new Color(1, 1, 1) : bannerColor;
            foreach (var button in Instance._modalButtonParent.GetComponentsInChildren<Button>()) Destroy(button.gameObject);
            foreach (var button in buttons)
            {
                var buttonObject = Instantiate(Instance._modalButtonInstance, Instance._modalButtonParent);
                buttonObject.gameObject.SetActive(true);
                buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = button.Item1;
                buttonObject.GetComponent<Button>().onClick.AddListener(button.Item2);
            }
            Instance.gameObject.SetActive(true);
        }

        public static void ResetModal()
        {
            Instance._modalText.text = "";
            Instance._modalText.color = Color.white;
            Instance._modalBanner.color = Color.white;
            foreach (var button in Instance._modalButtonParent.GetComponentsInChildren<Button>()) Destroy(button.gameObject);
            Instance.gameObject.SetActive(false);
        }
    }
}

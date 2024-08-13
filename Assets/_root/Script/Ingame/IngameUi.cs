using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Client;
using _root.Script.Data;
using _root.Script.Ingame.Ability;
using _root.Script.Network;
using InGameSettings;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _root.Script.Ingame
{
    public class IngameUi : MonoBehaviour
    {
        private AbilityManager abilityManager;
        private TextMeshProUGUI dayText;

        private GameEndUi gameEndUi;

        private IngameCardInfoUi ingameCardInfoUi;
        private EventTrigger otherDetailHover;
        private ProgressDetailUi otherProgressDetail;
        private Slider otherProgressSlider;
        private TextMeshProUGUI otherProgressText;
        private TextMeshProUGUI otherTimeText;

        private SelectionModeUi selectionModeUi;

        private EventTrigger selfDetailHover;

        private ProgressDetailUi selfProgressDetail;

        private Slider selfProgressSlider;

        private TextMeshProUGUI selfProgressText;
        private TextMeshProUGUI selfTimeText;

        private Button sleepButton;
        private TurnDisplayUi turnDisplayUi;
        private InGameSettingsManager ingameSettingsManager;

        private Animator        carduseageAnim;
        private GameObject      textpannel;
        private TextMeshProUGUI turnhandcarduse;

        private void Awake()
        {
            var textMeshProUis = GetComponentsInChildren<TextMeshProUGUI>();
            dayText = textMeshProUis[0];

            var sliderUis = GetComponentsInChildren<Slider>();
            selfProgressSlider = sliderUis[0];
            otherProgressSlider = sliderUis[1];

            selfProgressText = textMeshProUis[1];
            otherProgressText = textMeshProUis[2];

            sleepButton = GetComponentInChildren<Button>();

            selfTimeText = textMeshProUis[3];
            otherTimeText = textMeshProUis[4];

            var coverCanvas = GameObject.Find("Cover Canvas");

            var details = coverCanvas.GetComponentsInChildren<ProgressDetailUi>();
            selfProgressDetail = details[0];
            otherProgressDetail = details[1];

            var detailHovers = GetComponentsInChildren<EventTrigger>();
            selfDetailHover = detailHovers[0];
            otherDetailHover = detailHovers[1];

            ingameCardInfoUi = FindObjectOfType<IngameCardInfoUi>();

            turnDisplayUi = FindObjectOfType<TurnDisplayUi>();

            selectionModeUi = FindObjectOfType<SelectionModeUi>();

            abilityManager = FindObjectOfType<AbilityManager>();

            ingameSettingsManager = FindObjectOfType<InGameSettingsManager>();

            gameEndUi = FindObjectOfType<GameEndUi>();
            
            textpannel      = GameObject.FindGameObjectWithTag("CardUseage");
            carduseageAnim  = textpannel.GetComponent<Animator>();
            turnhandcarduse = textpannel.GetComponentInChildren<TextMeshProUGUI>();
        }

        public SelectionModeUi GetSelectionModeUi()
        {
            return selectionModeUi;
        }

        public void Init()
        {
            dayText.text = $"D - {(1 == GameStatics.dDay ? "Day" : GameStatics.dDay - 1)}";

            selfTimeText.text = "24";
            otherTimeText.text = "24";

            selfProgressSlider.value = 0;
            otherProgressSlider.value = 0;

            selfProgressText.text = "0%";
            otherProgressText.text = "0%";

            var infos = GameStatics.stageProjects;
            selfProgressDetail.Init(infos);
            otherProgressDetail.Init(infos);
        }

        public void SetHover(bool active)
        {
            selfDetailHover.enabled = active;
            otherDetailHover.enabled = active;
        }

        public void DayChange(int day)
        {
            dayText.text = $"D - {(day == GameStatics.dDay ? "Day" : GameStatics.dDay - day)}";
        }

        public void TimeChanged(int time, bool self)
        {
            (self ? GameStatics.self : GameStatics.other).time = time;
            (self ? selfTimeText : otherTimeText).text         = $"{time}";
        }

        public void SetProgress(float progress, bool self)
        {
            StartCoroutine(SetProgressLerp(progress, self));
        }

        private IEnumerator SetProgressLerp(float progress, bool self)
        {
            var progressTemp = (self ? selfProgressSlider : otherProgressSlider).value;
            var elapsedTime = 0f;
            while (elapsedTime < 2f)
            {
                var prValue = Mathf.Lerp(progressTemp, progress, elapsedTime);
                (self ? selfProgressSlider : otherProgressSlider).value = prValue;
                (self ? selfProgressText : otherProgressText).text = $"{prValue:P2}";
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            (self ? selfProgressSlider : otherProgressSlider).value = progress;
            (self ? selfProgressText : otherProgressText).text = $"{progress:P2}";
        }

        public void SetProgressDetail(Dictionary<MajorType, int> projects, bool self)
        {
            var totalProgress =
                GameStatics.CalcTotalProgress((self ? selfProgressDetail : otherProgressDetail).SetProgresses(projects));
            SetProgress(totalProgress, self);
        }

        public void SetCardInfo(IngameCard card)
        {
            ingameCardInfoUi.SetInfo(card);
        }

        public void DisplayTurn(bool myTurn)
        {
            turnDisplayUi.TurnNotify(myTurn);
            TurnDisplayDefault.TurnChange(myTurn);
        }

        public void SetInteract(bool active)
        {
            if (active != sleepButton.interactable) sleepButton.interactable = active;
        }

        public void SetAbilities(GameStudentCard card, Action<AbilityButton> onSelect, Action<Tiers> onClick)
        {
            abilityManager.SetAbilities(card, onSelect, onClick);
        }

        public void SelectAbility(AbilityButton abilityButton)
        {
            ingameCardInfoUi.SetInfo(abilityButton.ability);
            abilityManager.SelectAbility(abilityButton);
        }

        public void SetGameEnd(bool active, string info)
        {
            gameEndUi.Set(active, info);
        }

        public void SayOutLoud(string text, bool isMine)
        {
            turnhandcarduse.text = text;
            carduseageAnim.Play("CardUseage");
            textpannel.GetComponent<Image>().color = isMine ? new Color(0.34f, 0.73f, 1f, 1f) : new Color(1f, 0.42f, 0.34f, 1f);
        }
    }
}
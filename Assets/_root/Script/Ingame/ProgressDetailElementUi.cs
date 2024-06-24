using System;
using System.Collections;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Ingame
{
    public class ProgressDetailElementUi : MonoBehaviour
    {
        public MajorType type;
        [SerializeField] private float ChangeTime;

        public int maxProgress;
        public int progress;

        private Image majorIcon;
        private Slider progressBar;
        private TextMeshProUGUI progressText;

        private void Awake()
        {
            majorIcon = GetComponentInChildren<Image>();
            progressBar = GetComponentInChildren<Slider>();
            progressText = GetComponentInChildren<TextMeshProUGUI>();
            progress = 0;
        }

        public void Init(Tuple<MajorType, int> info)
        {
            var sprite = Resources.Load<Sprite>($"Major/Type={info.Item1.ToString()}");
            if (sprite) majorIcon.sprite = sprite;

            type = info.Item1;
            maxProgress = info.Item2;
            progressBar.value = 0;
            progressText.text = "0pt";
        }

        public void SetProgress(int _progress)
        {
            var progressTemp = progress;
            progress = _progress;
            StartCoroutine(ChangeProgress(progressTemp, _progress));
        }

        public void AddProgress(int _progress)
        {
            progress += _progress;
            SetProgress(progress);
        }

        private IEnumerator ChangeProgress(int TempProgress, int _progress)
        {
            var elapsedTime = 0f;
            var plus = _progress - TempProgress;
            while (elapsedTime < ChangeTime)
            {
                var pr = TempProgress + plus * (elapsedTime / ChangeTime);
                progressBar.value = Mathf.Clamp01(pr / maxProgress);
                progressText.text = $"{(int)pr}pt";
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            progress = _progress;
            progressBar.value = Mathf.Clamp01((float)progress / maxProgress);
            progressText.text = $"{progress}pt";
        }
    }
}
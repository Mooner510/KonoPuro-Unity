using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Network;
using UnityEngine;

namespace _root.Script.Ingame
{
    public class ProgressDetailUi : MonoBehaviour
    {
        [SerializeField] private GameObject elementPrefab;
        [SerializeField] private float richTime;

        private readonly List<ProgressDetailElementUi> elementUis = new();

        private CanvasGroup canvasGroup;
        private Coroutine coroutine;
        private float originAlpha;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            canvasGroup.alpha = 0;
        }

        public void Init(HashSet<Tuple<MajorType, int>> infos)
        {
            foreach (var progressDetailElementUi in elementUis)
                Destroy(progressDetailElementUi.gameObject);

            foreach (var info in infos)
            {
                var element = Instantiate(elementPrefab, transform).GetComponent<ProgressDetailElementUi>();
                element.Init(info);
                elementUis.Add(element);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>total progress</returns>
        public int SetProgresses(Dictionary<MajorType, int> projects)
        {
            var sum = 0;
            foreach (var pair in projects)
            {
                var element = elementUis.First(x => x.type == pair.Key);
                if (element) element.SetProgress(pair.Value);
                sum += Mathf.Clamp(element.progress, 0, element.maxProgress);
            }

            return sum;
        }

        public void Show(bool show)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(ShowCoroutine(show ? 1 : -1));
        }

        private IEnumerator ShowCoroutine(float destination)
        {
            var timer = canvasGroup.alpha * richTime;
            while (timer <= richTime && timer >= 0)
            {
                timer += Time.deltaTime * destination;
                canvasGroup.alpha = Mathf.Lerp(0, 1, Mathf.Clamp01(timer / richTime));
                yield return null;
            }
        }
    }
}
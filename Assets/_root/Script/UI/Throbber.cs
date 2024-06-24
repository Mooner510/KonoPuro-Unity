using System.Collections;
using UnityEngine;

namespace _root.Script.UI
{
    public class Throbber : MonoBehaviour
    {
        [SerializeField] private float speed = 100;

        private Coroutine coroutine;
        private RectTransform throbberRect;

        private void Awake()
        {
            throbberRect = transform.GetChild(0).GetComponent<RectTransform>();
        }

        private void Start()
        {
            SetActive(false);
        }

        public void SetActive(bool on)
        {
            gameObject.SetActive(on);
            if (coroutine != null) StopCoroutine(coroutine);
            if (on) coroutine = StartCoroutine(Rotate());
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                throbberRect.Rotate(new Vector3(0, 0, 1), speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
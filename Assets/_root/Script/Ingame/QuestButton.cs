using System.Collections;
using UnityEngine;

namespace _root.Script.Ingame
{
    public class QuestButton : MonoBehaviour
    {
        [SerializeField] private GameObject QuestDataUI;
        [SerializeField] private Vector3 ButtonOffPos;
        [SerializeField] private Vector3 ButtonOnPos;
        [SerializeField] private Vector3 HoverOnPos;
        [SerializeField] private float ButtonMoveTime;
        private bool isActive;
        private bool isActiveComplite = true;
        private RectTransform QuestTransform;

        private void Start()
        {
            isActive = true;
        }

        public void Click()
        {
            if (isActive)
            {
                isActive = false;
                QuestDataUI.GetComponent<QuestUi>().GetON();
                StopCoroutine(ToOn());
                StartCoroutine(ToOff());
            }
            else
            {
                isActive = true;
                QuestDataUI.GetComponent<QuestUi>().GetOff();
                StopCoroutine(ToOff());
                StartCoroutine(ToOn());
            }
        }

        public void hoverOn()
        {
            if (isActiveComplite) StartCoroutine(isHover());
        }

        public void hoverOff()
        {
            if (isActiveComplite) StartCoroutine(isDisHover());
        }

        private IEnumerator isHover()
        {
            var elapsedTime = 0f;
            while (elapsedTime < 0.3f)
            {
                if (isActive)
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                        Vector3.Lerp(ButtonOnPos, HoverOnPos, elapsedTime / 0.3f);
                else
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D = ButtonOnPos;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator isDisHover()
        {
            var elapsedTime = 0f;
            while (elapsedTime < 0.3f)
            {
                if (isActive)
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                        Vector3.Lerp(HoverOnPos, ButtonOnPos, elapsedTime / 0.3f);
                else
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D = ButtonOnPos;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ToOff()
        {
            var elapsedTime = 0f;
            var OnTransform = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
            while (elapsedTime < ButtonMoveTime)
            {
                if (!isActive)
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                        Vector3.Lerp(OnTransform, ButtonOffPos, elapsedTime / ButtonMoveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ToOn()
        {
            var elapsedTime = 0f;
            var OnTransform = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
            while (elapsedTime < ButtonMoveTime)
            {
                if (isActive)
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                        Vector3.Lerp(OnTransform, ButtonOnPos, elapsedTime / ButtonMoveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isActiveComplite = true;
        }
    }
}
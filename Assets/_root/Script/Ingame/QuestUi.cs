using System.Collections;
using UnityEngine;

namespace _root.Script.Ingame
{
    public class QuestUi : MonoBehaviour
    {
        [SerializeField] private Vector3 GetOnPos;
        [SerializeField] private Vector3 GetOffPos;
        [SerializeField] private float Speed;
        [SerializeField] private float WaitTime;
        private bool GettingOn;

        public void GetON()
        {
            GettingOn = true;
            StartCoroutine(ToGetOn());
        }

        public void GetOff()
        {
            GettingOn = false;
            StartCoroutine(ToGetOff());
        }

        private IEnumerator ToGetOn()
        {
            var NowPos = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
            var elapsedtime = 0f;
            while (elapsedtime < WaitTime)
            {
                elapsedtime += Time.deltaTime;
                yield return null;
            }

            elapsedtime = 0f;
            while (elapsedtime < Speed)
                if (GettingOn)
                {
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                        Vector3.Lerp(NowPos, GetOnPos, elapsedtime / Speed);
                    elapsedtime += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    yield break;
                }

            gameObject.GetComponent<RectTransform>().anchoredPosition3D = GetOnPos;
        }

        private IEnumerator ToGetOff()
        {
            var NowPos = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
            var elapsedtime = 0f;
            while (elapsedtime < Speed)
                if (!GettingOn)
                {
                    gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                        Vector3.Lerp(NowPos, GetOffPos, elapsedtime / Speed);
                    elapsedtime += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    yield break;
                }

            gameObject.GetComponent<RectTransform>().anchoredPosition3D = GetOffPos;
        }
    }
}
using System.Collections;
using _root.Script.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Client
{
    public class Lol : MonoBehaviour
    {
        public RawImage image;

        private void Start()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (true)
            {
                image.Alpha(0.8f);
                Debug.Log("1");
                yield return new WaitForSeconds(1);
                image.Alpha(0.2f);
                Debug.Log("0");
                yield return new WaitForSeconds(1);
            }
        }
    }
}
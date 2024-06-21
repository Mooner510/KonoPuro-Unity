using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cardinrange
{
    public class ExitGatcha : MonoBehaviour
    {
        public Transform cards;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Exitgatcha()
        {
            SceneManager.LoadScene("MainScene");
        }
        public void SkipGatcha()
        {
            gameObject.SetActive(false);
            foreach (var c in cards.GetComponentsInChildren<CardinrandomRange>()) c.OnMouseDown();
        }
    }
}

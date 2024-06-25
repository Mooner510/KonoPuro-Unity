using System;
using Config_Manager;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace cardinrange
{
    public class SpriteArray : MonoBehaviour
    {
        [SerializeField] private Sprite[] Sprites;
        private Image _spriteRenderer;
        public int randomindex;
        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer=gameObject.GetComponent<Image>();
            SpriteRandom();
        }
        public void SpriteRandom()
        {
            Random.InitState(DateTime.Now.Day);
            randomindex = Random.Range(0, Sprites.Length);
            _spriteRenderer.sprite = Sprites[randomindex];
        }

        public void Turnitoff()
        {
            SpriteRandom();
            gameObject.SetActive(false);
        }
    }
}

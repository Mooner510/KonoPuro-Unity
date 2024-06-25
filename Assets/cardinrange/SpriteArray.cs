using System;
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
            var prevSeed = Random.seed;
            Random.InitState(DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day);
            randomindex = Random.Range(0, Sprites.Length);
            _spriteRenderer.sprite = Sprites[randomindex];
            Random.InitState(prevSeed);
        }

        public void Turnitoff()
        {
            SpriteRandom();
            gameObject.SetActive(false);
        }
    }
}

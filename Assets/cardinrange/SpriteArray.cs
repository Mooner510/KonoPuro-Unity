using UnityEngine;
using UnityEngine.UI;

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

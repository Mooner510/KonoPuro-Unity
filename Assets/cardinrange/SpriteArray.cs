using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Update is called once per frame
    void Update()
    {
        
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

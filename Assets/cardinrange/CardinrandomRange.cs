using System.Linq;
using _root.Script.Card;
using _root.Script.Manager;
using UnityEngine;

namespace cardinrange
{
    public class CardinrandomRange : MonoBehaviour
    {
        public SpriteRenderer[] pickUpCard;
        public string cardId;
        public int tier;
        public GameObject skipButton, exitButton;
    
        // Start is called before the first frame update
        private void Start()
        {
        
            pickUpCard = GetComponentsInChildren<SpriteRenderer>();
            pickUpCard[1].sprite = ResourceManager.GetSprite(cardId);
        }

        public void OnMouseDown()
        {
            pickUpCard[0].enabled = false;
            pickUpCard[1].enabled = true;
            if (GetComponent<GachaSingleCardSetter>()) exitButton.SetActive(true);
            if (GetComponentInParent<GachaMultiCardSetter>())
            {
                if (GetComponentInParent<GachaMultiCardSetter>().GetComponentsInChildren<CardinrandomRange>().All(c => c.pickUpCard[1].enabled))
                {
                    skipButton.SetActive(false);
                    exitButton.SetActive(true);
                }
            }
            if (tier <= 2) return;
            GetComponentInChildren<Light>().enabled = true;
            GetComponentInChildren<Light>().color = GetColorByTier(tier);
        }

        public static Color GetColorByTier(int tier)
        {
            return tier switch
            {
                3 => Color.magenta,
                4 => Color.yellow,
                _ => Color.white
            };
        }
    }
}

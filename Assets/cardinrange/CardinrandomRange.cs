using _root.Script.Manager;
using UnityEngine;

namespace cardinrange
{
    public class CardinrandomRange : MonoBehaviour
    {
        public SpriteRenderer[] pickUpCard;
        public string cardId;
        public int tier;
    
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

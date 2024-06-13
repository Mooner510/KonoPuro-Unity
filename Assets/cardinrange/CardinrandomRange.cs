using _root.Script.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace cardinrange
{
    public class CardinrandomRange : MonoBehaviour
    {
        public SpriteRenderer[] pickUpCard;
        public string cardId;
    
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
            GetComponentInChildren<Light>().enabled = false;
        }
    }
}

using cardinrange;
using UnityEngine;

namespace _root.Script.Card
{
    public class GachaDirectionColor : MonoBehaviour
    {
        public static int maxTier;

        private void Start()
        {
            GetComponent<SpriteRenderer>().color = CardinrandomRange.GetColorByTier(maxTier);
        }
    }
}
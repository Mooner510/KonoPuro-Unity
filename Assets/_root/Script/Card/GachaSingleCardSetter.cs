using _root.Script.Network;
using cardinrange;
using UnityEngine;

namespace _root.Script.Card
{
    public class GachaSingleCardSetter : MonoBehaviour
    {
        public static PlayerCardResponse gatchaCard;

        private void Start()
        {
            GetComponentInChildren<CardinrandomRange>().cardId = gatchaCard.cardType;
            GetComponentInChildren<CardinrandomRange>().tier = gatchaCard.tier;
        }
    }
}
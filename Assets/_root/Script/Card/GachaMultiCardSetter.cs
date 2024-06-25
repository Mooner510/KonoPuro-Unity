using System.Collections.Generic;
using _root.Script.Network;
using cardinrange;
using UnityEngine;

namespace _root.Script.Card
{
    public class GachaMultiCardSetter : MonoBehaviour
    {
        public static List<PlayerCardResponse> gatchaCards = new();

        private void Awake()
        {
            var a = GetComponentsInChildren<CardinrandomRange>();
            for (var i = 0; i < gatchaCards.Count; i++)
            {
                a[i].cardId = gatchaCards[i].cardType;
                a[i].tier = gatchaCards[i].tier;
            }
        }
    }
}
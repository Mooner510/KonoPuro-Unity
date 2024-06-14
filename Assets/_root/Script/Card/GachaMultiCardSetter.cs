using System;
using System.Collections.Generic;
using _root.Script.Network;
using cardinrange;
using UnityEngine;

namespace _root.Script.Card
{
    public class GachaMultiCardSetter : MonoBehaviour
    {
        public static List<PlayerCardResponse> gatchaCards = new();
        private void Start()
        {
            var a = GetComponentsInChildren<CardinrandomRange>();
            for (var i = 0; i < gatchaCards.Count; i++)
            {
                a[i].cardId = gatchaCards[i].cardType;
                var l = a[i].GetComponentInChildren<Light>();
                if (gatchaCards[i].tier <= 2) continue;
                l.enabled = true;
                l.color = GetColorByTier(gatchaCards[i].tier);
            }
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

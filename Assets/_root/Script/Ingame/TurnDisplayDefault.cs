using TMPro;
using UnityEngine;

namespace _root.Script.Ingame
{
    public class TurnDisplayDefault : MonoBehaviour
    {
        private static TextMeshProUGUI turnText;

        public void Start()
        {
            turnText = GetComponent<TextMeshProUGUI>();
        }

        public static void TurnChange(bool isTurn)
        {
            turnText.text = isTurn ? "My Turn" : "Other Turn";
        }
    }
}
using System.Collections.Generic;
using System.Globalization;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _root.Script.Main
{
    public class GachaDirecting : MonoBehaviour
    {
        public static List<PlayerCardResponse> gatchaCards = new();
        private bool multi;
        public Transform singleCard, singleCardEndPos;
        private void Start()
        {
            multi = gatchaCards.Count > 1;
        }

        private void Update()
        {
            if (multi)
            {
                
            }
            else
            {
                singleCard.position = Vector3.Lerp(singleCard.position, singleCardEndPos.position, 0.1f);
                singleCard.rotation = Quaternion.Lerp(singleCard.rotation, singleCardEndPos.rotation, 0.1f);
                if (singleCard.rotation.ToString().Equals(singleCardEndPos.rotation.ToString()))
                {
                    SceneManager.LoadScene("CardGatchaSingle");
                }
            }
        }
    }
}

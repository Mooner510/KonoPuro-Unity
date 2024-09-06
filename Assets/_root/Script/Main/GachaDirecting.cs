using System.Collections.Generic;
using System.Linq;
using _root.Script.Card;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _root.Script.Main
{
    public class GachaDirecting : MonoBehaviour
    {
        public static List<PlayerCardResponse> gatchaCards = new();
        public Transform singleCard, singleCardEndPos;
        public Transform multiCard, multiCardEndPos;
        private bool multi;
        private Transform[] _mc;
        private Transform[] _ec;
        private Light _light;

        private void Start()
        {
            _light = GetComponent<Light>();
            multi = gatchaCards.Count > 1;
            if (!multi) return;
            _mc = multiCard.GetComponentsInChildren<MultiComponent>().Select(mc => mc.transform).ToArray();
            _ec = multiCardEndPos.GetComponentsInChildren<MultiComponent>().Select(mc => mc.transform).ToArray();
        }

        private void Update()
        {
            if (multi)
            {
                for (var i = 0; i < _mc.Length; i++)
                {
                    _mc[i].position = Vector3.Lerp(_mc[i].position, _ec[i].position, 0.1f);
                    _mc[i].rotation = Quaternion.Lerp(_mc[i].rotation, _ec[i].rotation, 0.1f);
                }

                if (_mc[0].rotation.ToString().Equals(_ec[0].rotation.ToString())) _light.intensity = Mathf.Lerp(_light.intensity, 21000, 0.05f);
                if (!(_light.intensity > 20000f)) return;
                GachaMultiCardSetter.gatchaCards = gatchaCards;
                SceneManager.LoadScene("CardGatchaMulti");
            }
            else
            {
                singleCard.position = Vector3.Lerp(singleCard.position, singleCardEndPos.position, 0.1f);
                singleCard.rotation = Quaternion.Lerp(singleCard.rotation, singleCardEndPos.rotation, 0.1f);
                if (singleCard.rotation.ToString().Equals(singleCardEndPos.rotation.ToString()))
                    _light.intensity = Mathf.Lerp(_light.intensity, 21000, 0.05f);
                if (!(_light.intensity > 20000f)) return;
                GachaSingleCardSetter.gatchaCard = gatchaCards[0];
                SceneManager.LoadScene("CardGatchaSingle");
            }
        }
    }
}
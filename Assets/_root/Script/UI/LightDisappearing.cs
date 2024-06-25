using _root.Script.Manager;
using UnityEngine;

namespace _root.Script.UI
{
    public class LightDisappearing : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.PlaySoundInstance("Audio/CARD_TIER_UNPACK");
        }

        private void Update()
        {
            GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, 0, 0.1f);
        }
    }
}
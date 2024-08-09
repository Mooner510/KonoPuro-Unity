using _root.Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityTechnologies.ParticlePack.Shared.Scripts
{
    public class ShowPlayerGold : MonoBehaviour
    {
        [FormerlySerializedAs("veiwgold")] [SerializeField] private TextMeshProUGUI viewGold;
        private bool _switchButton;
        public void OnHover()
        {
            viewGold.text = UserData.Instance.gold.ToString();
        }

        public void OnUnHover()
        {
            viewGold.text = "Gold";
        }
    }
}

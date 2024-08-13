using _root.Script.Client;
using UnityEngine;

namespace InGameSettings
{
    public class InGameSettingsManager : MonoBehaviour
    {
       [SerializeField] private GameObject settingsPanel;

        private void Start()
        {
            gameObject.SetActive(true);
            settingsPanel.SetActive(false);
        }

        public void OnButtonClick(bool isEnter)
        {
            settingsPanel.SetActive(isEnter);
        }

        public void Surrender()
        {
            NetworkClient.Send(RawProtocol.Of(106, null));  
        }
    }
}

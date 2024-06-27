using UnityEngine;

namespace _root.Script.Config
{
    public class ConfigButton : MonoBehaviour
    {
        private GameObject Configs;
        private GameObject configButton;
        private GameObject UserInfoPanel;
        private UserInfoPanel InfoScript;
        private GameObject CreditPanel;
        private Animator anim;
        private void Start()
        {
            gameObject.GetComponent<Animator>().Play("Recorded (1)");
            configButton = transform.GetChild(0).gameObject;
            Configs = FindObjectOfType<ConfigInUI>().gameObject;
            UserInfoPanel = FindObjectOfType<UserInfoPanel>().gameObject;
            InfoScript = UserInfoPanel.GetComponent<UserInfoPanel>();
            CreditPanel = GameObject.Find("CreditPanel");
            UserInfoPanel.SetActive(false);
            Configs.SetActive(false);
            CreditPanel.SetActive(false);
        }

        public void SetInteractConfigButton(bool active)
        {
            configButton.SetActive(active);
        }
        public void ConfigOn()
        {
            Configs.SetActive(true);
            Configs.GetComponent<ConfigInUI>().Init();
        }

        public void ConfigOff()
        {
            Configs.SetActive(false);
        }
        public void InfoOn()
        {
            UserInfoPanel.SetActive(true);
            InfoScript.Init();
        }
        public void InfoOff()
        {
            UserInfoPanel.SetActive(false);
        }

        public void CreditOn()
        {
            CreditPanel.SetActive(true);
        }

        public void CreditOff()
        {
            CreditPanel.SetActive(false);
        }
    }
}

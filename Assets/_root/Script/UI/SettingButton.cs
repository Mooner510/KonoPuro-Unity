using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.UI
{
    public class SettingButton : MonoBehaviour
    {
        public Transform sButton;
        public Transform settingPanel;
        private bool _settingEnable;

        private void Update()
        {
            settingPanel.localPosition =
                new Vector3(Mathf.Lerp(settingPanel.localPosition.x, _settingEnable ? 708 : 1095, 0.1f),
                    settingPanel.localPosition.y, settingPanel.localPosition.z);
            var rotation = sButton.localRotation;
            sButton.localRotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y,
                Mathf.Lerp(rotation.eulerAngles.z, _settingEnable ? 359.99f : 0, 0.1f));
            foreach (var image in settingPanel.gameObject.GetComponentsInChildren<Image>())
                image.color = Color.Lerp(image.color, _settingEnable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0),
                    0.1f);
        }

        public void SButton()
        {
            _settingEnable = !_settingEnable;
        }

        public void Resign()
        {
            Debug.Log("Resigned");
            // todo Resign-Code
        }
    }
}
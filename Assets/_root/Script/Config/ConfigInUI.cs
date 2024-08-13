using _root.Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _root.Script.Config
{
    public class ConfigInUI : MonoBehaviour
    {
        private GameObject Volume;
        private Slider VolumeSlider;
        private bool Muted = false;
        //private SoundVolumeManager VolumeManager;
        private GameObject Joke;
        private Slider JokeSlider;
        private GameObject FPS_Limit;
        private Slider FPS_Slider;
        private TextMeshProUGUI FPS_Value;
        public bool isChange = false;

        private void Awake()
        {
            Volume = transform.GetChild(2).gameObject;
            VolumeSlider = Volume.GetComponent<Slider>();
            Joke = transform.GetChild(3).gameObject;
            JokeSlider = Joke.GetComponent<Slider>();
            FPS_Limit = transform.GetChild(4).gameObject;
            FPS_Slider = FPS_Limit.GetComponent<Slider>();
            FPS_Value = FPS_Limit.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public void Init()
        {
            VolumeSlider.value = ConfigManager.ConfigData.SoundVolume;
            if (VolumeSlider.value == 0f)
                Muted = true;
            JokeSlider.value = ConfigManager.ConfigData.Light;
            FPS_Slider.maxValue = (int)Screen.currentResolution.refreshRateRatio.value+1;
            FPS_Slider.value = ConfigManager.ConfigData.FPSLimit;
            Debug.Log(Screen.currentResolution.refreshRateRatio.value);
            FPS_Value.text = FPS_Slider.value >= (float)Screen.currentResolution.refreshRateRatio.value ? "제한 없음" : $"FPS : {(int)FPS_Slider.value}";
            isChange = false;
        }
        public void GetOff()
        {
            ConfigManager.ConfigData.SoundVolume = VolumeSlider.value;
            ConfigManager.ConfigData.Light = JokeSlider.value;
            ConfigManager.ConfigData.FPSLimit = (int)FPS_Slider.value;
            AudioManager.VolumeInitInstance();
            Settings.Save();
            if (isChange)
            {
                // Todo 저장 할지 말지 물어보는 패널 띄우기 @whitefish2n2
                gameObject.SetActive(false);
                Settings.Save();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        //값 안저장
        public void DisAcceptChange()
        {
            Screen.brightness = ConfigManager.ConfigData.Light;
        }
        public void ChangeVolume()
        {
            isChange = true;
            Muted = VolumeSlider.value == 0f;
        }

        public void Mute()
        {
            isChange = true;
            VolumeSlider.value = !Muted ? 0f : 0.5f;
        }
        public void ChangeFPS()
        {
            FPS_Value.text = FPS_Slider.value >= (float)Screen.currentResolution.refreshRateRatio.value ? "제한 없음" : $"FPS : {(int)FPS_Slider.value}";
            isChange = true;
        }

        private bool _isJoking;
        public void JokingSystem()
        {
            _isJoking = false;
            JokeSlider.value = 0.05f;
        }
        public void JokingPrepare()
        {
            _isJoking = true;
        }

        public void JokingChange()
        {
            if (!_isJoking) JokingSystem();
        }
    }
}

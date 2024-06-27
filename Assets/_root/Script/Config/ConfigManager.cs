using _root.Script.Manager;
using UnityEngine;

namespace _root.Script.Config
{
    public class ConfigManager : MonoBehaviour
    {
        public static Settings.Data ConfigData;
        private void Awake()
        {
            ConfigData = new Settings.Data();
            DontDestroyOnLoad(gameObject);
            Settings.Load();
        }

        private void Start()
        {
            Screen.brightness = ConfigData.Light;
            if (ConfigData.FPSLimit > 0)
            {
                Application.targetFrameRate = ConfigData.FPSLimit;
            }
            AudioManager.VolumeInitInstance();
        }
    }
}


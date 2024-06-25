using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config_Manager
{
    public class ConfigManager : MonoBehaviour
    {
        public static Settings.Data ConfigData;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ConfigData = Settings.Load();
            Screen.brightness = ConfigData.Light;
            if (ConfigData.FPS_Limit > 0)
            {
                Application.targetFrameRate = ConfigData.FPS_Limit;
            }
        }
    }
}


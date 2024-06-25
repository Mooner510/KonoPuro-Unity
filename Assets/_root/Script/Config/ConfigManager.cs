using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Manager;
using UnityEngine;

namespace Config_Manager
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
            if (ConfigData.FPS_Limit > 0)
            {
                Application.targetFrameRate = ConfigData.FPS_Limit;
            }
            AudioManager.VolumeInitInstance();
        }
    }
}


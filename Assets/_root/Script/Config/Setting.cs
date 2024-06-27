using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace _root.Script.Config
{
    public static class Settings
    {
        public class Data
        {
            public float SoundVolume = 1f;
            public float Light = 0.05f;
            public int FPSLimit = 60;
        }
        public static void Save()
        {
            File.WriteAllBytes(Application.dataPath + "/Config.json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ConfigManager.ConfigData)));
        }
        public static void Load()
        {
            try
            {
                ConfigManager.ConfigData = JsonConvert.DeserializeObject<Data>(Encoding.UTF8.GetString(File.ReadAllBytes(Application.dataPath + "/Config.json")));
            }
            catch (Exception)
            {
                Debug.LogError("Create New Save File.");
                ConfigManager.ConfigData = new Data();
                Save();
            }
        }
    }
}


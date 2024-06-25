using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Config_Manager
{
    public static class Settings
    {
        public class Data
        {
            public float SoundVolume = 1f;
            public float Light = 1f;
            public int FPS_Limit = 60;
        }
        public static void Save(Data Setting)
        {
            var stream = new FileStream(Application.dataPath + "/_root/Script/Config/Config.json", FileMode.OpenOrCreate);
            var jsonData = JsonConvert.SerializeObject(Setting);
            var data = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
        public static Data Load()
        {
            try
            {
                var stream = new FileStream(Application.dataPath + "/_root/Script/Config/Config.json", FileMode.Open);
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                stream.Close();
                var jsonData = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<Data>(jsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Data NewData = new Data() { FPS_Limit = 60, Light = 1, SoundVolume = 1 };
                Save(NewData);
                var stream = new FileStream(Application.dataPath + "/_root/Script/Config/Config.json", FileMode.Open);
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                stream.Close();
                var jsonData = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<Data>(jsonData);
            }
        }
    }
}


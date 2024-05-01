using System.Security.Cryptography;
using System.Text;
using UnityEngine;
namespace _root.Script.Utils
{
    public class EncryptedPlayerPrefs
    {
        private const string PrivateKey = "7AOrGfAaPQchx1abvmQj";
        private static readonly string[] Keys = { "tjr21agF", "Oq2mBsrT", "FoAm2Atr", "j2A1fogA", "U8otAbn1" };

        private static string Md5(string strToEncrypt)
        {
            UTF8Encoding ue = new UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        private static void SaveEncryption(string key, string type, string value)
        {
            int keyIndex = (int)Mathf.Floor(Random.value * Keys.Length);
            string secretKey = Keys[keyIndex];
            string check = Md5(type + "_" + PrivateKey + "_" + secretKey + "_" + value);
            PlayerPrefs.SetString(key + "_encryption_check", check);
            PlayerPrefs.SetInt(key + "_used_key", keyIndex);
        }

        private static bool CheckEncryption(string key, string type, string value)
        {
            int keyIndex = PlayerPrefs.GetInt(key + "_used_key");
            string secretKey = Keys[keyIndex];
            string check = Md5(type + "_" + PrivateKey + "_" + secretKey + "_" + value);
            if (!PlayerPrefs.HasKey(key + "_encryption_check")) return false;
            string storedCheck = PlayerPrefs.GetString(key + "_encryption_check");
            return storedCheck == check;
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            SaveEncryption(key, "int", value.ToString());
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            SaveEncryption(key, "float", Mathf.Floor(value * 1000).ToString());
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            SaveEncryption(key, "string", value);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            int value = PlayerPrefs.GetInt(key);
            return !CheckEncryption(key, "int", value.ToString()) ? defaultValue : value;
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            float value = PlayerPrefs.GetFloat(key);
            return !CheckEncryption(key, "float", Mathf.Floor(value * 1000).ToString()) ? defaultValue : value;
        }

        public static string GetString(string key, string defaultValue = "")
        {
            string value = PlayerPrefs.GetString(key);
            return !CheckEncryption(key, "string", value) ? defaultValue : value;
        }

        public static bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(key + "_encryption_check");
            PlayerPrefs.DeleteKey(key + "_used_key");
        }

    }
}
using System;
using _root.Script.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _root.Script.Card
{
    public class GachaBuildup : MonoBehaviour
    {
        public void GachaBuildup1Started()
        {
            AudioManager.PlaySoundInstance("Audio/GACHA-1");
        }
        public void GachaBuildup2Audio(string path)
        {
            AudioManager.PlaySoundInstance(path);
        }

        public void GachaBuildup1Ended()
        {
            SceneManager.LoadScene("GachaDirectingBuildupScene");
        }
        public void GachaBuildup2Ended()
        {
            SceneManager.LoadScene("GachaDirectingScene");
        }
    }
}

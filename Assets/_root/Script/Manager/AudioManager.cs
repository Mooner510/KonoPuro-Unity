using System;
using System.Collections.Generic;
using UnityEngine;

namespace _root.Script.Manager
{
    public class AudioManager : MonoBehaviour
    {
        private static readonly Dictionary<string, AudioClip> Clips = new();
        private static AudioManager instance;

        public void Start()
        {
            instance = this;
        }

        public static AudioManager GetInstance()
        {
            return instance;
        }

        public static void PlaySoundInstance(string path)
        {
            GetInstance().PlaySound(path);
        }
        
        public void PlaySound(string path)
        {
            GetComponent<AudioSource>().PlayOneShot(GetClip(path));
        }

        private static AudioClip GetClip(string path)
        {
            if (Clips.TryGetValue(path, out var clip)) return clip;
            clip = Resources.Load<AudioClip>(path);
            if (!clip) return null;
            Clips[path] = clip;
            return clip;
        }
    }
}

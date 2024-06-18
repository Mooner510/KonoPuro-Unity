using System;
using System.Collections.Generic;
using _root.Script.Utils.SingleTon;
using UnityEngine;

namespace _root.Script.Manager
{
    [RequireComponent(typeof(AudioSource))] 
    public class AudioManager : SingleMono<AudioManager>
    {
        private static readonly Dictionary<string, AudioClip> Clips = new();

        public static void PlaySoundInstance(string path)
        {
            Instance.PlaySound(path);
        }

        public void PlaySound(string path)
        {
            var clip = GetClip(path);
            if(clip) GetComponent<AudioSource>().PlayOneShot(clip);
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

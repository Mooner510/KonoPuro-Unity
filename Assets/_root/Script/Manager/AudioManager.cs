using System;
using System.Collections.Generic;
using _root.Script.Config;
using _root.Script.Utils.SingleTon;
using UnityEngine;

namespace _root.Script.Manager
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingleMono<AudioManager>
    {
        private static readonly Dictionary<string, AudioClip> Clips = new();
        private AudioSource _audioSource;
        private readonly List<Action<AudioSource>> _eventHandlers = new();

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_eventHandlers.Count == 0 || _audioSource.isPlaying) return;
            for (var i = 0; i < _eventHandlers.Count; i++) _eventHandlers[i](_audioSource);
        }

        public static void AddEventHandlerInstance(Action<AudioSource> action)
        {
            Instance.AddEventHandler(action);
        }

        public static void ClearEventHandlerInstance()
        {
            Instance.ClearEventHandler();
        }
        
        public static void SetAsBackgroundMusicInstance(string path, bool loop = false)
        {
            Instance.SetAsBackgroundMusic(path, loop);
        }

        public static void StopAllSoundsInstance()
        {
            Instance.StopAllSounds();
        }

        public static void VolumeInitInstance()
        {
            Instance.VolumeInit();
        }

        public static void PlaySoundInstance(string path)
        {
            Instance.PlaySound(path);
        }

        private void PlaySound(string path)
        {
            var clip = GetClip(path);
            if (clip) _audioSource.PlayOneShot(clip);
        }

        private void SetAsBackgroundMusic(string path, bool loop = false)
        {
            var clip = GetClip(path);
            if (!clip) return;
            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        private void AddEventHandler(Action<AudioSource> action)
        {
            _eventHandlers.Add(action);
        }

        private void ClearEventHandler()
        {
            _eventHandlers.Clear();
        }

        private void StopAllSounds()
        {
            _audioSource.Stop();
        }

        private void VolumeInit()
        {
            _audioSource.volume = ConfigManager.ConfigData.SoundVolume;
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
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
        private AudioSource _audioSource;
        private readonly List<Action<AudioSource>> _eventHandlers = new();

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_eventHandlers.Count == 0 || _audioSource.isPlaying) return;
            foreach (var eventHandler in _eventHandlers) eventHandler.Invoke(_audioSource);
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

        public static void PlaySoundInstance(string path)
        {
            Instance.PlaySound(path);
        }

        public void PlaySound(string path)
        {
            var clip = GetClip(path);
            if (clip) _audioSource.PlayOneShot(clip);
        }

        public void SetAsBackgroundMusic(string path, bool loop = false)
        {
            var clip = GetClip(path);
            if (!clip) return;
            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        public void AddEventHandler(Action<AudioSource> action)
        {
            _eventHandlers.Add(action);
        }

        public void ClearEventHandler()
        {
            _eventHandlers.Clear();
        }

        public void StopAllSounds()
        {
            _audioSource.Stop();
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
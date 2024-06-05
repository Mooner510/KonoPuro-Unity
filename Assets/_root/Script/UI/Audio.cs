using System;
using UnityEngine;

namespace _root.Script.UI
{
    public class Audio : MonoBehaviour
    {
        private AudioSource _audioSource;
        public AudioClip loop;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.clip = loop;
            _audioSource.Play();
            _audioSource.loop = true;
        }
    }
}

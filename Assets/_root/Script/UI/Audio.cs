using UnityEngine;

namespace _root.Script.UI
{
    public class Audio : MonoBehaviour
    {
        public AudioClip loop;
        private AudioSource _audioSource;

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
using UnityEngine;

namespace _root.Script.UI
{
    public class Audio : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip loop;
    
        private void Update()
        {
            if (audioSource.isPlaying) return;
            audioSource.clip = loop;
            audioSource.Play();
            audioSource.loop = true;
        }
    }
}
